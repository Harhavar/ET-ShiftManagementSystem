using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.UserModel;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShiftMgtDbContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Et_shiftmsnsgementsystem
{
    //[TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserRepository> userRepositoryMock;
        //private Mock<ShiftManagementDbContext> shiftManagementDbMock;
        private Mock<IEmailSender> emailSenderMock;
        private readonly UserController _controller;

        //[SetUp]
        public UserControllerTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            //shiftManagementDbMock = new Mock<ShiftManagementDbContext>();
            emailSenderMock = new Mock<IEmailSender>();
           _controller = new UserController(userRepositoryMock.Object,  emailSenderMock.Object);
        }

        [Test]
        public void UserController_Constructor_SetsDependencies()
        {
            // Act
             var controller = new UserController(userRepositoryMock.Object, emailSenderMock.Object);

            // Assert
            Assert.IsNotNull(controller);
            Assert.IsInstanceOf<UserController>(controller);
        }
        [Test]
        public async Task GetUser_Returns_OkObjectResult_With_UserData()
        {
            // Arrange
            var users = new List<User>()
        {
            new User { username = "johndoe", Role = "admin", IsActive = true, ContactNumber = "1234567890", AlternateContactNumber = "0987654321", Email = "johndoe@example.com", CreatedDate = DateTime.Now }
        };
            userRepositoryMock.Setup(x => x.GetUser()).Returns(users);

            // Act
            var result =  _controller.GetUser();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<List<GetUserRequest>>());
            var user = (List<GetUserRequest>)okResult.Value;
            Assert.That(users.Count, Is.EqualTo(user.Count));
            Assert.That(users[0].username, Is.EqualTo(user[0].username));
            Assert.That(users[0].Role, Is.EqualTo(user[0].Role));
            Assert.That(users[0].IsActive, Is.EqualTo(user[0].IsActive));
            Assert.That(users[0].ContactNumber, Is.EqualTo(user[0].ContactNumber));
            Assert.That(users[0].AlternateContactNumber, Is.EqualTo(user[0].AlternateContactNumber));
            Assert.That(users[0].Email, Is.EqualTo(user[0].Email));
            Assert.That(users[0].CreatedDate, Is.EqualTo(user[0].CreatedDate));
        }

        [Test]
        public async Task GetUser_Returns_NotFoundResult_When_UserIsNull()
        {
            // Arrange
            userRepositoryMock.Setup(x => x.GetUser()).Returns((List<User>)null);

            // Act
            var result = _controller.GetUser();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void GetUser_Throws_Exception()
        {
            // Arrange
            userRepositoryMock.Setup(x => x.GetUser()).Throws(new Exception());

            // Act + Assert
            Assert.Throws<Exception>(() => _controller.GetUser());
        }
        [Test]
        public void GetUser_Returns_OkObjectResult_With_UserCount_When_TenantId_Is_Valid()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var userCount = 5;
            userRepositoryMock.Setup(x => x.GetUserCount(tenantId)).Returns(userCount);

            // Act
            var result = _controller.GetUser(tenantId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(userCount));
        }

        [Test]
        public void GetUser_Returns_BadRequestResult_When_TenantId_Is_Empty()
        {
            // Arrange
            var tenantId = Guid.Empty;

            // Act
            var result = _controller.GetUser(tenantId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public void GetUser_by_tenent_Id_Throws_Exception()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            userRepositoryMock.Setup(x => x.GetUserCount(tenantId)).Throws(new Exception());

            // Act + Assert
            Assert.Throws<Exception>(() => _controller.GetUser(tenantId));
        }
        [Test]
        public async Task GetUsers_Returns_OkObjectResult_With_UserList_When_TenantId_Is_Valid()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var userList = new List<User> {
            new User { username = "John", Role = "Admin", IsActive = true, Email = "john@example.com", ContactNumber = "1234567890", AlternateContactNumber = "0987654321" },
            new User { username = "Jane", Role = "User", IsActive = true, Email = "jane@example.com", ContactNumber = "0987654321", AlternateContactNumber = "1234567890" }
        };
            userRepositoryMock.Setup(x => x.GetUser(tenantId)).ReturnsAsync(userList);

            // Act
            var result = await _controller.GetUsers(tenantId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.TypeOf<List<GetUserRequest>>());
            var userListResult = okResult.Value as List<GetUserRequest>;
            Assert.That(userListResult.Count, Is.EqualTo(userList.Count));
            Assert.That(userListResult[0].username, Is.EqualTo(userList[0].username));
            Assert.That(userListResult[0].Role, Is.EqualTo(userList[0].Role));
            Assert.That(userListResult[0].IsActive, Is.EqualTo(userList[0].IsActive));
            Assert.That(userListResult[0].Email, Is.EqualTo(userList[0].Email));
            Assert.That(userListResult[0].ContactNumber, Is.EqualTo(userList[0].ContactNumber));
            Assert.That(userListResult[0].AlternateContactNumber, Is.EqualTo(userList[0].AlternateContactNumber));
            Assert.That(userListResult[1].username, Is.EqualTo(userList[1].username));
            Assert.That(userListResult[1].Role, Is.EqualTo(userList[1].Role));
            Assert.That(userListResult[1].IsActive, Is.EqualTo(userList[1].IsActive));
            Assert.That(userListResult[1].Email, Is.EqualTo(userList[1].Email));
            Assert.That(userListResult[1].ContactNumber, Is.EqualTo(userList[1].ContactNumber));
            Assert.That(userListResult[1].AlternateContactNumber, Is.EqualTo(userList[1].AlternateContactNumber));
        }

        [Test]
        public async Task GetUsers_Returns_NotFoundResult_When_UserList_Is_Null()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            List<User> userList = null;
            userRepositoryMock.Setup(x => x.GetUser(tenantId)).ReturnsAsync(userList);

            // Act
            var result = await _controller.GetUsers(tenantId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void GetUsers_Throws_Exception()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            userRepositoryMock.Setup(x => x.GetUser(tenantId)).Throws(new Exception());

            // Act + Assert
            Assert.ThrowsAsync<Exception>(async () => await _controller.GetUsers(tenantId));
        }
        [Test]
        public async Task Getuser_ValidUserId_ReturnsOkResult()
        {
            // Arrange
            //var mockUserRepository = new Mock<IUserRepository>();
            //var controller = new UserController(mockUserRepository.Object);
            var userId = Guid.NewGuid();
            var user = new User() { id = userId, username = "JohnDoe", Role = "Admin", IsActive = true, Email = "john.doe@example.com", ContactNumber = "1234567890", AlternateContactNumber = "0987654321" };
            userRepositoryMock.Setup(repo => repo.Get(userId)).Returns(user);

            // Act
            var result = await _controller.Getuser(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(1, (okResult.Value as List<GetUserRequest>).Count);
            var userRequest = (okResult.Value as List<GetUserRequest>)[0];
            //Assert.AreEqual(userId, userRequest.);
            Assert.AreEqual(user.username, userRequest.username);
            Assert.AreEqual(user.Role, userRequest.Role);
            Assert.AreEqual(user.IsActive, userRequest.IsActive);
            Assert.AreEqual(user.Email, userRequest.Email);
            Assert.AreEqual(user.ContactNumber, userRequest.ContactNumber);
            Assert.AreEqual(user.AlternateContactNumber, userRequest.AlternateContactNumber);
        }
        [Test]
        public async Task Getuser_InvalidUserId_ReturnsNotFoundResult()
        {
            // Arrange
            //var mockUserRepository = new Mock<IUserRepository>();
            //var controller = new UserController(mockUserRepository.Object);
            var userId = Guid.NewGuid();
            userRepositoryMock.Setup(repo => repo.Get(userId)).Returns((User)null);

            // Act
            var result = await _controller.Getuser(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task Getuser_ExceptionThrown_ReturnsInternalServerErrorResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            userRepositoryMock.Setup(repo => repo.Get(userId)).Throws(new Exception());

            // Act
            //var result = await _controller.Getuser(userId);

            //// Assert
            //Assert.IsInstanceOf<StatusCodeResult>(result);
            //var statusCodeResult = result as StatusCodeResult;
            //Assert.AreEqual(500, statusCodeResult.StatusCode);
        }
        [Test]
        public async Task Getuser_EmptyListOfOrganizationRequests_ReturnsOkResultWithEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User() { id = userId, username = "JohnDoe", Role = "Admin", IsActive = true, Email = "john.doe@example.com", ContactNumber = "1234567890", AlternateContactNumber = "0987654321" };
            userRepositoryMock.Setup(repo => repo.Get(userId)).Returns(user);

            // Act
            var result = await _controller.Getuser(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(1, (okResult.Value as List<GetUserRequest>).Count);
        }
        [Test]
        public async Task AddUser_ValidData_ReturnsCreatedAtActionWithUserInfo()
        {
            var tenantId = Guid.NewGuid();
            var userRequest = new AddUserRequest
            {
                FirstName = "John",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                AlternateContactNumber = "0987654321"
            };

            userRepositoryMock.Setup(r => r.RegisterUserAsync(It.IsAny<ShiftMgtDbContext.Entities.User>()))
                .ReturnsAsync(new ShiftMgtDbContext.Entities.User
                {
                    id = Guid.NewGuid(),
                    FirstName = "John",
                    Email = "john@example.com",
                    ContactNumber = "1234567890",
                    AlternateContactNumber = "0987654321",
                    username = "john",
                    TenentID = tenantId
                });

            // Act
            var result = await _controller.AddUser(tenantId, userRequest);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            
            userRepositoryMock.Verify(r => r.RegisterUserAsync(It.IsAny<ShiftMgtDbContext.Entities.User>()), Times.Once);
            emailSenderMock.Verify(es => es.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        [Test]
        public async Task AddUser_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            
            var tenantId = Guid.NewGuid();
            var userRequest = new AddUserRequest
            {
                FirstName = "",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                AlternateContactNumber = "0987654321"
            };

            // Act
            var result = await _controller.AddUser(tenantId, userRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task AddUser_ExceptionThrown_RethrowsException()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var emailSenderMock = new Mock<IEmailSender>();
            var controller = new UserController(userRepositoryMock.Object, emailSenderMock.Object);

            var tenantId = Guid.NewGuid();
            var userRequest = new AddUserRequest
            {
                FirstName = "John",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890",
                AlternateContactNumber = "0987654321"
            };

            userRepositoryMock.Setup(repo => repo.RegisterUserAsync(It.IsAny<User>()))
                .ThrowsAsync(new Exception("An error occurred while registering the user."));

            // Act + Assert
            var ex = Assert.ThrowsAsync<Exception>(() => controller.AddUser(tenantId, userRequest));
            Assert.That(ex.Message, Is.EqualTo("An error occurred while registering the user."));
        }
        [Test]
        public async Task UpdateUser_ValidInput_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateUserRequest = new UpdateUserRequest()
            {
                UserName = "John",
                Email = "john@test.com",
                ContactNumber = "1234567890",
                AlternateContactNumber = "0987654321",
                //IsActive = true
            };
            //var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.EditUser(userId, It.IsAny<User>()))
                              .ReturnsAsync(new User() );
            //var controller = new UserController(userRepositoryMock.Object);

            // Act
            var result = await _controller.UpdateUser(userId, updateUserRequest);

            // Assert
            //userRepositoryMock.Verify(x => x.EditUser(userId, It.IsAny<User>()), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
           // var updatedUser = (UserDto)((OkObjectResult)result).Value;
           //Assert.AreEqual(userId, updatedUser.id);
           // Assert.AreEqual(updateUserRequest.FirstName, updatedUser.FirstName);
           // Assert.AreEqual(updateUserRequest.Email, updatedUser.Email);
           // Assert.AreEqual(updateUserRequest.ContactNumber, updatedUser.ContactNumber);
           // Assert.AreEqual(updateUserRequest.AlternateContactNumber, updatedUser.AlternateContactNumber);
           // Assert.AreEqual(updateUserRequest.IsActive, updatedUser.IsActive);
        }
        [Test]
        public async Task UpdateUser_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateUserRequest = new UpdateUserRequest()
            {
                UserName = "John",
                Email = "john@test.com",
                ContactNumber = "1234567890",
                AlternateContactNumber = "0987654321",
                //IsActive = true
            };
            //var userRepositoryMock = new Mock<IUserRepository>();
            //userRepositoryMock.Setup(x => x.EditUser(userId, It.IsAny<User>()))
                              //.Returns(null);
           // var controller = new _controller(userRepositoryMock.Object);

            // Act
            var result = await 
                _controller.UpdateUser(userId, updateUserRequest);

            // Assert
            userRepositoryMock.Verify(x => x.EditUser(userId, It.IsAny<User>()), Times.Once);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task UpdateUser_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.Empty;
            var updateUserRequest = new UpdateUserRequest()
            {
                UserName = "",
                Email = "",
                ContactNumber = "",
                AlternateContactNumber = "",
                //IsActive = true
            };
            var userRepositoryMock = new Mock<IUserRepository>();
            //var controller = new UserController(userRepositoryMock.Object);

            // Act
            var result = await _controller.UpdateUser(userId, updateUserRequest);

            // Assert
            userRepositoryMock.Verify(x => x.EditUser(userId, It.IsAny<User>()), Times.Never);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public void GetAssignedProject_UserIdIsEmpty_ReturnsBadRequest()
        {
            // Arrange
            Guid userId = Guid.Empty;

            // Act
            var result = _controller.GetAssignedProject(userId);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void GetAssignedProject_NoAssignedProjects_ReturnsOkWithCountZero()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            // userRepository.AssignedProject returns an empty list
            userRepositoryMock.Setup(repo => repo.AssignedProject(It.IsAny<Guid>())).Returns(new List<UserShift>());

            // Act
            var result = _controller.GetAssignedProject(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(0, okResult.Value);
        }

        [Test]
        public void GetAssignedProject_AssignedProjectsExist_ReturnsOkWithProjectCount()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            // userRepository.AssignedProject returns a list with 2 projects
            var projects = new List<UserShift>
        {
            new UserShift { ID = Guid.NewGuid() },
            new UserShift { ID = Guid.NewGuid() }
        };
            userRepositoryMock.Setup(repo => repo.AssignedProject(It.IsAny<Guid>())).Returns(projects);

            // Act
            var result = _controller.GetAssignedProject(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(projects.Count, okResult.Value);
        }


    }

}
