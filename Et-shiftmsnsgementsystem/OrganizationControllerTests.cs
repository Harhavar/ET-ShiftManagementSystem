using AutoMapper;
using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Services;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Et_shiftmsnsgementsystem
{
    public class OrganizationControllerTests
    {
        private Mock<IorganizationServices> _organizationServicesMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IEmailSender> _emailSenderMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private readonly OrganizationController _controller;

        //[SetUp]
        public OrganizationControllerTests()
        {
            _organizationServicesMock = new Mock<IorganizationServices>();
            _mapperMock = new Mock<IMapper>();
            _emailSenderMock = new Mock<IEmailSender>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _controller = new OrganizationController(_organizationServicesMock.Object, _mapperMock.Object, _emailSenderMock.Object, _userRepositoryMock.Object);

        }

        [Test]
        public void Constructor_WithValidArguments_ShouldCreateInstance()
        {
            // Arrange

            // Act
            var controller = new OrganizationController(_organizationServicesMock.Object, _mapperMock.Object, _emailSenderMock.Object, _userRepositoryMock.Object);

            // Assert
            Assert.IsNotNull(controller);
        }
        [Test]
        public async Task Get_ReturnsOkObjectResult_WhenOrganizationDataExists()
        {
            // Arrange
            var organizationList = new List<Organization>
        {
            new Organization
            {
                OrganizationName = "Org1",
                EmailAddress = "org1@example.com",
                PhoneNumber = "1234567890",
                CreatedDate = DateTime.UtcNow
            },
            new Organization
            {
                OrganizationName = "Org2",
                EmailAddress = "org2@example.com",
                PhoneNumber = "0987654321",
                CreatedDate = DateTime.UtcNow
            }
        };

            _organizationServicesMock.Setup(x => x.GetOrganizationData()).Returns(organizationList);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okObjectResult = result.Result as OkObjectResult;
            var organizationRequestList = okObjectResult.Value as List<ET_ShiftManagementSystem.Models.organizationModels.organizationdataRequest>;
            Assert.IsNotNull(organizationRequestList);
            Assert.AreEqual(organizationList.Count, organizationRequestList.Count);
            for (int i = 0; i < organizationList.Count; i++)
            {
                Assert.AreEqual(organizationList[i].OrganizationName, organizationRequestList[i].OrganizationName);
                Assert.AreEqual(organizationList[i].EmailAddress, organizationRequestList[i].EmailAddress);
                Assert.AreEqual(organizationList[i].PhoneNumber, organizationRequestList[i].PhoneNumber);
                Assert.AreEqual(organizationList[i].CreatedDate, organizationRequestList[i].CreatedDate);
            }
        }
        [Test]
        public async Task Get_ReturnsNotFoundResult_WhenOrganizationDataIsNull()
        {
            // Arrange
            _organizationServicesMock.Setup(x => x.GetOrganizationData()).Returns(null as List<Organization>);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task Get_ReturnsOkObjectResultWithEmptyList_WhenOrganizationDataIsEmpty()
        {
            // Arrange
            _organizationServicesMock.Setup(x => x.GetOrganizationData()).Returns(new List<Organization>());

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okObjectResult = result.Result as OkObjectResult;
            var organizationRequestList = okObjectResult.Value as List<ET_ShiftManagementSystem.Models.organizationModels.organizationdataRequest>;
            Assert.IsNotNull(organizationRequestList);
            Assert.AreEqual(0, organizationRequestList.Count);
        }
        [Test]
        public async Task GetDetails_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<IorganizationServices>();
            var mockMapper = new Mock<IMapper>();
            var mockEmailSender = new Mock<IEmailSender>();
            var mockUserRepository = new Mock<IUserRepository>();

            var organizationList = new List<Organization>()
    {
        new Organization() { TenentID = Guid.NewGuid(), OrganizationName = "Org1" },
        new Organization() { TenentID =Guid.NewGuid(), OrganizationName = "Org2" }
    };

            mockService.Setup(x => x.GetOrganizationData()).Returns(organizationList);

            var controller = new OrganizationController(mockService.Object, mockMapper.Object, mockEmailSender.Object, mockUserRepository.Object);

            // Act
            var result = await controller.GetDetails();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsInstanceOf<IEnumerable<ET_ShiftManagementSystem.Models.organizationModels.OrganizationDetailsRequest>>(okResult.Value);
            var organizationRequestList = okResult.Value as IEnumerable<ET_ShiftManagementSystem.Models.organizationModels.OrganizationDetailsRequest>;
            Assert.AreEqual(organizationList.Count, organizationRequestList.Count());
        }
        [Test]
        public async Task GetDetails_ReturnsNotFound_WhenOrganizationIsNull()
        {
            // Arrange
            var mockServices = new Mock<IorganizationServices>();
            mockServices.Setup(s => s.GetOrganizationData()).Returns((List<Organization>)null);
            var controller = new OrganizationController(mockServices.Object, null, null, null);

            // Act
            var result = await controller.GetDetails();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task GetDetails_ReturnsBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            var mockService = new Mock<IorganizationServices>();
            mockService.Setup(x => x.GetOrganizationData()).Throws(new Exception("Test exception"));
            var controller = new OrganizationController(mockService.Object, null, null, null);

            // Act
            var result = await controller.GetDetails();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
        [Test]
        public async Task GetOrganizationById_ReturnsOk_WhenOrganizationExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var organization = new Organization { TenentID = id, OrganizationName = "Test Org" };
            var organizationDTO = new ET_ShiftManagementSystem.Models.organizationModels.Organization { TenentID = id, OrganizationName = "Test Org" };
            _organizationServicesMock.Setup(services => services.GetOrgByID(id)).ReturnsAsync(organization);
            _mapperMock.Setup(mapper => mapper.Map<ET_ShiftManagementSystem.Models.organizationModels.Organization>(organization)).Returns(organizationDTO);

            // Act
            var result = await _controller.GetOrganizationById(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<ET_ShiftManagementSystem.Models.organizationModels.Organization>(okResult.Value);
            Assert.AreEqual(organizationDTO, okResult.Value);
        }
        [Test]
        public async Task GetOrganizationById_ReturnsNotFound_WhenOrganizationDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _organizationServicesMock.Setup(services => services.GetOrgByID(id)).ReturnsAsync((Organization)null);

            // Act
            var result = await _controller.GetOrganizationById(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        //[Test]
        //public async Task GetOrganizationById_WhenIdIsValid_ReturnsOk()
        //{
        //    // Arrange
        //    var mockService = new Mock<IorganizationServices>();
        //    var mockMapper = new Mock<IMapper>();
        //   // var controller = new OrganizationController(mockService.Object, mockMapper.Object);

        //    var organizationId = Guid.NewGuid();
        //    var organization = new Organization { TenentID = organizationId, OrganizationName = "TestOrg" };
        //    mockService.Setup(x => x.GetOrgByID(organizationId)).ReturnsAsync(organization);

        //    // Act
        //    var result = await _controller.GetOrganizationById(organizationId);

        //    // Assert
        //    Assert.IsInstanceOf<OkObjectResult>(result);
        //    var okResult = result as OkObjectResult;
        //    Assert.AreEqual(organizationId, ((ET_ShiftManagementSystem.Models.organizationModels.Organization)okResult.Value).TenentID);
        //}

        [Test]
        public async Task GetOrganizationById_WhenIdIsInvalid_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<IorganizationServices>();
            var mockMapper = new Mock<IMapper>();
            //var controller = new OrganizationController(mockService.Object, mockMapper.Object);

            var organizationId = Guid.NewGuid();
            mockService.Setup(x => x.GetOrgByID(organizationId)).ReturnsAsync((Organization)null);

            // Act
            var result = await _controller.GetOrganizationById(organizationId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetOrganizationById_WhenIdIsEmpty_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<IorganizationServices>();
            var mockMapper = new Mock<IMapper>();
            //var controller = new OrganizationController(mockService.Object, mockMapper.Object);

            // Act
            var result = await _controller.GetOrganizationById(Guid.Empty);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task AddOrganization_WhenBadRequest_ReturnsBadRequest()
        {
            // Arrange
            var controller = new OrganizationController(null, null, null, null);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.AddOrganization(null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task AddOrganization_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            //var mockService = new Mock<IOrganizationService>();
            //var mockUserRepository = new Mock<IUserRepository>();
            //var mockEmailSender = new Mock<IEmailSender>();
            _organizationServicesMock.Setup(x => x.AddOrgnization(It.IsAny<Organization>())).Throws(new Exception("Test Exception"));

            var controller = new OrganizationController(_organizationServicesMock.Object, _mapperMock.Object, _emailSenderMock.Object, _userRepositoryMock.Object);

            var request = new ET_ShiftManagementSystem.Models.organizationModels.AddOrganizationRequest()
            {
                OrganizationName = "Test Organization",
                EmailAddress = "test@example.com",
                PhoneNumber = "1234567890",
                Adminemailaddress = "admin@example.com",
                Adminfullname = "Test Admin",
                OrganizationLogo = new byte[] { 0x1, 0x2, 0x3 },
                Pincode = "123456",
                CityTown = "Test City",
                Country = "Test Country",
                StateProvince = "Test Province",
                StreetLandmark = "Test Street",
                HouseBuildingNumber = "Test Building"
            };

            // Act
            var result = await controller.AddOrganization(request);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            //Assert.AreEqual("Test Exception", ((BadRequestObjectResult)result));
        }
        [Test]
        public async Task UpdateOrganization_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var TenentID = Guid.NewGuid();
            var organizationToUpdate = new ET_ShiftManagementSystem.Models.organizationModels.UpdateOrganizationRequest
            {
                OrganizationName = "New Organization Name",
                OrganizationLogo = new byte[] { 1, 2, 3 },
                Adminemailaddress = "admin@example.com",
                StateProvince = "State",
                StreetLandmark = "Street",
                Country = "Country",
                Adminfullname = "Admin Name",
                CityTown = "City",
                EmailAddress = "contact@example.com",
                HouseBuildingNumber = "1234",
                PhoneNumber = "1234567890",
                Pincode = "123456"
            };

            var updatedOrganization = new ET_ShiftManagementSystem.Entities.Organization
            {
                TenentID = Guid.NewGuid(),
                OrganizationName = organizationToUpdate.OrganizationName,
                OrganizationLogo = organizationToUpdate.OrganizationLogo,
                Adminemailaddress = organizationToUpdate.Adminemailaddress,
                StateProvince = organizationToUpdate.StateProvince,
                StreetLandmark = organizationToUpdate.StreetLandmark,
                Country = organizationToUpdate.Country,
                Adminfullname = organizationToUpdate.Adminfullname,
                CityTown = organizationToUpdate.CityTown,
                EmailAddress = organizationToUpdate.EmailAddress,
                HouseBuildingNumber = organizationToUpdate.HouseBuildingNumber,
                PhoneNumber = organizationToUpdate.PhoneNumber,
                Pincode = organizationToUpdate.Pincode
            };

            var mockServices = new Mock<ET_ShiftManagementSystem.Services.IorganizationServices>();
            mockServices.Setup(x => x.UpdateOrganization(TenentID, It.IsAny<ET_ShiftManagementSystem.Entities.Organization>()))
                .ReturnsAsync(updatedOrganization);

            //var controller = new OrganizationController(mockServices.Object);

            // Act
            var result = await _controller.updateOrganization(TenentID, organizationToUpdate);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            //// var updateOrg = Assert.IsType<Models.organizationModels.Organization>(okResult.Value);

            // Assert.Equal(updatedOrganization.TenentID, updateOrg.TenentID);
            // Assert.Equal(updatedOrganization.OrganizationName, updateOrg.OrganizationName);
            // Assert.Equal(updatedOrganization.EmailAddress, updateOrg.EmailAddress);
            // Assert.Equal(updatedOrganization.Country, updateOrg.Country);
            // Assert.Equal(updatedOrganization.LastModifiedDate, updateOrg.LastModifiedDate);
            // Assert.Equal(updatedOrganization.Adminemailaddress, updateOrg.Adminemailaddress);
            // Assert.Equal(updatedOrganization.Adminfullname, updateOrg.Adminfullname);
            // Assert.Equal(updatedOrganization.CityTown, updateOrg.CityTown);
            // Assert.Equal(updatedOrganization.HouseBuildingNumber, updateOrg.HouseBuildingNumber);
            // Assert.Equal(updatedOrganization.PhoneNumber, updateOrg.PhoneNumber);
            // Assert.Equal(updatedOrganization.Pincode, updateOrg.Pincode);
            // Assert.Equal(updatedOrganization.OrganizationLogo, updateOrg.OrganizationLogo);
            // Assert.Equal(updatedOrganization.StateProvince, updateOrg.StateProvince);
            // Assert.Equal(updatedOrganization.StreetLandmark, updateOrg.StreetLandmark);
        }
        [Test]
        public async Task UpdateOrganization_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var TenentID = Guid.NewGuid();
            var request = new ET_ShiftManagementSystem.Models.organizationModels.UpdateOrganizationRequest
            {
                OrganizationName = "Acme Corp",
                EmailAddress = "info@acmecorp.com",
                Country = "USA",
                Adminemailaddress = "admin@acmecorp.com",
                Adminfullname = "John Doe",
                CityTown = "New York",
                HouseBuildingNumber = "123 Main St",
                PhoneNumber = "555-1234",
                Pincode = "10001",
                StateProvince = "NY",
                StreetLandmark = "Times Square",
                OrganizationLogo = new byte[] { 0x00, 0x01, 0x02, 0x03 }
            };

            var organization = new Organization
            {
                //TenentID = Guid.NewGuid(),
                OrganizationName = request.OrganizationName,
                EmailAddress = request.EmailAddress,
                Country = request.Country,
                Adminemailaddress = request.Adminemailaddress,
                Adminfullname = request.Adminfullname,
                CityTown = request.CityTown,
                HouseBuildingNumber = request.HouseBuildingNumber,
                PhoneNumber = request.PhoneNumber,
                Pincode = request.Pincode,
                StateProvince = request.StateProvince,
                StreetLandmark = request.StreetLandmark,
                OrganizationLogo = request.OrganizationLogo
            };

            _organizationServicesMock.Setup(x => x.UpdateOrganization(TenentID, It.IsAny<Organization>()))
                        .ReturnsAsync(organization);

            // Act
            var result = await _controller.updateOrganization(TenentID, request) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            var updatedOrg = result.Value as Organization;
            //Assert.IsNotNull(updatedOrg);
            //Assert.AreEqual(organization.TenentID, updatedOrg.TenentID);
            //Assert.AreEqual(organization.OrganizationName, updatedOrg.OrganizationName);
            //Assert.AreEqual(organization.EmailAddress, updatedOrg.EmailAddress);
            //Assert.AreEqual(organization.Country, updatedOrg.Country);
            //Assert.AreEqual(organization.Adminemailaddress, updatedOrg.Adminemailaddress);
            //Assert.AreEqual(organization.Adminfullname, updatedOrg.Adminfullname);
            //Assert.AreEqual(organization.CityTown, updatedOrg.CityTown);
            //Assert.AreEqual(organization.HouseBuildingNumber, updatedOrg.HouseBuildingNumber);
            //Assert.AreEqual(organization.PhoneNumber, updatedOrg.PhoneNumber);
            //Assert.AreEqual(organization.Pincode, updatedOrg.Pincode);
            //Assert.AreEqual(organization.StateProvince, updatedOrg.StateProvince);
            //Assert.AreEqual(organization.StreetLandmark, updatedOrg.StreetLandmark);
            //Assert.AreEqual(organization.OrganizationLogo, updatedOrg.OrganizationLogo);
        }
        [Test]
        public async Task DeleteOrganization_ValidId_ReturnsOk()
        {
            // Arrange
            //var mockService = new Mock<ET_ShiftManagementSystem.Services.IorganizationServices>();
            _organizationServicesMock.Setup(s => s.DeleteOrganization(It.IsAny<Guid>())).ReturnsAsync(new ET_ShiftManagementSystem.Entities.Organization());

            //var controller = new OrganizationController(mockService.Object);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
              new Claim(ClaimTypes.Role, "SystemAdmin")
            })); // Set up authorization

            var idToDelete = Guid.NewGuid();

            // Act
            var result = await
                _controller.Deleteorg(idToDelete);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf<ET_ShiftManagementSystem.Entities.Organization>(okResult.Value);
            _organizationServicesMock.Verify(s => s.DeleteOrganization(It.IsAny<Guid>()), Times.Once);
        }
        [Test]
        public async Task Deleteorg_ReturnsOkResult()
        {
            // Arrange
           // var mockService = new Mock<ET_ShiftManagementSystem.Services.IorganizationServices>();
            _organizationServicesMock.Setup(s => s.DeleteOrganization(It.IsAny<Guid>())).ReturnsAsync(new ET_ShiftManagementSystem.Entities.Organization());

            // Act
            var result = await _controller.Deleteorg(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task Deleteorg_OrganizationNotFound_ReturnsNotFound()
        {
            // Arrange
            //var mockService = new Mock<IOrganizationService>();
            _organizationServicesMock.Setup(s => s.DeleteOrganization(It.IsAny<Guid>())).ReturnsAsync((Organization)null);
            //var controller = new OrganizationController(mockService.Object);

            // Act
            var result = await _controller.Deleteorg(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task Deleteorg_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            _organizationServicesMock.Setup(x => x.DeleteOrganization(id)).ThrowsAsync(new Exception("Test exception"));
            //var controller = new OrganizationsController(_servicesMock.Object);

            // Act
            var result = await _controller.Deleteorg(id);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }


    }

}
