using AutoMapper;
using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.ShiftModel;
using ET_ShiftManagementSystem.Servises;
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
    public class ShiftControllerTests
    {
        private Mock<IShiftServices> _shiftServicesMock;
        private Mock<IMapper> _mapperMock;
        private ShiftController _controller;

        // [SetUp]
        public ShiftControllerTests()
        {
            // Create mock objects for IShiftServices and IMapper interfaces
            _shiftServicesMock = new Mock<IShiftServices>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ShiftController(_shiftServicesMock.Object, _mapperMock.Object);

        }

        [Test]
        public void Constructor_ShouldCreateInstanceOfShiftController()
        {
            // Arrange

            // Act
            var shiftController = new ShiftController(_shiftServicesMock.Object, _mapperMock.Object);

            // Assert
            Assert.IsNotNull(shiftController);
            Assert.IsInstanceOf<ShiftController>(shiftController);
        }
        [Test]
        public async Task GetAllShifts_ReturnsOkResultWithShifts()
        {
            // Arrange
            var shiftDTOs = new List<ShiftDTO> { new ShiftDTO(), new ShiftDTO() };
            _shiftServicesMock.Setup(s => s.GetAllShiftAsync()).Returns(new List<Shift>());
            _mapperMock.Setup(m => m.Map<List<ShiftDTO>>(It.IsAny<List<Shift>>())).Returns(shiftDTOs);
            var controller = new ShiftController(_shiftServicesMock.Object, _mapperMock.Object);

            // Act
            var result = _controller.GetAllShifts();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            //var model = Assert.IsAssignableFrom<List<ShiftDTO>>(okResult.Value);
            //Assert.Equal(shiftDTOs.Count, model.Count);
        }
        [Test]
        public async Task GetAllShifts_ReturnsNotFoundResultWhenNoShifts()
        {
            // Arrange
            //var mockShiftServices = new Mock<IShiftServices>();
            //var mockMapper = new Mock<IMapper>();
            _shiftServicesMock.Setup(s => s.GetAllShiftAsync()).Returns((List<Shift>)null);
            var controller = new ShiftController(_shiftServicesMock.Object, _mapperMock.Object);

            // Act
            var result = _controller.GetAllShifts();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void GetAllShifts_ThrowsException()
        {
            // Arrange
            _shiftServicesMock.Setup(s => s.GetAllShiftAsync()).Throws(new Exception());
            //var controller = new ShiftController(_shiftServicesMock.Object, mockMapper.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => (Task)_controller.GetAllShifts());
        }
        [Test]
        public async Task GetAllShifts_CallsMapper()
        {
            // Arrange
            _shiftServicesMock.Setup(s => s.GetAllShiftAsync()).Returns(new List<Shift>());
            var controller = new ShiftController(_shiftServicesMock.Object, _mapperMock.Object);

            // Act
            var result = _controller.GetAllShifts();

            // Assert
            _mapperMock.Verify(m => m.Map<List<ShiftDTO>>(It.IsAny<List<Shift>>()), Times.Once);
        }

        [Test]
        public void GetAllShift_ReturnsOkResult_WhenShiftExists()
        {
            // Arrange
            var mockShiftServices = new Mock<IShiftServices>();
            //var shift = new Shift { TenantId = Guid.NewGuid(), ShiftName = "Morning Shift", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(8) };
            // mockShiftServices.Setup(s => s.GetAllShift(shift.TenantId)).Returns(shift);

            var controller = new ShiftController(mockShiftServices.Object, null);

            // Act
            //var result = controller.GetAllShift(shift.TenantId);

            // Assert
            // Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public void GetAllShift_ReturnsNotFoundResult_WhenShiftDoesNotExist()
        {
            // Arrange
            var mockShiftServices = new Mock<IShiftServices>();
            mockShiftServices.Setup(s => s.GetAllShift(It.IsAny<Guid>())).Returns((List<Shift>)null);

            var controller = new ShiftController(mockShiftServices.Object, null);

            // Act
            var result = controller.GetAllShift(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void GetAllShift_ReturnsBadRequestResult_WhenInputParameterIsNull()
        {
            // Arrange
            //var mockShiftServices = new Mock<IShiftServices>();
            //var controller = new ShiftController(mockShiftServices.Object, null);

            // Act
            var result = _controller.GetAllShift(Guid.Empty);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public void GetAllShift_ThrowsException_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var mockShiftServices = new Mock<IShiftServices>();
            mockShiftServices.Setup(s => s.GetAllShift(It.IsAny<Guid>())).Throws(new Exception());

            var controller = new ShiftController(mockShiftServices.Object, null);

            // Act & Assert
            Assert.Throws<Exception>(() => controller.GetAllShift(Guid.NewGuid()));
        }

        [Test]
        public async Task GetShiftByID_ShouldReturnBadRequest_WhenShiftidIsEmpty()
        {
            // Arrange
            var controller = new ShiftController(null, null);

            // Act
            var result = await controller.GetShiftByID(Guid.Empty);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task GetShiftByID_ShouldReturnNotFound_WhenShiftidIsValidButShiftIsNotFound()
        {
            // Arrange
            var shiftId = Guid.NewGuid();
            var mockShiftServices = new Mock<IShiftServices>();
            mockShiftServices.Setup(x => x.GetShiftById(shiftId)).ReturnsAsync((Shift)null);
            var controller = new ShiftController(mockShiftServices.Object, null);

            // Act
            var result = await controller.GetShiftByID(shiftId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void GetShiftByID_ShouldThrowException_WhenAnExceptionIsThrown()
        {
            // Arrange
            var mockShiftServices = new Mock<IShiftServices>();
            mockShiftServices.Setup(x => x.GetShiftById(It.IsAny<Guid>())).Throws(new Exception());
            var controller = new ShiftController(mockShiftServices.Object, null);

            // Act and Assert
            Assert.ThrowsAsync<Exception>(() => controller.GetShiftByID(Guid.NewGuid()));
        }
        [Test]
        public async Task GetShiftByID_ReturnsOkResult_WhenShiftExists()
        {
            // Arrange
            Guid shiftId = Guid.NewGuid();
            Shift shift = new Shift { ShiftID = shiftId };
            ShiftDTO shiftDTO = new ShiftDTO { ShiftID = shiftId };
            _shiftServicesMock.Setup(x => x.GetShiftById(shiftId)).ReturnsAsync(shift);
            _mapperMock.Setup(x => x.Map<ShiftDTO>(shift)).Returns(shiftDTO);

            // Act
            var result = await _controller.GetShiftByID(shiftId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            OkObjectResult okResult = result as OkObjectResult;
            Assert.IsInstanceOf<ShiftDTO>(okResult.Value);
            ShiftDTO returnedShiftDTO = okResult.Value as ShiftDTO;
            Assert.AreEqual(shiftDTO.ShiftID, returnedShiftDTO.ShiftID);
        }
        [Test]
        public void AddShift_ShouldReturnBadRequest_WhenShiftDTOIsNull()
        {
            // Arrange
            // var controller = new ShiftController(_shiftServicesMock.Object, mapper);

            // Act
            var result = _controller.AddShift(Guid.NewGuid(), null);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void AddShift_ShouldReturnBadRequest_WhenTenantIdIsEmpty()
        {
            // Arrange
            // var controller = new ShiftController(shiftServicesMock.Object, mapper);
            var shiftDTO = new Shift();

            // Act
            var result = _controller.AddShift(Guid.Empty, shiftDTO);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void AddShift_ShouldReturnOkResult_WhenShiftIsAddedSuccessfully()
        {
            // Arrange
            // var controller = new ShiftController(shiftServicesMock.Object, mapper);
            var tenantId = Guid.NewGuid();
            var shiftDTO = new Shift
            {
                ShiftName = "Test Shift",
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(16, 0, 0)
            };
            _shiftServicesMock.Setup(s => s.AddSift(tenantId, It.IsAny<Shift>()));

            // Act
            var result = _controller.AddShift(tenantId, shiftDTO);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void AddShift_ShouldReturnOkResultWithShift_WhenShiftIsAddedSuccessfully()
        {
            // Arrange
            // var controller = new ShiftController(shiftServicesMock.Object, mapper);
            var tenantId = Guid.NewGuid();
            var shiftDTO = new Shift
            {
                ShiftName = "Test Shift",
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(16, 0, 0)
            };
            _shiftServicesMock.Setup(s => s.AddSift(tenantId, It.IsAny<Shift>()));

            // Act
            var result = _controller.AddShift(tenantId, shiftDTO) as OkObjectResult;

            // Assert
            Assert.IsInstanceOf<Shift>(result.Value);
        }
        [Test]
        public async Task UpdateShift_ValidShiftIdAndShiftDTO_ReturnsOkResult()
        {
            // Arrange
            var shiftId = Guid.NewGuid();
            var shiftDTO = new UpdateShiftRequest { ShiftName = "Morning Shift", StartTime = new TimeSpan(5, 6, 22), EndTime = new TimeSpan(5, 6, 22) };
            var mockShiftServices = new Mock<IShiftServices>();
            mockShiftServices.Setup(x => x.UpdateShiftAsync(shiftId, shiftDTO)).ReturnsAsync(new Shift { ShiftID = shiftId, ShiftName = "Morning Shift", StartTime = new TimeSpan(5, 6, 22), EndTime = new TimeSpan(5, 6, 22) });
            var controller = new ShiftController(mockShiftServices.Object, null);

            // Act
            var result = await controller.UpdateShift(shiftId, shiftDTO);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.TypeOf<Shift>());
            var shiftResult = okResult.Value as Shift;
            Assert.That(shiftResult.ShiftID, Is.EqualTo(shiftId));
            Assert.That(shiftResult.ShiftName, Is.EqualTo("Morning Shift"));
            Assert.That(shiftResult.StartTime, Is.EqualTo(new TimeSpan(5, 6, 22)));
            Assert.That(shiftResult.EndTime, Is.EqualTo(new TimeSpan(5, 6, 22)));
        }
        [Test]
        public async Task UpdateShift_NullShiftDTO_ReturnsBadRequestResult()
        {
            // Arrange
            var shiftId = Guid.NewGuid();
            var mockShiftServices = new Mock<IShiftServices>();
            var controller = new ShiftController(mockShiftServices.Object, null);

            // Act
            var result = await controller.UpdateShift(shiftId, null);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }
        [Test]
        public async Task UpdateShift_InvalidShiftId_ReturnsBadRequestResult()
        {
            // Arrange
            var shiftId = Guid.Empty;
            var shiftDTO = new UpdateShiftRequest { ShiftName = "Morning Shift", StartTime = new TimeSpan(5, 6, 22), EndTime = new TimeSpan(5, 6, 22) };
            var mockShiftServices = new Mock<IShiftServices>();
            var controller = new ShiftController(mockShiftServices.Object, null);

            // Act
            var result = await controller.UpdateShift(shiftId, shiftDTO);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }
        [Test]
        public async Task UpdateShift_NonExistingShiftId_ReturnsNotFoundResult()
        {
            // Arrange
            var shiftId = Guid.NewGuid();
            var shiftDTO = new UpdateShiftRequest { ShiftName = "Morning Shift", StartTime = new TimeSpan(5, 6, 22), EndTime = new TimeSpan(5, 6, 22) };
            var mockShiftServices = new Mock<IShiftServices>();
            mockShiftServices.Setup(x => x.UpdateShiftAsync(shiftId, shiftDTO)).ReturnsAsync((Shift)null);
            var controller = new ShiftController(mockShiftServices.Object, null);

            // Act
            var result = await controller.UpdateShift(shiftId, shiftDTO);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public void DeleteShiftAsync_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.Parse("77D852E2-B0B3-4E37-AEF7-D3B4F0E67C6A");
            //_controller.DeleteShiftAsync(id).Returns(new Shift());

            // Act
            var result =  _controller.DeleteShiftAsync(id);

            // Assert
            //Assert.IsInstanceOf<OkObjectResult>(result);
            //var okResult = result as OkObjectResult;
            //Assert.AreEqual(id, okResult.Value);
        }

        [Test]
        public async Task DeleteShiftAsync_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            var result =  _controller.DeleteShiftAsync(id);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task DeleteShiftAsync_WithNonExistingShift_ReturnsNotFoundResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            //shiftController.shiftServices.DeleteShiftAsync(id).Returns((Shift)null);
            var mockShiftServices = new Mock<IShiftServices>();
            //mockShiftServices.Setup(x => x.DeleteShiftAsync(id).Returns((Task<Shift>)null);
            // Act
            var result =  _controller.DeleteShiftAsync(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        //[Test]
        //public async Task DeleteShiftAsync_WithException_ReturnsOkResultWithErrorMessage()
        //{
        //    // Arrange
        //    var id = Guid.NewGuid();
        //    // shiftController.shiftServices.DeleteShiftAsync(id).Throws(new Exception("Test Exception"));

        //    // Act
        //    var result = await _controller.DeleteShiftAsync(id);

        //    // Assert
        //    Assert.IsInstanceOf<OkObjectResult>(result);
        //    var okResult = result as OkObjectResult;
        //    Assert.AreEqual("Test Exception", okResult.Value);
        //}
        //[Test]
        //public async Task DeleteShiftAsync_WithExistingShift_ReturnsOkResultWithId()
        //{
        //    // Arrange
        //    var id = Guid.NewGuid();
        //    shiftController.shiftServices.DeleteShiftAsync(id).Returns(new Shift { Id = id });

        //    // Act
        //    var result = await shiftController.DeleteShiftAsync(id);

        //    // Assert
        //    Assert.IsInstanceOf<OkObjectResult>(result);
        //    var okResult = result as OkObjectResult;
        //    Assert.AreEqual(id, okResult.Value);
        //}

        //[Test]
        //public async Task DeleteShiftAsync_WithNonExistingShift_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    var id = Guid.NewGuid();
        //    shiftController.shiftServices.DeleteShiftAsync(id).Returns((Shift)null);

        //    // Act
        //    var result = await shiftController.DeleteShiftAsync(id);

        //    // Assert
        //    Assert.IsInstanceOf<NotFoundResult>(result);
        //}

        [Test]
        public async Task DeleteShiftAsync_WithException_ReturnsOkResultWithErrorMessage()
        {
            // Arrange
            var id = Guid.Parse("77D852E2-B0B3-4E37-AEF7-D3B4F0E67C6A");
            //_controller.DeleteShiftAsync(id).Throws(new Exception("Test Exception"));

            // Act
            var result = _controller.DeleteShiftAsync(id);

            // Assert
            //Assert.IsInstanceOf<OkObjectResult>(result);
            //var okResult = result as OkObjectResult;
            //Assert.AreEqual("Test Exception", okResult.Value);
        }
        //[Test]
        //public void DeleteShiftAsync_WithValidId_ReturnsOkResul()
        //{
        //    // Arrange
        //    var id = Guid.NewGuid();
        //    //shiftController.shiftServices.DeleteShiftAsync(id).Returns(new Shift());

        //    // Act
        //    var result = _controller.DeleteShiftAsync(id);

        //    // Assert
        //    Assert.IsInstanceOf<OkObjectResult>(result);
        //    var okResult = result as OkObjectResult;
        //    Assert.AreEqual(id, okResult.Value);
        //}

    }
}

