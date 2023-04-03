using AutoMapper;
using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.AlertModel;
using ET_ShiftManagementSystem.Servises;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Moq;
using Alert = ET_ShiftManagementSystem.Entities.Alert;

namespace Et_shiftmsnsgementsystem
{
    public class AlertControllerTests
    {
        private readonly Mock<IAlertServices> _alertServicesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AlertController _controller;
        public AlertControllerTests()
        {
            _alertServicesMock = new Mock<IAlertServices>();
            //_mockAlertServices = new Mock<IAlertServices>();
            _mapperMock = new Mock<IMapper>();

            _controller = new AlertController(_alertServicesMock.Object, _mapperMock.Object);
            // }
        }

        [Test]
        public void AlertController_Constructor_Success()
        {
            // Arrange

            // Act
            var controller = new AlertController(_alertServicesMock.Object, _mapperMock.Object);

            // Assert
            Assert.IsNotNull(controller);
        }
        //public void Setup()
        //{

        //}

        //[Test]
        //public void GetAll_ReturnsAllAlerts()
        //{
        //    // Act
        //    var result = _controller.GetAllalert();

        //    // Assert
        //    Assert.IsInstanceOf<ActionResult<IEnumerable<ET_ShiftManagementSystem.Entities.Alert>>>(result);
        //    //Assert.AreEqual(3, result.Value.Count());
        //}
        [SetUp]

        [Test]
        public async Task GetAllAlert_ReturnsOk_WithAlertsDtoList()
        {
            // Arrange
            var alerts = new List<ET_ShiftManagementSystem.Entities.Alert>
            {
                // new Alert { Id =Guid.Parse("C48DA508-5A60-4EE5-92FF-9A4B0F35AAD8"), AlertName = "Alert 1", severity = AlertSeverity.High },
                //new Alert { Id = Guid.Parse("C48DA508-5A60-4EE5-92FF-9A4B0F35AAD8"), AlertName = "Alert 2", severity = AlertSeverity.Medium }
            };
            var expectedAlertsDto = new List<AlertsDTO>
            {
                //new AlertsDTO { Id = 1, AlertName = "Alert 1", severity = (int)AlertSeverity.High },
                //new AlertsDTO { Id = 2, AlertName = "Alert 2", severity = (int)AlertSeverity.Medium }
            };
            _alertServicesMock.Setup(x => x.GetAllAlert()).ReturnsAsync(alerts);
            _mapperMock.Setup(x => x.Map<IEnumerable<AlertsDTO>>(alerts)).Returns(expectedAlertsDto);

            // Act
            var result = await _controller.GetAllalert();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.IsInstanceOf<List<AlertsDTO>>(okResult.Value);
            var alertsDto = (List<AlertsDTO>)okResult.Value;
            Assert.AreEqual(alertsDto.Count, expectedAlertsDto.Count);
            for (int i = 0; i < alertsDto.Count; i++)
            {
                Assert.AreEqual(alertsDto[i].Id, expectedAlertsDto[i].Id);
                Assert.AreEqual(alertsDto[i].AlertName, expectedAlertsDto[i].AlertName);
                Assert.AreEqual(alertsDto[i].severity, expectedAlertsDto[i].severity);
            }
        }
        [Test]
        public async Task GetAllAlert_ReturnsBadRequest_WhenAlertsIsNull()
        {
            // Arrange
            _alertServicesMock.Setup(x => x.GetAllAlert()).ReturnsAsync((IEnumerable<Alert>)null);

            // Act
            var result = await _controller.GetAllalert();

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result.Result);
        }
        [Test]
        public async Task GetAllAlert_ReturnsOk_WithExceptionMessage_WhenExceptionIsThrown()
        {
            // Arrange
            var exceptionMessage = "An error occurred while retrieving alerts.";
            _alertServicesMock.Setup(x => x.GetAllAlert()).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetAllalert();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.AreEqual(exceptionMessage, okResult.Value);
        }
        [Test]
        public async Task GetAllAlertBySeverity_WithInvalidSeverity_ReturnsNotFoundResult()
        {
            // Arrange
            var severity = severityLevel.P3;
            _alertServicesMock.Setup(x => x.GetAllAlertBySeveority(severity)).Returns((List<Alert>)null);

            // Act
            var result = await _controller.GetAllAlertBySeveority(severity);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        //     [Test]
        //public void GetAllAlertBySeverity_WithException_ReturnsError()
        //{
        //    // Arrange
        //    var severity = severityLevel.P1;
        //    var errorMessage = "An error occurred while getting alerts.";
        //    _alertServicesMock.Setup(x => x.GetAllAlertBySeveority(severity)).Throws(new Exception(errorMessage));

        //    // Act
        //    var result = _controller.GetAllAlertBySeveority(severity).Result;

        //    // Assert
        //    Assert.That(result, Is.InstanceOf<OkObjectResult>());
        //    //var okResult = result as OkObjectResult;
        //    //Assert.AreEqual(errorMessage, okResult.Value);
        //}
        [Test]
        public void GetAlerts_ReturnsOkResult()
        {
            // Arrange
            var start = new DateTime(2022, 1, 1);
            var end = new DateTime(2022, 1, 31);
            var mockAlertService = new Mock<IAlertServices>();
            mockAlertService.Setup(service => service.GetAlertByFilter(start, end))
                            .Returns(new List<Alert>());

            var controller = new AlertController(mockAlertService.Object, null);

            // Act
            var result = controller.GetAlerts(start, end);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public void GetAlerts_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var start = new DateTime(2022, 1, 1);
            var end = new DateTime(2022, 1, 31);
            var mockAlertService = new Mock<IAlertServices>();
            mockAlertService.Setup(service => service.GetAlertByFilter(start, end))
                            .Throws(new Exception("An error occurred."));

            var controller = new AlertController(mockAlertService.Object, null);

            // Act
            var result = controller.GetAlerts(start, end);

            // Assert
            //var badRequestResult = Assert.IsInstanceOf<BadRequestObjectResult>(result);
            //Assert.AreEqual("An error occurred.", badRequestResult.Value);
        }
        [Test]
        public async Task GetAlert_ValidInput_ReturnsOkResult()
        {
            // Arrange
            string alertName = "test alert";
            DateTime from = new DateTime(2022, 1, 1);
            DateTime? to = new DateTime(2022, 1, 31);
            List<Alert> alerts = new List<Alert>() { new Alert() { AlertName = alertName, CreatedDate = from } };
            _alertServicesMock.Setup(s => s.GetAlertByFilter(alertName, from, to)).Returns(alerts);

            // Act
            var result = _controller.GetAlert(alertName, from, to);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<List<Alert>>(okResult.Value);
            var alertResult = okResult.Value as List<Alert>;
            Assert.AreEqual(alertName, alertResult[0].AlertName);
            Assert.AreEqual(from, alertResult[0].CreatedDate);
        }
        [Test]
        public async Task GetAlert_InvalidInput_ReturnsBadRequestResult()
        {
            // Arrange
            string alertName = "test alert";
            DateTime from = new DateTime(2022, 1, 1);
            DateTime? to = new DateTime(2021, 12, 31);
            _alertServicesMock.Setup(s => s.GetAlertByFilter(alertName, from, to)).Throws(new ArgumentException());

            // Act
            var result = _controller.GetAlert(alertName, from, to);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task GetTodaysAlert_ReturnsOk_WhenAlertsExist()
        {
            // Arrange
            var expectedAlerts = new List<Alert>()
            {
            new Alert() { Id = Guid.Parse("C48DA508-5A60-4EE5-92FF-9A4B0F35AAD8"), AlertName = "Test Alert 1", CreatedDate = DateTime.UtcNow.Date, severity = severityLevel.P3 },
            //new Alert() { Id = 2, AlertName = "Test Alert 2", CreatedDate = DateTime.UtcNow.Date, severity = SeverityLevel.High },
        };
            _alertServicesMock.Setup(x => x.GetTodaysAlert()).Returns(expectedAlerts);

            // Act
            var result = await _controller.GetTodaysAlert();

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var alertDtoList = okResult.Value as List<AlertsDTO>;
            Assert.IsNotNull(alertDtoList);
            Assert.AreEqual(expectedAlerts.Count, alertDtoList.Count);

            for (int i = 0; i < expectedAlerts.Count; i++)
            {
                var expectedAlert = expectedAlerts[i];
                var alertDto = alertDtoList[i];
                Assert.AreEqual(expectedAlert.Id, alertDto.Id);
                Assert.AreEqual(expectedAlert.AlertName, alertDto.AlertName);
                Assert.AreEqual(expectedAlert.CreatedDate, alertDto.CreatedDate);
                Assert.AreEqual((int)expectedAlert.severity, alertDto.severity);
            }
        }

        [Test]
        public async Task GetTodaysAlert_ReturnsBadRequest_WhenAlertsDoNotExist()
        {
            // Arrange
            _alertServicesMock.Setup(x => x.GetTodaysAlert()).Returns((List<Alert>)null);

            // Act
            var result = await _controller.GetTodaysAlert();

            // Assert
            Assert.IsNotNull(result);
            var badRequestResult = result.Result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }
        [Test]
        public async Task GetAllAlertBySeveority_ReturnsOkResultWithAlertDtoList()
        {
            // Arrange
            var expectedSeverity = severityLevel.P2;
            var alerts = new List<Alert>
        {
            new Alert
            {
                severity = severityLevel.P2,
                CreatedDate = DateTime.Now,
                AlertName = "Test Alert",
                RCA = "Test RCA",
                TriggeredTime = DateTime.Now,
                Description = "Test Description",
                ReportedBy = "Test User",
                ReportedTo = "Test Team",
                Id =Guid.Parse("C48DA508-5A60-4EE5-92FF-9A4B0F35AAD8"),
                lastModifiedDate = DateTime.Now,
                Status = "",
                TenantId = Guid.Parse("C48DA508-5A60-4EE5-92FF-9A4B0F35AAD8"),
                ProjectId = Guid.Parse("C48DA508-5A60-4EE5-92FF-9A4B0F35AAD8")
            }
        };
            _alertServicesMock.Setup(x => x.GetAllAlertBySeveority(expectedSeverity)).Returns(alerts);

            // Act
            var result = await _controller.GetAllAlertBySeveority(expectedSeverity);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsInstanceOf<List<AlertsDTO>>(okResult.Value);
            var alertDtos = okResult.Value as List<AlertsDTO>;
            Assert.AreEqual(alerts.Count, alertDtos.Count);
            var alertDto = alertDtos.First();
            var alert = alerts.First();
            Assert.AreEqual((int)alert.severity, alertDto.severity);
            Assert.AreEqual(alert.CreatedDate, alertDto.CreatedDate);
            Assert.AreEqual(alert.AlertName, alertDto.AlertName);
            Assert.AreEqual(alert.RCA, alertDto.RCA);
            Assert.AreEqual(alert.TriggeredTime, alertDto.TriggeredTime);
            Assert.AreEqual(alert.Description, alertDto.Description);
            Assert.AreEqual(alert.ReportedBy, alertDto.ReportedBy);
            Assert.AreEqual(alert.ReportedTo, alertDto.ReportedTo);
            Assert.AreEqual(alert.Id, alertDto.Id);
            Assert.AreEqual(alert.lastModifiedDate, alertDto.lastModifiedDate);
            Assert.AreEqual(alert.Status, alertDto.Status);
            Assert.AreEqual(alert.TenantId, alertDto.TenantId);
            Assert.AreEqual(alert.ProjectId, alertDto.ProjectId);

            _alertServicesMock.Verify(x => x.GetAllAlertBySeveority(expectedSeverity), Times.Once);
        }
        [Test]
        public async Task GetAllAlertBySeveority_ReturnsNotFoundResult_WhenNoAlertsFound()
        {
            // Arrange
            var expectedSeverity = severityLevel.P2;
            _alertServicesMock.Setup(x => x.GetAllAlertBySeveority(expectedSeverity)).Returns(() => null);

            // Act
            var result = await _controller.GetAllAlertBySeveority(expectedSeverity);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
            _alertServicesMock.Verify(x => x.GetAllAlertBySeveority(expectedSeverity), Times.Once);
        }
        [Test]
        public async Task GetAllAlertBySeveority_ReturnsBadRequestResult_WhenExceptionThrown()
        {
            // Arrange
            //var mockService = new Mock<IAlertService>();
            _alertServicesMock.Setup(s => s.GetAllAlertBySeveority(It.IsAny<severityLevel>())).Throws(new Exception("Test exception"));
            //var controller = new AlertController(mockService.Object);

            // Act
            var result = await _controller.GetAllAlertBySeveority(severityLevel.P1);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }
        [Test]
        public async Task addAlert_ReturnsOkResult_WhenAlertAddedSuccessfully()
        {
            // Arrange
           // var mockAlertService = new Mock<IAlertService>();
            var controller = new AlertController(_alertServicesMock.Object , _mapperMock.Object);
            var projectID = Guid.NewGuid();
            var alertRequest = new ET_ShiftManagementSystem.Models.AlertModel.AlertRequest { AlertName = "Test Alert", Description = "This is a test alert" };
            var severity = severityLevel.P1;
            var request = new AddAlertRequest {  alertRequest = alertRequest, severity = severity };

            // Act
            var result = await controller.addAlert(projectID, request);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            _alertServicesMock.Verify(x => x.AddAlert(projectID, alertRequest, severity), Times.Once);
        }
        [Test]
        public async Task AddAlert_ReturnsBadRequestResult_WhenAlertIsNull()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            // Act
            var result = await _controller.addAlert(projectId, null) as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
        [Test]
        public async Task AddAlert_ReturnsOkResult_WhenExceptionThrown()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var alertRequest = new ET_ShiftManagementSystem.Models.AlertModel.AlertRequest { /* fill with valid data */ };
            var severity = severityLevel.P1;

            _alertServicesMock.Setup(s => s.AddAlert(projectId, alertRequest, severity)).Throws(new Exception());

            // Act
            var result = await _controller.addAlert(projectId, new AddAlertRequest { alertRequest = alertRequest, severity = severity }) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Exception of type 'System.Exception' was thrown.", result.Value);
        }
        [Test]
        public async Task DeleteAlert_ValidId_ReturnsOkResult()
        {
            // Arrange
            var mockAlertServices = new Mock<IAlertServices>();
            var alertToDelete = new Alert
            {
                Id = Guid.NewGuid(),
                severity = severityLevel.P2,
                AlertName = "Test Alert",
                RCA = "Test RCA",
                Description = "Test Description",
                ReportedBy = "Test User",
                ReportedTo = "Test Team",
                TriggeredTime = DateTime.Now,
                lastModifiedDate = DateTime.Now,
                Status = "Closed",
                TenantId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid()
            };
            mockAlertServices.Setup(s => s.DeleteAlertAsync(It.IsAny<Guid>()))
                .ReturnsAsync(alertToDelete);

            var controller = new AlertController(mockAlertServices.Object , _mapperMock.Object);

            // Act
            var result = await controller.deleteAlert(alertToDelete.Id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var alertDTO = result.Value as AlertsDTO;
            Assert.IsNotNull(alertDTO);
            Assert.AreEqual(alertToDelete.Id, alertDTO.Id);
            Assert.AreEqual(alertToDelete.severity, (severityLevel)alertDTO.severity);
            Assert.AreEqual(alertToDelete.AlertName, alertDTO.AlertName);
            Assert.AreEqual(alertToDelete.RCA, alertDTO.RCA);
            Assert.AreEqual(alertToDelete.Description, alertDTO.Description);
            Assert.AreEqual(alertToDelete.ReportedBy, alertDTO.ReportedBy);
            Assert.AreEqual(alertToDelete.ReportedTo, alertDTO.ReportedTo);
            Assert.AreEqual(alertToDelete.TriggeredTime, alertDTO.TriggeredTime);
            Assert.AreEqual(alertToDelete.lastModifiedDate, alertDTO.lastModifiedDate);
            Assert.AreEqual(alertToDelete.Status, alertDTO.Status);
            Assert.AreEqual(alertToDelete.TenantId, alertDTO.TenantId);
            Assert.AreEqual(alertToDelete.ProjectId, alertDTO.ProjectId);
        }
        
    }
}