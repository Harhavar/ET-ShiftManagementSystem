using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.Authmodel;
using ET_ShiftManagementSystem.Services;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShiftMgtDbContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ET_ShiftManagementSystem.Servises.TokenHandler;

namespace Et_shiftmsnsgementsystem
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<ITokenHandler> tokenHandlerMock;
        private Mock<IEmailSender> emailSenderMock;
        private Mock<ITenateServices> tenateServicesMock;
        private Mock<IorganizationServices> iorganizationServicesMock;
        private AuthController authController;

        public AuthControllerTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            tokenHandlerMock = new Mock<ITokenHandler>();
            emailSenderMock = new Mock<IEmailSender>();
            tenateServicesMock = new Mock<ITenateServices>();
            iorganizationServicesMock = new Mock<IorganizationServices>();
            authController = new AuthController(userRepositoryMock.Object, tokenHandlerMock.Object, emailSenderMock.Object, tenateServicesMock.Object, iorganizationServicesMock.Object);
        }


        [Test]
        public void Constructor_ShouldInjectDependenciesCorrectly()
        {
            // Act
            var authController = new AuthController(userRepositoryMock.Object, tokenHandlerMock.Object, emailSenderMock.Object, tenateServicesMock.Object, iorganizationServicesMock.Object);

            // Assert
            Assert.That(authController, Is.Not.Null);
            Assert.That(authController.userRepository, Is.EqualTo(userRepositoryMock.Object));
            Assert.That(authController.tokenHandler, Is.EqualTo(tokenHandlerMock.Object));
            Assert.That(authController.emailSender, Is.EqualTo(emailSenderMock.Object));
            Assert.That(authController.tenateServices, Is.EqualTo(tenateServicesMock.Object));
            Assert.That(authController.iorganizationServices, Is.EqualTo(iorganizationServicesMock.Object));
        }
        [Test]
        public void LoginAsync_ReturnsOkObjectResult_WithValidCredentials()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockTokenHandler = new Mock<ITokenHandler>();
            var mockEmailSender = new Mock<IEmailSender>();
            var mockTenateServices = new Mock<ITenateServices>();
            var mockOrganizationServices = new Mock<IorganizationServices>();
            var controller = new AuthController(
                mockUserRepository.Object,
                mockTokenHandler.Object,
                mockEmailSender.Object,
                mockTenateServices.Object,
                mockOrganizationServices.Object
            );

            var loginRequest = new ET_ShiftManagementSystem.Models.Authmodel.LoginRequest
            {
                username = "testuser",
                password = "testpassword"
            };

            var user = new User
            {
                id = Guid.NewGuid(),
                TenentID = Guid.NewGuid(),
                Role = "User"
            };

            mockUserRepository.Setup(x => x.AuthenticateAsync(loginRequest.username, loginRequest.password)).Returns(user);

            var token = "testtoken";
            mockTokenHandler.Setup(x => x.CreateToken(user)).ReturnsAsync(token);

            // Act
            var result =  controller.LoginAync(loginRequest);

            // Assert
            /*var okResult =*/ Assert.IsInstanceOf<OkObjectResult>(result);
            //var response = Assert.IsType<myResponce>(okResult.Value);
            //Assert.AreEqual(user.id, response.id);
            //Assert.AreEqual(user.TenentID, response.TenentID);
            //Assert.AreEqual(user.Role, response.Role);
            //Assert.AreEqual(token, response.Token);
        }
        [Test]
        public void LoginAsync_ReturnsOkObjectResult_WhenAuthenticationSucceeds()
        {
            // Arrange
            var loginRequest = new LoginRequest { username = "testuser", password = "testpassword" };
            var user = new User { id = Guid.NewGuid(), TenentID = Guid.NewGuid(), Role = "User" };
            var token = "testtoken";
            userRepositoryMock.Setup(x => x.AuthenticateAsync(loginRequest.username, loginRequest.password)).Returns(user);
            tokenHandlerMock.Setup(x => x.CreateToken(user)).ReturnsAsync(token);

            // Act
            var result = authController.LoginAync(loginRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<myResponce>(okResult.Value);
            var response = (myResponce)okResult.Value;
            Assert.AreEqual(user.id, response.id);
            Assert.AreEqual(user.TenentID, response.TenentID);
            Assert.AreEqual(user.Role, response.Role);
            //Assert.AreEqual(token, response.Token);
        }
        [Test]
        public void LoginAsync_ReturnsBadRequestObjectResult_WhenAuthenticationFails()
        {
            // Arrange
            var loginRequest = new LoginRequest { username = "testuser", password = "testpassword" };
            User user = null;
            userRepositoryMock.Setup(x => x.AuthenticateAsync(loginRequest.username, loginRequest.password)).Returns(user);

            // Act
            var result = authController.LoginAync(loginRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task ForgetPassword_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Arrange
            forgotPasswordRequest request = null;

            // Act
            var result = await authController.ForgetPassword(request);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task ForgetPassword_ReturnsBadRequestResult_WhenEmailNotFound()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindByEmailAsync("invalidemail@test.com"))
                              .ReturnsAsync((User)null);
            var mockTokenHandler = new Mock<ITokenHandler>();
            var mockEmailSender = new Mock<IEmailSender>();
            var controller = new AuthController(mockUserRepository.Object, mockTokenHandler.Object, mockEmailSender.Object, null, null);
            var request = new forgotPasswordRequest { Email = "invalidemail@test.com" };

            // Act
            var result = await controller.ForgetPassword(request);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task ForgetPassword_SendsEmailToUser_WhenUserFound()
        {
            // Arrange
            var user = new User { id = Guid.NewGuid(), Email = "user@example.com" };
            var request = new forgotPasswordRequest { Email = user.Email };
            var token = Guid.NewGuid().ToString();
            var saveToken = new Token { Useremail = user.Email, UserId = user.id, UserToken = token, ExpirationDate = DateTime.UtcNow.AddDays(1), TokenUsed = false };
            userRepositoryMock.Setup(r => r.FindByEmailAsync(request.Email)).ReturnsAsync(user);
            //tokenHandlerMock.Setup(h => h.SaveToken(saveToken)).Verifiable();
            emailSenderMock.Setup(s => s.SendEmailAsync(request.Email, It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask).Verifiable();

            // Act
            var result = await authController.ForgetPassword(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            tokenHandlerMock.Verify();
            emailSenderMock.Verify();
        }
        [Test]
        public async Task ForgetPassword_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var request = new forgotPasswordRequest { Email = "user@example.com" };
            userRepositoryMock.Setup(r => r.FindByEmailAsync(request.Email)).ReturnsAsync((User)null);

            // Act
            var result = await authController.ForgetPassword(request);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task ResetPassword_WithValidModel_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var model = new ResetPasswordViewModel { Password = "newPassword", ConfirmPassword = "newPassword" };

            // Act
            var result = await authController.ResetPassword(userId, model) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Password updated successfully", result.Value);

        }
        [Test]
        public async Task ResetPassword_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var model = new ResetPasswordViewModel { Password = "newPassword", ConfirmPassword = "wrongPassword" };

            // Act
            var result = await authController.ResetPassword(userId, model) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Password must be same", result.Value);
        }
    }

}
