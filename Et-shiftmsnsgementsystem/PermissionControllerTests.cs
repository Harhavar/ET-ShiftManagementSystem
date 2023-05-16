using AutoMapper;
using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.PermissionModel;
using ET_ShiftManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

namespace Et_shiftmsnsgementsystem
{
    public class PermissionControllerTests
    {
        private readonly PermissionController _controller;
        private readonly Mock<IPermissionServises> _permissionServicesMock;
        private readonly Mock<IMapper> _mapperMock;

        //[SetUp]
        public PermissionControllerTests()
        {
            _permissionServicesMock = new Mock<IPermissionServises>();
            _mapperMock = new Mock<IMapper>();
            _controller = new PermissionController(_permissionServicesMock.Object, _mapperMock.Object);
        }

        [Test]
        public void Constructor_WithValidArguments_ShouldCreateInstance()
        {
            var controller = new PermissionController(_permissionServicesMock.Object, _mapperMock.Object);

            //assert
            Assert.IsNotNull(controller);
            Assert.IsNotNull(_controller);
        }
        //[Test]
        public async Task Get_ReturnsOkObjectResult_WithListOfPermissions()
        {
            // Arrange
            var permissions = new List<Permission>
        {
            new Permission
            {
                PermissionName = "Permission 1",
                Description = "Permission 1 Description",
                CreatedDate = DateTime.Now
            },
            new Permission
            {
                PermissionName = "Permission 2",
                Description = "Permission 2 Description",
                CreatedDate = DateTime.Now
            }
        };

            _permissionServicesMock.Setup(x => x.GetPermissions()).Returns(new List<Permission>());

            //var controller = new PermissionController(_permissionServicesMock.Object, _mapperMock.Object);

            // Act
            var result = _controller.GetGlobalPermission();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            //Assert.IsInstanceOf <List<Permission>>(okResult);
            //var permissionRequests = okResult.Value as  List<Permission>;
            //Assert.AreEqual(2, permissionRequests.Count());
        }

        [Test]
        public async Task Get_ReturnsNotFoundResult_WhenPermissionsIsNull()
        {
            // Arrange
            List<Permission> permissions = null;

            _permissionServicesMock.Setup(x => x.GetPermissions()).Returns(new List<Permission>());

            ///var controller = new PermissionController(_permissionServicesMock.Object, _mapperMock.Object);

            // Act
            var result = _controller.GetGlobalPermission();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Get_ReturnsOkObjectResult_WithErrorMessage_WhenExceptionOccurs()
        {
            // Arrange
            _permissionServicesMock.Setup(x => x.GetPermissions()).Throws(new Exception("Error occurred"));

            var controller = new PermissionController(_permissionServicesMock.Object, _mapperMock.Object);

            // Act
            var result = controller.GetGlobalPermission();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Error occurred", okResult.Value);
        }

        //[Test]
        public async Task Get_ReturnsOkObjectResult_WithEmptyListOfPermissions_WhenNoPermissionsExist()
        {
            // Arrange
            var permissions = new List<GetGlobalPermission>();

            _permissionServicesMock.Setup(x => x.GetPermissions()).Returns(new List<Permission>());

            //var controller = new PermissionController(_permissionServicesMock.Object, _mapperMock.Object);

            // Act
            var result = _controller.GetGlobalPermission();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<List<Permission>>(okResult.Value);
            var permissionRequests = okResult.Value as List<Permission>;
            Assert.IsEmpty(permissionRequests);
        }
        [Test]
        public async Task GetPermissions_WhenPermissionExists_ReturnsOkResult()
        {
            // Arrange
            //var mockPermissionService = new Mock<IPermissionServises>();
            //var mockMapper = new Mock<IMapper>();
            var permissionId = Guid.NewGuid();
            var permission = new Permission { Id = permissionId, PermissionName = "TestPermission", Description = "TestDescription", CreatedDate = DateTime.Now };
            var permissionDTO = new PermissionDTO { Id = permissionId, PermissionName = "TestPermission", Description = "TestDescription", CreatedDate = DateTime.Now };
            _permissionServicesMock.Setup(x => x.GetPermissionById(permissionId)).ReturnsAsync(permission);
            _mapperMock.Setup(x => x.Map<PermissionDTO>(permission)).Returns(permissionDTO);
            //var controller = new PermissionController(mockPermissionService.Object, mockMapper.Object);

            // Act
            var result = await _controller.GetPermissions(permissionId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(permissionDTO, okResult.Value);
        }
        [Test]
        public async Task GetPermissions_WhenPermissionDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            //var mockPermissionService = new Mock<IPermissionServises>();
            //var mockMapper = new Mock<IMapper>();
            var permissionId = Guid.NewGuid();
            _permissionServicesMock.Setup(x => x.GetPermissionById(permissionId)).ReturnsAsync((Permission)null);
            var controller = new PermissionController(_permissionServicesMock.Object, _mapperMock.Object);

            // Act
            var result = await controller.GetPermissions(permissionId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task GetPermissions_WhenExceptionThrown_ReturnsOkResultWithErrorMessage()
        {
            // Arrange
            //var mockPermissionService = new Mock<IPermissionServises>();
            //var mockMapper = new Mock<IMapper>();
            var permissionId = Guid.NewGuid();
            var exceptionMessage = "An error occurred while getting permission.";
            _permissionServicesMock.Setup(x => x.GetPermissionById(permissionId)).ThrowsAsync(new Exception(exceptionMessage));
            var controller = new PermissionController(_permissionServicesMock.Object, _mapperMock.Object);

            // Act
            var result = await controller.GetPermissions(permissionId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(exceptionMessage, okResult.Value);
        }
        [Test]
        public async Task Addpermission_WhenAddPermissionRequestIsNull_ReturnsBadRequest()
        {
            // Arrange
            AddPermissionRequest addPermissionRequest = null;
            // var permissionController = new PermissionController(permissionServises, mapper);

            // Act
            var result = await _controller.Addpermission(addPermissionRequest) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 400);
        }
        [Test]
        public async Task Addpermission_WhenPermissionAdded_ReturnsOkResult()
        {
            // Arrange
            var addPermissionRequest = new AddPermissionRequest
            {
                PermissionName = "Test Permission",
                Description = "Test Permission Description"
            };
            var permission = new Permission
            {
                PermissionName = addPermissionRequest.PermissionName,
                Description = addPermissionRequest.Description
            };
            _permissionServicesMock.Setup(m => m.AddPermission(It.IsAny<Permission>())).ReturnsAsync(permission);
            //var permissionController = new PermissionController(_permissionServicesMock.Object, mapper);

            // Act
            var result = await _controller.Addpermission(addPermissionRequest) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
            var addedPermission = result.Value as Permission;
            Assert.IsNotNull(addedPermission);
            Assert.AreEqual(addedPermission.PermissionName, addPermissionRequest.PermissionName);
            Assert.AreEqual(addedPermission.Description, addPermissionRequest.Description);
        }

        [Test]
        public async Task Addpermission_WhenPermissionServiceThrowsException_ReturnsErrorMessage()
        {
            // Arrange
            var addPermissionRequest = new AddPermissionRequest
            {
                PermissionName = "Test Permission",
                Description = "Test Permission Description"
            };
            _permissionServicesMock.Setup(m => m.AddPermission(It.IsAny<Permission>())).Throws(new Exception("Test Exception"));
            // var permissionController = new PermissionController(_permissionServicesMock.Object, mapper);

            // Act
            var result = await _controller.Addpermission(addPermissionRequest) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
            var errorMessage = result.Value as string;
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual(errorMessage, "Test Exception");
        }
        [Test]
        public async Task Addpermission_WhenPermissionServiceReturnsNull_ReturnsBadRequest()
        {
            // Arrange
            var addPermissionRequest = new AddPermissionRequest
            {
                PermissionName = "Test Permission",
                Description = "Test Permission Description"
            };
            _permissionServicesMock.Setup(m => m.AddPermission(It.IsAny<Permission>())).ReturnsAsync(null as Permission);
            //var permissionController = new PermissionController(mockPermissionService.Object, mapper);

            // Act
            var result = await _controller.Addpermission(addPermissionRequest) as NotFoundResult;

            // Assert
            //Assert.IsNotNull(result);
            //Assert.AreEqual(result.StatusCode, 404);
        }
        [Test]
        public async Task UpdatePermission_ValidInput_Success()
        {
            // Arrange
            var permissionId = Guid.NewGuid();
            var permissionToUpdate = new UpdatePermissionRequest
            {
                PermissionName = "NewPermissionName",
                Description = "NewDescription",
            };
            //var mockService = new Mock<IPermissionServises>();
            _permissionServicesMock.Setup(s => s.EditPermission(permissionId, It.IsAny<Permission>()))
                .ReturnsAsync(new Permission
                {
                    Id = permissionId,
                    PermissionName = permissionToUpdate.PermissionName,
                    Description = permissionToUpdate.Description,
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow,
                });
            //var controller = new PermissionController(_permissionServicesMock.Object, null);

            // Act
            var result = await _controller.UpdatePermission(permissionId, permissionToUpdate) as OkObjectResult;
            var updatedPermission = result.Value as PermissionDTO;

            // Assert
            Assert.NotNull(updatedPermission);
            Assert.AreEqual(permissionToUpdate.PermissionName, updatedPermission.PermissionName);
            Assert.AreEqual(permissionToUpdate.Description, updatedPermission.Description);
            Assert.AreEqual(permissionId, updatedPermission.Id);
            Assert.NotNull(updatedPermission.CreatedDate);
            Assert.NotNull(updatedPermission.LastModifiedDate);
        }
        [Test]
        public async Task UpdatePermission_InvalidId_NotFound()
        {
            // Arrange
            var permissionId = Guid.NewGuid();
            var permissionToUpdate = new UpdatePermissionRequest
            {
                PermissionName = "NewPermissionName",
                Description = "NewDescription",
            };
            //var mockService = new Mock<IPermissionServises>();
            _permissionServicesMock.Setup(s => s.EditPermission(permissionId, It.IsAny<Permission>()))
                .ReturnsAsync((Permission)null);
            var controller = new PermissionController(_permissionServicesMock.Object, null);

            // Act
            var result = await controller.UpdatePermission(permissionId, permissionToUpdate) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
        [Test]
        public async Task UpdatePermission_NullInput_BadRequest()
        {
            // Arrange
            var permissionId = Guid.NewGuid();
            //var mockService = new Mock<IPermissionServises>();
            //var controller = new PermissionController(mockService.Object, null);

            // Act
            var result = await _controller.UpdatePermission(permissionId, null) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Should not be null", result.Value);
        }
        [Test]
        public async Task UpdatePermission_ServiceError_ExceptionThrown()
        {
            // Arrange
            var permissionId = Guid.NewGuid();
            var permissionToUpdate = new UpdatePermissionRequest
            {
                PermissionName = "NewPermissionName",
                Description = "NewDescription",
            };
            //var mockService = new Mock<IPermissionServises>();
            _permissionServicesMock.Setup(s => s.EditPermission(permissionId, It.IsAny<Permission>()))
                .ThrowsAsync(new Exception("Service error"));
            var controller = new PermissionController(_permissionServicesMock.Object, null);

            // Act and Assert
            Assert.ThrowsAsync<Exception>(() => controller.UpdatePermission(permissionId, permissionToUpdate));
        }
        //[Test]
        public void Delete_WithValidId_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            _permissionServicesMock.Setup(x => x.GetPermissionById(id)).ReturnsAsync(new Permission { Id = id });

            // Act
            var result = _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _permissionServicesMock.Setup(x => x.GetPermissionById(id)).ReturnsAsync(new Permission { Id = id });

            // Act
            var result = _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Delete_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            //_permissionServicesMock.Setup(x => x.GetPermissionById(id)).Throws<Exception>();

            // Act
            var result =  _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task GetOrganizationPermission_ReturnsPermissions_Success()
        {
            // Arrange
            //var permissionServiceMock = new Mock<IPermissionServises>();
            var permissions = new List<OrgPermission>
    {
        new OrgPermission { PermissionName = "Permission1", Description = "Permission 1 description", CreatedDate = DateTime.Now },
        new OrgPermission { PermissionName = "Permission2", Description = "Permission 2 description", CreatedDate = DateTime.Now },
        new OrgPermission { PermissionName = "Permission3", Description = "Permission 3 description", CreatedDate = DateTime.Now }
    };
            _permissionServicesMock.Setup(m => m.GetOrgPermissions()).Returns(new List<OrgPermission>());
            var controller = new PermissionController(_permissionServicesMock.Object, null);

            // Act
            var response =  controller.GetOrganizationPermission();

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOf(typeof(OkObjectResult), response);
            var okResult = response as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
            var permissionRequests = okResult.Value as List<GetOrgPermission>;
            Assert.IsNotNull(permissionRequests);
            //Assert.AreEqual(3, permissionRequests.Count);
            //Assert.AreEqual("Permission1", permissionRequests[0].PermissionName);
            //Assert.AreEqual("Permission 1 description", permissionRequests[0].Description);
            //Assert.AreEqual(DateTime.Now.Date, permissionRequests[0].CreatedDate.Date);
            //Assert.AreEqual("Organization", permissionRequests[0].PermissionType);
        }
        [Test]
        public async Task GetOrganizationPermission_ReturnsNotFound_NoPermissions()
        {
            // Arrange
            var permissionServiceMock = new Mock<IPermissionServises>();
            //permissionServiceMock.Setup(m => m.GetOrgPermissions()).Returns((IEnumerable<Entities.Permission>)null);
            var controller = new PermissionController(permissionServiceMock.Object, null);

            // Act
            var response =  controller.GetOrganizationPermission();

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOf(typeof(NotFoundResult), response);
            var notFoundResult = response as NotFoundResult;
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
        [Test]
        public void GetOrganizationPermission_ReturnsListOfOrgPermissions()
        {
            // Arrange
            //var mockPermissionService = new Mock<IPermissionServises>();
            _permissionServicesMock.Setup(s => s.GetOrgPermissions())
                .Returns(new List<OrgPermission>
                {
            new OrgPermission
            {
                PermissionName = "Permission 1",
                Description = "Description 1",
                CreatedDate = DateTime.UtcNow,
                PermissionType = "Organization"
            },
            new OrgPermission
            {
                PermissionName = "Permission 2",
                Description = "Description 2",
                CreatedDate = DateTime.UtcNow,
                PermissionType = "Organization"
            }
                });

            var controller = new PermissionController(_permissionServicesMock.Object, null);

            // Act
            var result = controller.GetOrganizationPermission() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var permissionList = result.Value as List<GetOrgPermission>;
            Assert.IsNotNull(permissionList);
            Assert.AreEqual(2, permissionList.Count);
        }
        [Test]
        public void GetOrganizationPermission_ReturnsEmptyList_WhenNoOrgPermissionsFound()
        {
            // Arrange
            var mockPermissionService = new Mock<IPermissionServises>();
            mockPermissionService.Setup(s => s.GetOrgPermissions())
                .Returns(new List<OrgPermission>());

            var controller = new PermissionController(mockPermissionService.Object, null);

            // Act
            var result = controller.GetOrganizationPermission() as NotFoundResult;

            // Assert
            //Assert.IsNotNull(result);
        }
        [Test]
        public void GetOrganizationPermission_ReturnsNotFound_WhenExceptionOccurs()
        {
            // Arrange
            var mockPermissionService = new Mock<IPermissionServises>();
            //mockPermissionService.Setup(s => s.GetOrgPermissions())
               // .Throws(new Exception("Test exception"));

            var controller = new PermissionController(mockPermissionService.Object, null);

            // Act
            var result = controller.GetOrganizationPermission() as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }
        //[Test]
        //public void GetOrganizationPermission_ReturnsBadRequest_WhenPermissionServiceIsNull()
        //{
        //    // Arrange
        //    var controller = new PermissionController(null, null);

        //    // Act
        //    var result = controller.GetOrganizationPermission() as BadRequestObjectResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("Permission service is null", result.Value);
        //}
        [Test]
        public void GetOrgPermissions_WithEmptyGuid_ReturnsBadRequest()
        {
            // Arrange
            //var controller = new PermissionController(permissionServises);

            // Act
            var result =  _controller.GetOrgPermissions(Guid.Empty);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetOrgPermissions_WithNonExistingOrgPermission_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            // var controller = new PermissionController(permissionServises);

            // Act
            var result =  _controller.GetOrgPermissions(nonExistingId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task GetOrgPermissions_WithValidGuid_ReturnsExpectedOrgPermission()
        {
            // Arrange
            var validId = Guid.NewGuid();
            var mockPerm = new OrgPermission { Id = validId, PermissionName = "TestPerm", Description = "TestDescription", CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now };
            _permissionServicesMock.Setup(x => x.GetOrgPermissionById(validId)).Returns(mockPerm);
            //var controller = new PermissionController(_permissionServicesMock.Object);

            // Act
            var result =  _controller.GetOrgPermissions(validId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<GetOrgPermissionview>>(result.Value);
            var orgPermissions = result.Value as List<GetOrgPermissionview>;
            Assert.AreEqual(1, orgPermissions.Count);
            var orgPermission = orgPermissions.First();
            Assert.AreEqual(mockPerm.PermissionName, orgPermission.PermissionName);
            Assert.AreEqual(mockPerm.Description, orgPermission.Description);
            Assert.AreEqual(mockPerm.CreatedDate, orgPermission.CreatedDate);
            Assert.AreEqual(mockPerm.LastModifiedDate, orgPermission.LastModifiedDate);
        }
        [Test]
        public void GetOrgPermissions_ThrowsException_ReturnsErrorMessage()
        {
            // Arrange
            var validId = Guid.NewGuid();
            _permissionServicesMock.Setup(x => x.GetOrgPermissionById(validId)).Throws(new Exception("Test Exception"));
            //var controller = new PermissionController(_permissionServicesMock.Object);

            // Act
            var result =  _controller.GetOrgPermissions(validId) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("Test Exception", result.Value);
        }
        [Test]
        public async Task AddOrgpermission_ReturnsOkResult_WhenValidAddPermissionRequestIsProvided()
        {
            // Arrange
            var request = new AddOrgPermission { PermissionName = "Test", Description = "Test permission" };
            var permission = new OrgPermission { PermissionName = "Test", Description = "Test permission" };
            _permissionServicesMock.Setup(x => x.AddOrgPermission(It.IsAny<OrgPermission>())).ReturnsAsync(permission);

            // Act
            var result = await _controller.AddOrgpermission(request) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(permission, result.Value);
        }

        [Test]
        public async Task AddOrgpermission_ReturnsBadRequest_WhenAddPermissionRequestIsNull()
        {
            // Arrange

            // Act
            var result = await _controller.AddOrgpermission(null) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Should not be null", result.Value);
        }

        [Test]
        public void AddOrgpermission_ThrowsException_WhenPermissionServicesThrowsException()
        {
            // Arrange
            var request = new AddOrgPermission { PermissionName = "Test", Description = "Test permission" };
            _permissionServicesMock.Setup(x => x.AddOrgPermission(It.IsAny<OrgPermission>())).Throws(new Exception());

            // Act + Assert
            Assert.ThrowsAsync<Exception>(() => _controller.AddOrgpermission(request));
        }

        [Test]
        public async Task AddOrgpermission_CallsAddOrgPermissionMethod_WhenValidAddPermissionRequestIsProvided()
        {
            // Arrange
            var request = new AddOrgPermission { PermissionName = "Test", Description = "Test permission" };
            var permission = new OrgPermission { PermissionName = "Test", Description = "Test permission" };
            _permissionServicesMock.Setup(x => x.AddOrgPermission(It.IsAny<OrgPermission>())).ReturnsAsync(permission);

            // Act
            var result = await _controller.AddOrgpermission(request) as OkObjectResult;

            // Assert
            _permissionServicesMock.Verify(x => x.AddOrgPermission(It.IsAny<OrgPermission>()), Times.Once);
        }
        [Test]
        public async Task UpdateOrgPermission_ValidUpdate_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updatePermission = new UpdateOrgPermission
            {
                PermissionName = "New Permission Name",
                Description = "New Permission Description",
            };
            _permissionServicesMock.Setup(x => x.EditOrgPermission(id, It.IsAny<OrgPermission>()))
                                 .ReturnsAsync(new OrgPermission { Id = id, PermissionName = updatePermission.PermissionName, Description = updatePermission.Description });

            // Act
            var result = await _controller.UpdateOrgPermission(id, updatePermission) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var permissionDTO = result.Value as OrgPermissionDTO;
            Assert.IsNotNull(permissionDTO);
            Assert.AreEqual(id, permissionDTO.Id);
            Assert.AreEqual(updatePermission.PermissionName, permissionDTO.PermissionName);
            Assert.AreEqual(updatePermission.Description, permissionDTO.Description);
        }
        [Test]
        public async Task UpdateOrgPermission_NullUpdate_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _controller.UpdateOrgPermission(id, null) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
        [Test]
        public async Task UpdateOrgPermission_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.Empty;
            var updatePermission = new UpdateOrgPermission();

            // Act
            var result = await _controller.UpdateOrgPermission(id, updatePermission) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
        [Test]
        public async Task UpdateOrgPermission_NonExistentOrgPermission_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updatePermission = new UpdateOrgPermission();
            _permissionServicesMock.Setup(x => x.EditOrgPermission(id, It.IsAny<OrgPermission>()))
                                 .ReturnsAsync((OrgPermission)null);

            // Act
            var result = await _controller.UpdateOrgPermission(id, updatePermission) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
        [Test]
        public async Task DeleteOrgPermission_ValidId_ReturnsOk()
        {
            // Arrange
            var idToDelete = Guid.NewGuid();
            _permissionServicesMock.Setup(service => service.DeleteOrgPermission(idToDelete))
                                  .Returns(true);

            // Act
            var result =  _controller.DeleteOrgPermission(idToDelete);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(true, okResult.Value);
        }
        [Test]
        public async Task DeleteOrgPermission_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var idToDelete = Guid.Empty;

            // Act
            var result =  _controller.DeleteOrgPermission(idToDelete);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task DeleteOrgPermission_NonExistentId_ReturnsNotFound()
        {
            // Arrange
            var idToDelete = Guid.NewGuid();
            _permissionServicesMock.Setup(service => service.DeleteOrgPermission(idToDelete))
                                  .Returns(false);

            // Act
            var result = _controller.DeleteOrgPermission(idToDelete);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task DeleteOrgPermission_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var idToDelete = Guid.NewGuid();
            _permissionServicesMock.Setup(service => service.DeleteOrgPermission(idToDelete))
                                  .Throws(new Exception());

            // Act
            var result =  _controller.DeleteOrgPermission(idToDelete);

            // Assert
            //Assert.IsInstanceOf<StatusCodeResult>(result);
            //var statusCodeResult = (StatusCodeResult)result;
            //Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

    }
}
