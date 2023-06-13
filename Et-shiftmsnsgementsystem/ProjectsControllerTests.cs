using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.ProjectsModel;
using ET_ShiftManagementSystem.Services;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET_ShiftManagementSystem.Models.ShiftModel;
using Microsoft.AspNetCore.Http;
using ShiftMgtDbContext.Entities;

namespace Et_shiftmsnsgementsystem
{
    //[TestFixture]
    public class ProjectsControllerTests
    {
        private Mock<IProjectServices> _projectServicesMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IShiftServices> _shiftServicesMock;
        private readonly ProjectsController _controller;

        //[SetUp]
        public ProjectsControllerTests()
        {
            _projectServicesMock = new Mock<IProjectServices>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _shiftServicesMock = new Mock<IShiftServices>();
            _controller = new ProjectsController(_projectServicesMock.Object, _userRepositoryMock.Object, _shiftServicesMock.Object);
        }

        [Test]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange

            // Act
            var controller = new ProjectsController(_projectServicesMock.Object, _userRepositoryMock.Object, _shiftServicesMock.Object);

            // Assert
            Assert.IsNotNull(controller);
        }

        [Test]
        public void Constructor_ShouldSetProjectServices()
        {
            // Arrange

            // Act
            var controller = new ProjectsController(_projectServicesMock.Object, _userRepositoryMock.Object, _shiftServicesMock.Object);

            // Assert
            // Assert.AreSame(_projectServicesMock.Object, _controller.);
        }

        [Test]
        public void Constructor_ShouldSetUserRepository()
        {
            // Arrange

            // Act
            var controller = new ProjectsController(_projectServicesMock.Object, _userRepositoryMock.Object, _shiftServicesMock.Object);

            // Assert
            // Assert.AreSame(_userRepositoryMock.Object, controller._userRepository);
        }

        [Test]
        public void Constructor_ShouldSetShiftServices()
        {
            // Arrange

            // Act
            var controller = new ProjectsController(_projectServicesMock.Object, _userRepositoryMock.Object, _shiftServicesMock.Object);

            // Assert
            //Assert.AreSame(_shiftServicesMock.Object, controller._shiftServices);
        }
        [Test]
        public void GetProjectsData_ReturnsOkResult_WithListOfProjectsViewRequests()
        {
            // Arrange
            var projects = new List<Projects>()
        {
            new Projects() { Name = "Project 1", Description = "Description 1"},
            new Projects() { Name = "Project 2", Description = "Description 2" } };
            _projectServicesMock.Setup(x => x.GetProjectsData()).Returns(projects);

            // Act
            var result = _controller.GetProjectsData() as OkObjectResult;
            var projectsViewRequests = result.Value as List<ProjectsViewRequest>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(projectsViewRequests);
            Assert.AreEqual(2, projectsViewRequests.Count);
            Assert.AreEqual("Project 1", projectsViewRequests[0].Name);
            Assert.AreEqual("Description 1", projectsViewRequests[0].Description);
            //Assert.AreEqual("Active", projectsViewRequests[0].Status);
            Assert.AreEqual("Project 2", projectsViewRequests[1].Name);
            Assert.AreEqual("Description 2", projectsViewRequests[1].Description);
            //Assert.AreEqual("Inactive", projectsViewRequests[1].Status);
        }
        [Test]
        public void GetProjectsData_ReturnsNotFoundResult_WhenProjectsDataIsNull()
        {
            // Arrange
            _projectServicesMock.Setup(x => x.GetProjectsData()).Returns((List<Projects>)null);

            // Act
            var result = _controller.GetProjectsData() as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public void GetProjectsData_ReturnsOkResult_WithExceptionMessage_WhenExceptionIsThrown()
        {
            // Arrange
            _projectServicesMock.Setup(x => x.GetProjectsData()).Throws(new Exception("Test Exception"));

            // Act
            //var result = _controller.GetProjectsData() as OkObjectResult;
           // var message = result.Value as string;

            // Assert
            //Assert.IsNotNull(result);
            //Assert.AreEqual(200, result.StatusCode);
            //Assert.IsNotNull(message);
            //Assert.AreEqual("Test Exception", message);
        }

        [Test]
        public void GetProjectsData_ReturnsBadRequestResult_WhenExceptionIsThrownAndResultIsNull()
        {
            // Arrange
            _projectServicesMock.Setup(x => x.GetProjectsData()).Throws(new Exception());

            // Act
            var result = _controller.GetProjectsData() as BadRequestResult;

            // Assert
            // Assert.IsNotNull(result);
            //Assert.AreEqual(400, result.StatusCode);
        }
        [Test]
        public void GetDetails_ReturnsOk_WhenProjectsExist()
        {
            // Arrange
            var projects = new List<Projects>
        {
            new Projects { ProjectId = Guid.NewGuid(), Name = "Project 1", Description = "Description 1", Status = "Active", CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now },
            new Projects { ProjectId = Guid.NewGuid(), Name = "Project 2", Description = "Description 2", Status = "Inactive", CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now },
        };
            _projectServicesMock.Setup(s => s.GetProjectsData()).Returns(projects);

            // Act
            var result = _controller.GetDetails();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<List<ProjectDetailsRequest>>(okResult.Value);
            var projectDetailsRequests = okResult.Value as List<ProjectDetailsRequest>;
            Assert.AreEqual(projects.Count, projectDetailsRequests.Count);
        }

        [Test]
        public void GetDetails_ReturnsNotFound_WhenNoProjectsExist()
        {
            // Arrange
            _projectServicesMock.Setup(s => s.GetProjectsData()).Returns((List<Projects>)null);

            // Act
            var result = _controller.GetDetails();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetDetails_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _projectServicesMock.Setup(s => s.GetProjectsData()).Throws(new Exception());

            // Act
            var result = _controller.GetDetails();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Exception of type 'System.Exception' was thrown.", okResult.Value);
        }

        [Test]
        public void GetDetails_ReturnsBadRequest_WhenNullIsReturned()
        {
            // Arrange
            _projectServicesMock.Setup(s => s.GetProjectsData()).Returns((List<Projects>)null);

            // Act
            var result = _controller.GetDetails();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void GetTenentProject_WithEmptyTenentId_ReturnsBadRequest()
        {
            // Arrange
            var tenentId = Guid.Empty;

            // Act
            var result = _controller.GetTenentProject(tenentId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void GetTenentProject_WithExistingTenentId_ReturnsOk()
        {
            // Arrange
            var tenentId = Guid.NewGuid();
            _projectServicesMock.Setup(x => x.GetProjectsData(tenentId)).Returns(new List<Projects>());

            // Act
            var result = _controller.GetTenentProject(tenentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetTenentProject_WithNonExistingTenentId_ReturnsNotFound()
        {
            // Arrange
            var tenentId = Guid.NewGuid();
            _projectServicesMock.Setup(x => x.GetProjectsData(tenentId)).Returns((List<Projects>)null);

            // Act
            var result = _controller.GetTenentProject(tenentId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void GetCount_ReturnsOkResult_WithProjectCount()
        {
            // Arrange
            var projectList = new List<Projects> { new Projects(), new Projects() };
            _projectServicesMock.Setup(x => x.GetProjectsData()).Returns(projectList);

            // Act
            var result = _controller.GetCount();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(projectList.Count, okResult.Value);
        }

        [Test]
        public void GetCount_ReturnsOkResult_WithZeroProjectCount()
        {
            // Arrange
            var projectList = new List<Projects>();
            _projectServicesMock.Setup(x => x.GetProjectsData()).Returns(projectList);

            // Act
            var result = _controller.GetCount();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(projectList.Count, okResult.Value);
        }

        [Test]
        public void GetCount_ReturnsBadRequestResult_WhenServiceThrowsException()
        {
            // Arrange
            _projectServicesMock.Setup(x => x.GetProjectsData()).Throws(new Exception());

            // Act
            var result = _controller.GetCount();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetCount_ReturnsOkResult_WithErrorMessage_WhenServiceThrowsException()
        {
            // Arrange
            var errorMessage = "Test Exception";
            _projectServicesMock.Setup(x => x.GetProjectsData()).Throws(new Exception(errorMessage));

            // Act
            var result = _controller.GetCount();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(errorMessage, okResult.Value);
        }
        [Test]
        public void GetCountOrganization_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var projects = new List<Projects> { new Projects(), new Projects(), new Projects() };
            _projectServicesMock.Setup(x => x.GetProjectsCount(tenantId)).Returns(3);
            // Act
            var result = _controller.GetCountOrganization(tenantId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(3, okResult.Value);
        }

        [Test]
        public void GetCountOrganization_WithEmptyId_ReturnsBadRequestResult()
        {
            // Arrange
            var tenantId = Guid.Empty;

            // Act
            var result = _controller.GetCountOrganization(tenantId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void GetCountOrganization_WithNonExistingProjects_ReturnsOkResultWithZeroCount()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            _projectServicesMock.Setup(x => x.GetProjectsCount(tenantId)).Returns(0);
            // Act
            var result = _controller.GetCountOrganization(tenantId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(0, okResult.Value);
        }

        [Test]
        public void GetCountOrganization_WithException_ReturnsOkResultWithErrorMessage()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            _projectServicesMock.Setup(x => x.GetProjectsCount(tenantId)).Returns(0);

            // Act
            var result = _controller.GetCountOrganization(tenantId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(0, okResult.Value);
        }
        [Test]
        public async Task AddProject_ReturnsCreated()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var addProjectRequest = new AddProjectRequest
            {
                Name = "Project 1",
                Description = "Project 1 description",
                UserShift = new List<UserShiftModel>
        {
            new UserShiftModel
            {
                UserId = Guid.NewGuid(),
                ShiftId = Guid.NewGuid()
            },
            new UserShiftModel
            {
                UserId = Guid.NewGuid(),
                ShiftId = Guid.NewGuid()
            }
        }
            };
            _projectServicesMock.Setup(x => x.AddProject(tenantId, It.IsAny<Projects>())).ReturnsAsync(new Projects { ProjectId = Guid.NewGuid() });
            _shiftServicesMock.Setup(x => x.Update(It.IsAny<List<UserShift>>()));


            // Act
            var result = await _controller.AddProject(tenantId, addProjectRequest);

            // Assert
            Assert.IsInstanceOf(typeof(CreatedAtActionResult), result);
        }

        [Test]
        public async Task AddProject_ReturnsBadRequest_WhenTenantIdIsEmpty()
        {
            // Arrange
            var tenantId = Guid.Empty;
            var addProjectRequest = new AddProjectRequest
            {
                Name = "Project 1",
                Description = "Project 1 description",
                UserShift = new List<UserShiftModel>
        {
            new UserShiftModel
            {
                UserId = Guid.NewGuid(),
                ShiftId = Guid.NewGuid()
            }
        }
            };

            // Act
            var result = await _controller.AddProject(tenantId, addProjectRequest);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }

        [Test]
        public async Task AddProject_ReturnsBadRequest_WhenAddProjectRequestIsNull()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            AddProjectRequest addProjectRequest = null;

            // Act
            var result = await _controller.AddProject(tenantId, addProjectRequest);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }
        [Test]
        public async Task UpdateProject_ValidProjectIdAndRequest_ReturnsOkResult()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var addProject = new EditProjectRequest
            {
                Name = "Test Project",
                Description = "Test Description",
                UserShift = new List<UserShiftModel>
        {
            new UserShiftModel
            {
                ShiftId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            }
        }
            };

            // Act
            var result = _controller.UpdateProject(projectId, addProject) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            //Assert.IsNotNull(result.Value);
            //var project = result.Value as Projects;
            //Assert.AreEqual(addProject.Name, project.Name);
            //Assert.AreEqual(addProject.Description, project.Description);
        }
        [Test]
        public async Task UpdateProject_EmptyProjectId_ReturnsBadRequestResult()
        {
            // Arrange
            var projectId = Guid.Empty;
            var addProject = new EditProjectRequest
            {
                Name = "Test Project",
                Description = "Test Description",
                UserShift = new List<UserShiftModel>
        {
            new UserShiftModel
            {
                ShiftId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            }
        }
            };

            // Act
            var result =  _controller.UpdateProject(projectId, addProject) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Test]
        public async Task UpdateProject_NullAddProject_ReturnsBadRequestResult()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            EditProjectRequest addProject = null;

            // Act
            var result =  _controller.UpdateProject(projectId, addProject) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }
        [Test]
        public async Task UpdateProject_ExceptionThrown_ReturnsOkResultWithErrorMessage()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var addProject = new EditProjectRequest
            {
                Name = "Test Project",
                Description = "Test Description",
                UserShift = new List<UserShiftModel>
        {
            new UserShiftModel
            {
                ShiftId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            }
        }
            };
            _projectServicesMock.Setup(s => s.EditProject(It.IsAny<Guid>(), It.IsAny<EditProjectRequest>())).ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result =  _controller.UpdateProject(projectId, addProject) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsNotNull(result.Value);
            var errorMessage = result.Value as string;
            Assert.AreEqual("Test Exception", errorMessage);
        }

        [Test]
        public async Task DeleteProject_ValidProjectId_ReturnsOk()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _projectServicesMock.Setup(x => x.DeleteProject(projectId)).ReturnsAsync(new Projects());

            // Act
            var result = await _controller.DeleteProject(projectId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteProject_InvalidProjectId_ReturnsBadRequest()
        {
            // Arrange
            var projectId = Guid.Empty;

            // Act
            var result = await _controller.DeleteProject(projectId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task DeleteProject_NonExistingProjectId_ReturnsNotFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _projectServicesMock.Setup(x => x.DeleteProject(projectId)).ReturnsAsync((Projects)null);

            // Act
            var result = await _controller.DeleteProject(projectId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task GetShiftUser_WithValidProjectId_ReturnsOkResult()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var users = new List<UserShift>
        {
            new UserShift { UserId = Guid.NewGuid() },
            new UserShift { UserId = Guid.NewGuid()  }
        };
            var shifts = new List<ET_ShiftManagementSystem.Entities.AddShiftRequest> { new ET_ShiftManagementSystem.Entities.AddShiftRequest { ShiftName = "shift1" } };
            //_shiftServicesMock.Setup(x => x.userShifts(projectId)).Returns(new <Task<List<User>>());
            _shiftServicesMock.Setup(x => x.UserShiftName(projectId)).ReturnsAsync(shifts);

            // Act
            var result = await _controller.GetShiftUser(projectId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetShiftUser_WithInvalidProjectId_ReturnsBadRequest()
        {
            // Arrange
            var projectId = Guid.Empty;

            // Act
            var result = await _controller.GetShiftUser(projectId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetShiftUser_WithExceptionThrown_ReturnsOkResultWithErrorMessage()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var errorMessage = "An error occurred";
            _shiftServicesMock.Setup(x => x.userShifts(projectId)).ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetShiftUser(projectId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(errorMessage, okResult.Value);
        }
        [Test]
        public async Task GetUserDetails_ValidShiftId_ReturnsOkResult()
        {
            // Arrange
           // var mockShiftService = new Mock<IShiftService>();
           // var controller = new ShiftController(mockShiftService.Object);
            var shiftId = Guid.NewGuid();

            // Act
            var result = await _controller.GetUserDetails(shiftId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task GetUserDetails_InvalidShiftId_ReturnsBadRequestResult()
        {
            // Arrange
            //var mockShiftService = new Mock<IShiftService>();
            //var controller = new ShiftController(mockShiftService.Object);
            var shiftId = Guid.Empty;

            // Act
            var result = await _controller.GetUserDetails(shiftId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task GetUserDetails_ExceptionThrown_ReturnsOkResultWithErrorMessage()
        {
            // Arrange
            //var mockShiftService = new Mock<IShiftService>();
            _shiftServicesMock.Setup(x => x.userShiftsDetails(It.IsAny<Guid>())).ThrowsAsync(new Exception("An error occurred."));
           // var controller = new ShiftController(mockShiftService.Object);
            var shiftId = Guid.NewGuid();

            // Act
            var result = await _controller.GetUserDetails(shiftId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<string>(okResult.Value);
            Assert.AreEqual("An error occurred.", okResult.Value);
        }

    }

}
