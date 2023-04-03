using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.OrganizationRoleModel;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Et_shiftmsnsgementsystem
{
    public class OrganizationRoleTest
    {
        private Mock<IOrganizationRoleServices> _organizationServicesMock;
        private readonly OrganizationRoleController _controller;
        public OrganizationRoleTest()
        {
            _organizationServicesMock = new Mock<IOrganizationRoleServices>();
            _controller = new OrganizationRoleController(_organizationServicesMock.Object);
        }
        [Test]
        public void Constructor_WithValidArguments_ShouldCreateInstance()
        {
            var controller = new OrganizationRoleController(_organizationServicesMock.Object);

            Assert.IsNotNull(controller);
        }
        [Test]
        public async Task GetOrganizationRole_ReturnsOkResult()
        {
            // Arrange
            // var mockRoleService = new Mock<IRoleService>();
            var roleList = new List<OrganizationRole>
    {
        new OrganizationRole { RoleName = "Role1", Description = "Description1", RoleType = "Type1", CreatedDate = DateTime.Now },
        new OrganizationRole { RoleName = "Role2", Description = "Description2", RoleType = "Type2", CreatedDate = DateTime.Now }
    };
            _organizationServicesMock.Setup(x => x.GetRoles()).Returns(roleList);

            //var controller = new OrganizationRoleController(_organizationServicesMock.Object);

            // Act
            var result = _controller.GetOrganizationRole() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var organizationRoles = result.Value as List<OrganizationRoleViewRequest>;
            Assert.IsNotNull(organizationRoles);
            Assert.AreEqual(2, organizationRoles.Count);
        }
        [Test]
        public void GetOrganizationRole_Returns_Ok_With_Roles()
        {
            // Arrange
            ///var mockRoleServices = new Mock<IRoleServices>();
            var roles = new List<OrganizationRole>()
    {
        new OrganizationRole()
        {
            RoleName = "Admin",
            Description = "Admin role",
            RoleType = "System",
            CreatedDate = DateTime.Now,
            LinkedPermission = "Full access"
        },
        new OrganizationRole()
        {
            RoleName = "Manager",
            Description = "Manager role",
            RoleType = "Custom",
            CreatedDate = DateTime.Now,
            LinkedPermission = "Read-only access"
        }
    };
            _organizationServicesMock.Setup(x => x.GetRoles()).Returns(roles);

            var controller = new OrganizationRoleController(_organizationServicesMock.Object);

            // Act
            var result = controller.GetOrganizationRole();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<List<OrganizationRoleViewRequest>>(okResult.Value);

            var organizationRoles = okResult.Value as List<OrganizationRoleViewRequest>;
            Assert.AreEqual(2, organizationRoles.Count);

            Assert.AreEqual("Admin", organizationRoles[0].RoleName);
            Assert.AreEqual("Admin role", organizationRoles[0].Description);
            Assert.AreEqual("System", organizationRoles[0].RoleType);
            Assert.AreEqual("Full access", organizationRoles[0].LinkedPermission);

            Assert.AreEqual("Manager", organizationRoles[1].RoleName);
            Assert.AreEqual("Manager role", organizationRoles[1].Description);
            Assert.AreEqual("Custom", organizationRoles[1].RoleType);
            Assert.AreEqual("Read-only access", organizationRoles[1].LinkedPermission);
        }
        [Test]
        public async Task GetOrganizationRole_ReturnsBadRequest()
        {
            // Arrange
            _organizationServicesMock.Setup(x => x.GetRoles()).Throws(new Exception("Test exception"));
          //  var controller = new OrganizationController(_organizationServicesMock.Object, _roleServicesMock.Object);

            // Act
            var result =  _controller.GetOrganizationRole();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task GetOrganizationRole_ReturnsBadRequest_WhenRoleServicesThrowsException()
        {
            // Arrange
            _organizationServicesMock.Setup(x => x.GetRoles()).Throws(new Exception("Failed to retrieve roles"));

            // Act
            var result = _controller.GetOrganizationRole();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Failed to retrieve roles", badRequestResult.Value);
        }
        [Test]
        public async Task GetGlobalRole_ReturnsOkWithGlobalRoleViewRequestList()
        {
            // Arrange
            var expectedGlobalRoles = new List<GlobalRole>
    {
        new GlobalRole
        {
            RoleName = "Admin",
            Description = "Admin role",
            CreatedDate = DateTime.UtcNow,
            LinkedPermission = "Can manage everything"
        },
        new GlobalRole
        {
            RoleName = "Guest",
            Description = "Guest role",
            CreatedDate = DateTime.UtcNow,
            LinkedPermission = "Can view only"
        }
    };
            //var mockRoleService = new Mock<IRoleService>();
            _organizationServicesMock.Setup(svc => svc.GetGlobalRoles()).Returns(expectedGlobalRoles);
           // var controller = new YourController(mockRoleService.Object);

            // Act
            var result =  _controller.GetGlobalRole();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<List<GlobalRoleViewRequest>>(okResult.Value);
            var actualGlobalRoles = (List<GlobalRoleViewRequest>)okResult.Value;
            Assert.AreEqual(expectedGlobalRoles.Count, actualGlobalRoles.Count);
            for (int i = 0; i < expectedGlobalRoles.Count; i++)
            {
                Assert.AreEqual(expectedGlobalRoles[i].RoleName, actualGlobalRoles[i].RoleName);
                Assert.AreEqual(expectedGlobalRoles[i].Description, actualGlobalRoles[i].Description);
                Assert.AreEqual(expectedGlobalRoles[i].CreatedDate, actualGlobalRoles[i].CreatedDate);
                Assert.AreEqual(expectedGlobalRoles[i].LinkedPermission, actualGlobalRoles[i].LinkedPermission);
            }
        }
        [Test]
        public async Task GetGlobalRoles_ReturnsOkResultWithGlobalRoles()
        {
            // Arrange
           // var mockRoleService = new Mock<IRoleService>();
            var globalRoles = new List<GlobalRole>
    {
        new GlobalRole { RoleName = "Role1", Description = "Description1", CreatedDate = DateTime.Now, LinkedPermission = "Permission1" },
        new GlobalRole { RoleName = "Role2", Description = "Description2", CreatedDate = DateTime.Now, LinkedPermission = "Permission2" },
        new GlobalRole { RoleName = "Role3", Description = "Description3", CreatedDate = DateTime.Now, LinkedPermission = "Permission3" }
    };
            _organizationServicesMock.Setup(service => service.GetGlobalRoles()).Returns(globalRoles);
            var controller = new OrganizationRoleController(_organizationServicesMock.Object);

            // Act
            var result = controller.GetGlobalRole();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<List<GlobalRoleViewRequest>>(okResult.Value);
            var globalRoleViewRequests = okResult.Value as List<GlobalRoleViewRequest>;
            Assert.AreEqual(globalRoles.Count, globalRoleViewRequests.Count);
            for (int i = 0; i < globalRoles.Count; i++)
            {
                Assert.AreEqual(globalRoles[i].RoleName, globalRoleViewRequests[i].RoleName);
                Assert.AreEqual(globalRoles[i].Description, globalRoleViewRequests[i].Description);
                Assert.AreEqual(globalRoles[i].CreatedDate, globalRoleViewRequests[i].CreatedDate);
                Assert.AreEqual(globalRoles[i].LinkedPermission, globalRoleViewRequests[i].LinkedPermission);
            }
        }
        [Test]
        public void GetGlobalRole_ReturnsBadRequest()
        {
            // Arrange
            _organizationServicesMock.Setup(x => x.GetGlobalRoles()).Throws(new Exception("Error"));

            // Act
            var result = _controller.GetGlobalRole() as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Error", result.Value);
        }
        [Test]
        public async Task GetGlobalRole_ReturnsNotFound_WhenNoRolesExist()
        {
            // Arrange
            //var mockRoleService = new Mock<IRoleService>();
            _organizationServicesMock.Setup(service => service.GetGlobalRoles()).Returns(() => null);
            var controller = new OrganizationRoleController(_organizationServicesMock.Object);

            // Act
            var result = controller.GetGlobalRole();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task GetOrganizationViewRole_ReturnsOk_WhenRoleExists()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var role = new OrganizationRole
            {
                RoleName = "Role 1",
                Description = "Role 1 Description",
                CreatedDate = DateTime.UtcNow,
                LinkedPermission =  "Permission 1",
                LastModifiedDate = DateTime.UtcNow
            };
            _organizationServicesMock.Setup(x => x.GetRoles(guid)).Returns(role);
            //var controller = new YourController(_organizationServicesMock.Object);

            // Act
            var result =  _controller.GetOrganizationViewRole(guid);

            // Assert
           /* var okResult =*/ Assert.IsInstanceOf<OkObjectResult>(result);
            //var organizationDTO = Assert.IsAssignableFrom<OrganizationCustomRoleViewRequest>(okResult);
            //Assert.AreEqual(role.RoleName, organizationDTO.RoleName);
            //Assert.AreEqual(role.Description, organizationDTO.Description);
            //Assert.AreEqual(role.CreatedDate, organizationDTO.CreatedDate);
            //Assert.AreEqual(role.LinkedPermission, organizationDTO.LinkedPermission);
            //Assert.AreEqual(role.LastModifiedDate, organizationDTO.LastModifiedDate);
        }

        [Test]
        public async Task GetOrganizationViewRole_ReturnsNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            var guid = Guid.NewGuid();
            _organizationServicesMock.Setup(x => x.GetRoles(guid)).Returns(null as OrganizationRole);
           // var controller = new YourController(_roleServicesMock.Object);

            // Act
            var result = _controller.GetOrganizationViewRole(guid);

            // Assert
             Assert.IsInstanceOf<NotFoundResult>(result);
        }


    }
}
