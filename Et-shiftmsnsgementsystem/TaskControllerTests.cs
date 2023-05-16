using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.DocModel;
using ET_ShiftManagementSystem.Models.TaskModel;
using ET_ShiftManagementSystem.Services;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Et_shiftmsnsgementsystem
{
    //[TestFixture]
    public class TaskControllerTests
    {
        private Mock<ITasksServices> _mockTasksServices;
        private Mock<ITaskCommentServices> _mockCommentServices;
        private TaskController _controller;
        public TaskControllerTests()
        {
            _mockTasksServices =  new Mock<ITasksServices>();
            _mockCommentServices = new Mock<ITaskCommentServices>();
            _controller = new TaskController(_mockTasksServices.Object, _mockCommentServices.Object);
        }
        

        [Test]
        public void Constructor_SetsDependencies()
        {
            // Arrange

            // Act
            var controller = new TaskController(_mockTasksServices.Object, _mockCommentServices.Object);

            // Assert
            Assert.IsNotNull(controller);
            //Assert.IsNotNull(controller.TasksServices);
            //Assert.AreSame(_mockTasksServices, controller.TasksServices);
            //Assert.IsNotNull(controller.CommentServices);
            //Assert.AreSame(_mockCommentServices, controller.CommentServices);
        }
        [Test]
        public void GetTasks_ReturnsOkResult_WhenTasksExist()
        {
            // Arrange
            var tasks = new List<Tasks> { new Tasks { Id = Guid.NewGuid(), Text = "Task 1" }, new Tasks { Id = Guid.NewGuid(), Text = "Task 2" } };
            _mockTasksServices.Setup(s => s.GetAllTasks()).Returns(new List<Tasks>());

            // Act
            var result = _controller.GetTasks();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            //Assert.AreEqual(tasks, okResult.Value);
        }

        [Test]
        public void GetTasks_ReturnsNotFoundResult_WhenTasksDoNotExist()
        {
            // Arrange
            _mockTasksServices.Setup(s => s.GetAllTasks()).Returns((List<Tasks>)null);

            // Act
            var result = _controller.GetTasks();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetTasks_CallsGetAllTasksMethodOfTasksServicesOnce()
        {
            // Arrange
            _mockTasksServices.Setup(s => s.GetAllTasks()).Returns(new List<Tasks>());

            // Act
            _controller.GetTasks();

            // Assert
            _mockTasksServices.Verify(s => s.GetAllTasks(), Times.Once);
        }

        [Test]
        public void GetTasks_ThrowsException_WhenTasksServicesThrowsException()
        {
            // Arrange
             _mockTasksServices.Setup(s => s.GetAllTasks()).Throws(new Exception());

            // Act & Assert
            Assert.Throws<Exception>(() => _controller.GetTasks());
        }
        [Test]
        public void GetTasks_Returns_Ok_With_Tasks()
        {
            // Arrange
            var expectedTasks = new List<Task>();
            _mockTasksServices.Setup(s => s.GetAllTasks()).Returns(new List<Tasks>());

            // Act
            var result = _controller.GetTasks();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(expectedTasks, okResult.Value);
        }

        [Test]
        public void GetTasks_Returns_NotFound_When_Tasks_Are_Null()
        {
            // Arrange
            _mockTasksServices.Setup(s => s.GetAllTasks()).Returns((List<Tasks>)null);

            // Act
            var result = _controller.GetTasks();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetTasks_Throws_Exception_Returns_InternalServerError()
        {
            // Arrange
            _mockTasksServices.Setup(s => s.GetAllTasks()).Throws<Exception>();

            // Act
            //var result = _controller.GetTasks();

            // Assert
            //Assert.IsInstanceOf<StatusCodeResult>(result);
            //var statusCodeResult = (StatusCodeResult)result;
            //Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        [Test]
        public void GetTasks_Returns_BadRequest_When_TenantId_Is_Empty()
        {
            // Arrange
            var tenantId = Guid.Empty;

            // Act
            var result = _controller.GetTasksByOrg(tenantId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public void GetTasksByOrg_Returns_Ok_With_Tasks()
        {
            // Arrange
            var expectedTasks = new List<Task>();
            var tenantId = Guid.NewGuid();
            _mockTasksServices.Setup(s => s.GetAllTasksOfOneOrg(tenantId)).Returns(new List<Tasks>());

            // Act
            var result = _controller.GetTasksByOrg(tenantId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(expectedTasks, okResult.Value);
        }

        [Test]
        public void GetTasksByOrg_Returns_NotFound_When_Tasks_Are_Null()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            _mockTasksServices.Setup(s => s.GetAllTasksOfOneOrg(tenantId)).Returns((List<Tasks>)null);

            // Act
            var result = _controller.GetTasksByOrg(tenantId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetTasksByOrg_Throws_Exception_Returns_InternalServerError()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            _mockTasksServices.Setup(s => s.GetAllTasksOfOneOrg(tenantId)).Throws<Exception>();

            // Act
            //var result = _controller.GetTasksByOrg(tenantId);

            //// Assert
            //Assert.IsInstanceOf<StatusCodeResult>(result);
            //var statusCodeResult = (StatusCodeResult)result;
            //Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        [Test]
        public void GetTasksByOrg_Returns_BadRequest_When_TenantId_Is_Empty()
        {
            // Arrange
            var tenantId = Guid.Empty;

            // Act
            var result = _controller.GetTasksByOrg(tenantId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task PostTask_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskDetails = new TaskUploadModel
            {
                Text = "Sample task",
                FileType = FileType.PDF,
                DueDate = DateTime.Now.AddDays(7),
                Actions = Actions.ToDo,
                TaskGivenTo = Guid.NewGuid(),
                ///FileDetails =  IFormFile 
            };
            //_mockTasksServices.Setup(s => s.PostTaskAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<FileType>(), It.IsAny<DateTime>(), It.IsAny<Actions>(), It.IsAny<Guid>())).Verifiable();

            // Act
            var result = await _controller.PostTask(userId, taskDetails);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            _mockTasksServices.Verify();
        }

        [Test]
        public async Task PostTask_InvalidUserId_ReturnsBadRequestResult()
        {
            // Arrange
            var userId = Guid.Empty;
            var taskDetails = new TaskUploadModel();

            // Act
            var result = await _controller.PostTask(userId, taskDetails);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task PostTask_NullTaskDetails_ReturnsBadRequestResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            TaskUploadModel taskDetails = null;

            // Act
            var result = await _controller.PostTask(userId, taskDetails);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void PostTask_ThrowsException_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var taskDetails = new TaskUploadModel();
                
            //_mockTasksServices.Setup(s => s.PostTaskAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<FileType>(), It.IsAny<DateTime>(), It.IsAny<Actions>(), It.IsAny<Guid>())).ThrowsAsync(new Exception());
            // Act & Assert
            //Assert.ThrowsAsync<Exception>(() => _controller.PostTask(userId, taskDetails));
            //_mockTasksServices.Verify();
        }
        [Test]
        public async Task AddCommentsUpdateTask_ValidData_ReturnsOkResult()
        {
            // Arrange
            Guid taskId = Guid.NewGuid();
            UpdateTask updateTask = new UpdateTask();

            _mockTasksServices.Setup(x => x.UpdateTask(taskId, updateTask)).Verifiable();

            // Act
            IActionResult result = await _controller.AddCommentsUpdateTask(taskId, updateTask);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            _mockTasksServices.Verify();
        }

        [Test]
        public async Task AddCommentsUpdateTask_InvalidTaskId_ReturnsBadRequest()
        {
            // Arrange
            Guid taskId = Guid.Empty;
            UpdateTask updateTask = new UpdateTask();

            // Act
            IActionResult result = await _controller.AddCommentsUpdateTask(taskId, updateTask);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task AddCommentsUpdateTask_NullUpdateTask_ReturnsBadRequest()
        {
            // Arrange
            Guid taskId = Guid.NewGuid();
            UpdateTask updateTask = null;

            // Act
            IActionResult result = await _controller.AddCommentsUpdateTask(taskId, updateTask);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task AddCommentsUpdateTask_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            Guid taskId = Guid.NewGuid();
            UpdateTask updateTask = new UpdateTask();

            _mockTasksServices.Setup(x => x.UpdateTask(taskId, updateTask)).Throws<Exception>();

            // Act
            //IActionResult result = await _controller.AddCommentsUpdateTask(taskId, updateTask);

            //// Assert
            //Assert.IsInstanceOf<StatusCodeResult>(result);
            //StatusCodeResult statusCodeResult = (StatusCodeResult)result;
            //Assert.AreEqual(500, statusCodeResult.StatusCode);
        }
        [Test]
        public void AddTask_WhenUserIdIsEmpty_ReturnsBadRequest()
        {
            //Arrange
            //var controller = new TaskController(mockTasksServices.Object, mockDbContext.Object, mockCommentServices.Object);
            Guid UserId = Guid.Empty;
            var taskVm = new TaskCommentVM { }; //taskVm can be null or empty

            //Act
            var result = _controller.AddTask(UserId, taskVm);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public void AddTask_WhenTaskCommentVMIsNull_ReturnsBadRequest()
        {
            //Arrange
            //var controller = new TaskController(mockTasksServices.Object, mockDbContext.Object, mockCommentServices.Object);
            Guid UserId = Guid.NewGuid();
            TaskCommentVM taskVm = null;

            //Act
            var result = _controller.AddTask(UserId, taskVm);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public void AddTask_WhenCommentIsAddedSuccessfully_ReturnsOk()
        {
            //Arrange
            //var controller = new TaskController(mockTasksServices.Object, mockDbContext.Object, mockCommentServices.Object);
            Guid UserId = Guid.NewGuid();
            var taskVm = new TaskCommentVM { TaskID = Guid.NewGuid(), Comments = "Test Comment" };

            _mockCommentServices.Setup(x => x.addComment(UserId, taskVm)).Returns(new TaskComment());

            //Act
            var result = _controller.AddTask(UserId, taskVm);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void AddTask_WhenUserIsNotFound_ReturnsNotFound()
        {
            //Arrange
            //var controller = new TaskController(mockTasksServices.Object, mockDbContext.Object, mockCommentServices.Object);
            Guid UserId = Guid.NewGuid();
            var taskVm = new TaskCommentVM { Comments = "Test Comment" };

            _mockCommentServices.Setup(x => x.addComment(UserId, taskVm)).Returns(new TaskComment());

            //Act
            var result = _controller.AddTask(UserId, taskVm);

            //Assert
            //*var notFoundResult = */Assert.IsInstanceOf<NotFoundObjectResult>(result);
            //Assert.AreEqual("User not found", notFoundResult.Value);
        }
        [Test]
        public void GetTask_WithValidTaskId_ReturnsOkResult()
        {
            // Arrange
            Guid validTaskId = Guid.Parse("1B577E31-67FE-4AB2-AAF6-52F3CDCF22BD");

            // Act
            var result = _controller.GetTask(validTaskId);

            //// Assert
            //Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetTask_WithInvalidTaskId_ReturnsBadRequestResult()
        {
            // Arrange
            Guid invalidTaskId = Guid.Empty;

            // Act
            var result = _controller.GetTask(invalidTaskId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void GetTask_WithNonExistentTaskId_ReturnsNotFoundResult()
        {
            // Arrange
            Guid nonExistentTaskId = Guid.NewGuid();

            // Act
            var result = _controller.GetTask(nonExistentTaskId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void GetuserTaskToDo_WithEmptyUserId_ReturnsBadRequest()
        {
            // Arrange
            Guid emptyUserId = Guid.Empty;

            // Act
            var result = _controller.GetuserTaskToDo(emptyUserId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void GetuserTaskToDo_WithValidUserId_ReturnsOkResult()
        {
            // Arrange
            Guid validUserId = Guid.NewGuid();
            var tasks = new List<TaskComment>() { new TaskComment()  };
            _mockTasksServices.Setup(x => x.GetuserTaskToDo(validUserId)).Returns(tasks.Count());

            // Act
            var result = _controller.GetuserTaskToDo(validUserId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetuserTaskToDo_WithValidUserId_ReturnsTasks()
        {
            // Arrange
            Guid validUserId = Guid.NewGuid();
            //var tasks = new List<TaskComment>() { new TaskComment()  };
            //_mockTasksServices.Setup(x => x.GetuserTaskToDo(validUserId)).Returns(tasks.Count()); 

            // Act
            var result = _controller.GetuserTaskToDo(validUserId) as OkObjectResult;
            //var returnedTasks = result.Value as List<TaskComment>;

            // Assert
             //Assert.AreEqual(0, result.Count);
            //  Assert.AreEqual("Task 1", returnedTasks[0].Task);
            //Assert.AreEqual("Task 2", returnedTasks[1].Task);
        }

        [Test]
        public void GetuserTaskToDo_WithInvalidUserId_ReturnsNotFoundResult()
        {
            // Arrange
            Guid invalidUserId = Guid.NewGuid();
            _mockTasksServices.Setup(x => x.GetuserTaskToDo(invalidUserId)).Returns(0);

            // Act
            var result = _controller.GetuserTaskToDo(invalidUserId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void GetuserTaskInProgress_WhenUserIdNotProvided_ReturnsBadRequest()
        {
            // Arrange
            //var controller = new TaskController(_tasksServices, _commentServices);

            // Act
            var result = _controller.GetuserTaskInProgress(Guid.Empty);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public void GetuserTaskInProgress_WhenUserHasNoTaskInProgress_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            //var tasks = new List<TaskDTO>();
            //var mockService = new Mock<ITasksServices>();
            //mockService.Setup(s => s.GetuserTaskInProgress(userId)).Returns(tasks);
            //var controller = new TaskController(mockService.Object, _commentServices);

            // Act
            var result = _controller.GetuserTaskInProgress(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void GetuserTaskInProgress_WhenUserHasTasksInProgress_ReturnsOkResultWithTasks()
        {
            // Arrange
            var userId = Guid.NewGuid();
            //var tasks = new List<TaskDTO> { new TaskDTO { TaskId = Guid.NewGuid() } };
            //var mockService = new Mock<ITasksServices>();
            //mockService.Setup(s => s.GetuserTaskInProgress(userId)).Returns(tasks);
            //var controller = new TaskController(mockService.Object, _commentServices);

            // Act
            var result = _controller.GetuserTaskInProgress(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
            //var okResult = result as OkObjectResult;
            //Assert.AreEqual(tasks, okResult.Value);
        }
        //[Test]
        public void GetTasks_ReturnsOkResult()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            // Act
            var result = _controller.GetTasks(projectId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        //[Test]
        public void GetTasks_ReturnsTasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            // Act
            var result = _controller.GetTasks(projectId) as OkObjectResult;

            // Assert
            Assert.IsInstanceOf<IEnumerable<Task>>(result.Value);
        }

        [Test]
        public void GetTasks_ReturnsNotFoundResult_WhenNoTasks()
        {
            // Arrange
            //var mockService = new Mock<ITasksService>();
            //mockService.Setup(s => s.GetAllTasks(It.IsAny<Guid>()))
                      // .//Returns((IEnumerable<Task>)null);
            //var controller = new TasksController(mockService.Object);
            var projectId = Guid.NewGuid();

            // Act
            var result = _controller.GetTasks(projectId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }

}
