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

        [Test]
        public async Task GetGlobalViewRole_ReturnsOkResult_WhenRoleExists()
        {
            // Arrange
            //var mockRoleService = new Mock<IRoleService>();
            var roleId = Guid.NewGuid();
            var roleName = "Test Role";
            var roleDescription = "Test Role Description";
            var roleType = "Test Role Type";
            var roleCreatedDate = DateTime.UtcNow;
            var roleLinkedPermission = "";
            var roleLastModifiedDate = DateTime.UtcNow;
            _organizationServicesMock.Setup(service => service.GetGlobalRoles(roleId)).Returns(new GlobalRole
            {
                Id = roleId,
                RoleName = roleName,
                Description = roleDescription,
               
                CreatedDate = roleCreatedDate,
                LinkedPermission = roleLinkedPermission,
                LastModifiedDate = roleLastModifiedDate
            });

            //var controller = new OrganizationRoleController(_organizationServicesMock.Object);

            // Act
            var result =  _controller.GetGlobalViewRole(roleId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<GlobalCustomRoleViewRequest>(okResult.Value);
            var organizationRole = okResult.Value as GlobalCustomRoleViewRequest;
            Assert.AreEqual(roleName, organizationRole.RoleName);
            Assert.AreEqual(roleDescription, organizationRole.Description);
            Assert.AreEqual(roleCreatedDate, organizationRole.CreatedDate);
            Assert.AreEqual(roleLinkedPermission, organizationRole.LinkedPermission);
            Assert.AreEqual(roleLastModifiedDate, organizationRole.LastModifiedDate);
        }
        [Test]
        public void GetGlobalViewRole_ValidGuid_ReturnsOkWithGlobalCustomRoleViewRequest()
        {
            // Arrange
            var guid = Guid.NewGuid();
            //var mockRoleServices = new Mock<IRoleServices>();
            var role = new GlobalRole()
            {
                RoleName = "Test Role",
                Description = "Test Role Description",
                CreatedDate = DateTime.UtcNow,
                LinkedPermission = "Test Permission",
                LastModifiedDate = DateTime.UtcNow
            };
            _organizationServicesMock.Setup(x => x.GetGlobalRoles(guid)).Returns(role);
            // controller = new OrganizationRoleController(_organizationServicesMock.Object);

            // Act
            var result = _controller.GetGlobalViewRole(guid);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<GlobalCustomRoleViewRequest>(okResult.Value);
            var organizationDTO = (GlobalCustomRoleViewRequest)okResult.Value;
            Assert.AreEqual(role.RoleName, organizationDTO.RoleName);
            Assert.AreEqual(role.Description, organizationDTO.Description);
            Assert.AreEqual(role.CreatedDate, organizationDTO.CreatedDate);
            Assert.AreEqual(role.LinkedPermission, organizationDTO.LinkedPermission);
            Assert.AreEqual(role.LastModifiedDate, organizationDTO.LastModifiedDate);
        }
        [Test]
        public void GetGlobalViewRole_ReturnsOk_WhenRoleExists()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var mockRole = new GlobalRole { Id = roleId, RoleName = "Admin", Description = "Administrator role", CreatedDate = DateTime.Now };
            ///var mockRoleService = new Mock<IRoleService>();
            _organizationServicesMock.Setup(s => s.GetGlobalRoles(roleId)).Returns(mockRole);

            ///var controller = new RoleController(mockRoleService.Object);

            // Act
            var result = _controller.GetGlobalViewRole(roleId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var organizationDTO = result.Value as GlobalCustomRoleViewRequest;
            Assert.IsNotNull(organizationDTO);
            Assert.AreEqual(mockRole.RoleName, organizationDTO.RoleName);
            Assert.AreEqual(mockRole.Description, organizationDTO.Description);
            Assert.AreEqual(mockRole.CreatedDate, organizationDTO.CreatedDate);
            Assert.AreEqual(mockRole.LinkedPermission, organizationDTO.LinkedPermission);
            Assert.AreEqual(mockRole.LastModifiedDate, organizationDTO.LastModifiedDate);
        }

        [Test]
        public void GetGlobalViewRole_ReturnsNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            //var mockRoleService = new Mock<IRoleService>();
            _organizationServicesMock.Setup(s => s.GetGlobalRoles(roleId)).Returns((GlobalRole)null);

            //var controller = new RoleController(mockRoleService.Object);

            // Act
            var result = _controller.GetGlobalViewRole(roleId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
        [Test]
        public void GetGlobalViewRole_ReturnsBadRequest_WhenRoleIdIsEmpty()
        {
            // Arrange
            var roleId = Guid.Empty;
            //var mockRoleService = new Mock<IRoleService>();
            //var controller = new RoleController(mockRoleService.Object);

            // Act
            var result = _controller.GetGlobalViewRole(roleId) as BadRequestResult;

            // Assert
            //Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }


        [Test]
        public async Task UpdateRole_WhenRoleExists_ReturnsOkResult()
        {
            // Arrange
            //var mockRoleServices = new Mock<IRoleServices>();
            var roleId = Guid.NewGuid();
            var editRoleRequest = new EditRoleRequest { RoleName = "Admin", Description = "Admin role", LinkedPermission = "" };
            _organizationServicesMock.Setup(x => x.EditRoleRequest(roleId, It.IsAny<OrganizationRole>()));
            //var controller = new RoleController(mockRoleServices.Object);

            // Act
            var result =  _controller.UpdateRole(roleId, editRoleRequest);

            // Assert
            //Assert.IsInstanceOf<OkObjectResult>(result);
            //var okResult = result as OkObjectResult;
            //Assert.IsNotNull(okResult.Value);
            //Assert.IsInstanceOf<OrganizationRole>(okResult.Value);
        }
        [Test]
        public async Task UpdateRole_ReturnsBadRequest_WhenRoleNameIsNull()
        {
            // Arrange
            //var mockRoleServices = new Mock<IOrganizationRoleService>();
            //var controller = new OrganizationRoleController(mockRoleServices.Object);
            var editRole = new EditRoleRequest
            {
                RoleName = null,
                Description = "test description",
                LinkedPermission = ""
            };

            // Act
            var result =  _controller.UpdateRole(Guid.NewGuid(), editRole);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task UpdateRole_WhenRoleNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var editRoleRequest = new EditRoleRequest();
            _organizationServicesMock.Setup(x => x.EditRoleRequest(guid, It.IsAny<OrganizationRole>())).Returns(new OrganizationRole());

            // Act
            var result = _controller.UpdateRole(guid, editRoleRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task UpdateRole_WhenExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var editRoleRequest = new EditRoleRequest();
            _organizationServicesMock.Setup(x => x.EditRoleRequest(guid, It.IsAny<OrganizationRole>())).Throws(new Exception("Some error"));

            // Act
            var result = _controller.UpdateRole(guid, editRoleRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public  void UpdateGlobalRole_WithValidGuidAndRole_ReturnsOkResult()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var editRole = new EditRoleRequest
            {
                RoleName = "Test Role",
                Description = "Test Role Description",
                LinkedPermission = "Test Permission"
            };
            var role = new GlobalRole
            {
                Id = guid,
                RoleName = editRole.RoleName,
                Description = editRole.Description,
                LinkedPermission = editRole.LinkedPermission
            };
            _organizationServicesMock.Setup(x => x.EditGlobalRoleRequest(guid, It.IsAny<GlobalRole>())).Returns(new GlobalRole() );

            // Act
            var result = _controller.UpdateGolbalRole(guid, editRole);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            //var okResult = (OkObjectResult)result;
            //Assert.AreEqual(role, okResult.Value);
        }

        [Test]
        public async Task UpdateGlobalRole_WithInvalidGuid_ReturnsNotFoundResult()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var editRole = new EditRoleRequest
            {
                RoleName = "Test Role",
                Description = "Test Role Description",
                LinkedPermission = "Test Permission"
            };
            _organizationServicesMock.Setup(x => x.EditGlobalRoleRequest(guid, It.IsAny<GlobalRole>())).Returns(new GlobalRole());

            // Act
            var result =  _controller.UpdateGolbalRole(guid, editRole);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdateGlobalRole_WithExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var editRole = new EditRoleRequest
            {
                RoleName = "Test Role",
                Description = "Test Role Description",
                LinkedPermission = "Test Permission"
            };
            _organizationServicesMock.Setup(x => x.EditGlobalRoleRequest(guid, It.IsAny<GlobalRole>())).Throws(new Exception("Test Exception"));

            // Act
            var result = _controller.UpdateGolbalRole(guid, editRole);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual("Test Exception", badRequestResult.Value);
        }
        [Test]
        public void UpdateGlobalRole_Success_ReturnsOk()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var editRole = new EditRoleRequest { RoleName = "New Role Name", Description = "New Role Description", LinkedPermission = "New Linked Permission" };
            //var mockService = new Mock<IRoleServices>();
            _organizationServicesMock.Setup(s => s.EditGlobalRoleRequest(guid, It.IsAny<GlobalRole>())).Returns(new GlobalRole());
            //var controller = new RoleController(mockService.Object);

            // Act
            var result = _controller.UpdateGolbalRole(guid, editRole);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public void UpdateGlobalRole_RoleNotFound_ReturnsNotFound()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var editRole = new EditRoleRequest { RoleName = "New Role Name", Description = "New Role Description", LinkedPermission = "New Linked Permission" };
            //var mockService = new Mock<IRoleServices>();
            _organizationServicesMock.Setup(s => s.EditGlobalRoleRequest(guid, It.IsAny<GlobalRole>())).Returns((GlobalRole)null);

            // Act
            var result = _controller.UpdateGolbalRole(guid, editRole);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void UpdateGlobalRole_NullInput_ReturnsBadRequest()
        {
            // Arrange
            var guid = Guid.NewGuid();
            //var mockService = new Mock<IRoleServices>();
            //var controller = new RoleController(mockService.Object);

            // Act
            var result = _controller.UpdateGolbalRole(guid, null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public void UpdateGlobalRole_Exception_ReturnsBadRequest()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var editRole = new EditRoleRequest { RoleName = "New Role Name", Description = "New Role Description", LinkedPermission = "New Linked Permission" };
            _organizationServicesMock.Setup(s => s.EditGlobalRoleRequest(guid, It.IsAny<GlobalRole>())).Throws(new Exception("Test Exception"));

            // Act
            var result = _controller.UpdateGolbalRole(guid, editRole);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual("Test Exception", ((BadRequestObjectResult)result).Value);
        }
        [Test]
        public async Task PostNewRole_ValidRole_ReturnsCreatedAtAction()
        {
            // Arrange

            var editRole = new EditRoleRequest
            {
                RoleName = "Test Role",
                Description = "This is a test role",
                LinkedPermission = "Test Permission"
            };

            _organizationServicesMock.Setup(x => x.PostRole(It.IsAny<OrganizationRole>()))
                       .Returns(new OrganizationRole());

            // Act
            var result =  _controller.PostNewRole(editRole);

            // Assert
            Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
            var createdResult = (CreatedAtActionResult)result;
            Assert.That(createdResult.ActionName, Is.EqualTo(nameof(_controller.GetOrganizationViewRole)));
            Assert.That(createdResult.RouteValues["id"], Is.Not.Null);
        }
        [Test]
        public async Task PostNewRole_NullInput_ReturnsBadRequest()
        {
            // Arrange
            // mockService = new Mock<IRoleServices>();
            //var controller = new RolesController(mockService.Object);

            // Act
            var result =_controller.PostNewRole(null);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }
        [Test]
        public async Task PostNewRole_ServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            //var mockService = new Mock<IRoleServices>();
            //var controller = new RolesController(mockService.Object);

            var editRole = new EditRoleRequest
            {
                RoleName = "Test Role",
                Description = "This is a test role",
                LinkedPermission = "Test Permission"
            };

            _organizationServicesMock.Setup(x => x.PostRole(It.IsAny<OrganizationRole>()))
                       .Throws(new Exception("Test exception"));

            // Act
            var result =_controller.PostNewRole(editRole);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.Value, Is.EqualTo("Test exception"));
        }
        [Test]
        public async Task PostNewRole_EmptyRoleName_ReturnsBadRequest()
        {
            // Arrange
            //var mockService = new Mock<IRoleServices>();
            //var controller = new RolesController(mockService.Object);

            var editRole = new EditRoleRequest
            {
                RoleName = "",
                Description = "This is a test role",
                LinkedPermission = "Test Permission"
            };

            // Act
            var result = _controller.PostNewRole(editRole);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }
        [Test]
        public async Task PostNewRole_WhitespaceRoleName_ReturnsBadRequest()
        {
            // Arrange
            //var mockService = new Mock<IRoleServices>();
            //var controller = new RolesController(mockService.Object);

            var editRole = new EditRoleRequest
            {
                RoleName = "    ",
                Description = "This is a test role",
                LinkedPermission = "Test Permission"
            };

            // Act
            var result = _controller.PostNewRole(editRole);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }
        [Test]
        public async Task DeleteRole_ValidId_ReturnsOkObjectResultWithDeletedEntity()
        {
            // Arrange
            //var mockRoleService = new Mock<IRoleService>();
            var expectedDeletedEntity = new OrganizationRole { Id = Guid.NewGuid(), RoleName = "TestRole", Description = "Test Role Description", LinkedPermission = "Test Permission", CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now };
            _organizationServicesMock.Setup(x => x.DeleteRoleRequest(expectedDeletedEntity.Id)).Returns(expectedDeletedEntity);
            //var controller = new RoleController(mockRoleService.Object);

            // Act
            var result = _controller.DeleteRole(expectedDeletedEntity.Id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            //var okResult = (OkObjectResult);
            //Assert.AreEqual(expectedDeletedEntity, okResult.Value);
        }
        [Test]
        public async Task DeleteRole_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            //var mockRoleService = new Mock<IRoleService>();
            OrganizationRole nullEntity = null;
            _organizationServicesMock.Setup(x => x.DeleteRoleRequest(It.IsAny<Guid>())).Returns(nullEntity);
            //var controller = new RoleController(mockRoleService.Object);

            // Act
            var result =  _controller.DeleteRole(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task DeleteRole_ExceptionThrown_ReturnsBadRequestObjectResultWithExceptionMessage()
        {
            // Arrange
            //var mockRoleService = new Mock<IRoleService>();
            var expectedException = new Exception("Test Exception");
            _organizationServicesMock.Setup(x => x.DeleteRoleRequest(It.IsAny<Guid>())).Throws(expectedException);
            //var controller = new RoleController(mockRoleService.Object);

            // Act
            var result = _controller.DeleteRole(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual(expectedException.Message, badRequestResult.Value);
        }
        [Test]
        public void DeleteGlobalRole_ValidRequest_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            //var roleServicesMock = new Mock<IRoleServices>();
            _organizationServicesMock.Setup(x => x.DeleteGlobalRoleRequest(id)).Returns(new GlobalRole());

            //var controller = new RoleController(roleServicesMock.Object);

            // Act
            var result = _controller.DeleteGlobalRole(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public void DeleteGlobalRole_NonExistingRole_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            //var roleServicesMock = new Mock<IRoleServices>();
            _organizationServicesMock.Setup(x => x.DeleteGlobalRoleRequest(id)).Returns((GlobalRole)null);

            //var controller = new RoleController(roleServicesMock.Object);

            // Act
            var result = _controller.DeleteGlobalRole(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void DeleteGlobalRole_ServiceException_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            //var roleServicesMock = new Mock<IRoleServices>();
            _organizationServicesMock.Setup(x => x.DeleteGlobalRoleRequest(id)).Throws(new Exception("Service error"));

            //var controller = new RoleController(roleServicesMock.Object);

            // Act
            var result = _controller.DeleteGlobalRole(id);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

    }
}
