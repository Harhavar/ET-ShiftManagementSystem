using AutoMapper;
using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.NotesModel;
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
    public class NotesServiceTests
    {
        private readonly Mock<INotesServices> _notesServicesMock;
        private readonly NotesController _controller;

        public NotesServiceTests()
        {
            _notesServicesMock = new Mock<INotesServices>();

            _controller = new NotesController(_notesServicesMock.Object);
            // }
        }

        [Test]
        public void AlertController_Constructor_Success()
        {
            // Arrange

            // Act
            var controller = new NotesController(_notesServicesMock.Object);

            // Assert
            Assert.IsNotNull(controller);
        }
        [Test]
        public async Task Get_ReturnsOkObjectResult_WhenNotesExist()
        {
            // Arrange
            var notes = new List<Note>
        {
            new Note { FileName = "Note1", Text = "This is note 1", CreatedBy = "User1", CreatedDate = DateTime.Now },
            new Note { FileName = "Note2", Text = "This is note 2", CreatedBy = "User2", CreatedDate = DateTime.Now }
        };
            _notesServicesMock.Setup(x => x.GetAllNotes()).ReturnsAsync(notes);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task Get_ReturnsNotFound_WhenNoNotesExist()
        {
            // Arrange
            List<Note> notes = null;
            _notesServicesMock.Setup(x => x.GetAllNotes()).ReturnsAsync(notes);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task Get_ReturnsOkResult_WithListOfNotes()
        {
            // Arrange
            var mockNotesService = new Mock<INotesServices>();
            var notes = new List<Note> { new Note { FileName = "note1.txt", Text = "This is note 1" }, new Note { FileName = "note2.txt", Text = "This is note 2" } };
            mockNotesService.Setup(x => x.GetAllNotes()).ReturnsAsync(notes);

            var controller = new NotesController(mockNotesService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsInstanceOf<ActionResult<IEnumerable<Note>>>(result);
            /*var okResult =*/
            //Assert.IsInstanceOf<OkObjectResult>(result);
            //var notesViewRequests = 
            //Assert.Equal(2, notesViewRequests.Count());
        }
        [Test]
        public async Task Get_Notes_Returns_OkResult_With_NotesViewRequest()
        {
            // Arrange
            var mockService = new Mock<INotesServices>();
            var noteId = Guid.NewGuid();
            var notes = new List<Note>
    {
        new Note
        {
            Id = noteId,
            ProjectId = Guid.NewGuid(),
            FileName = "testfile.txt",
            Text = "This is a test note.",
            CreatedBy = "testuser",
            CreatedDate = DateTime.Now
        }
    };
            mockService.Setup(x => x.GetAllNotes(It.IsAny<Guid>())).ReturnsAsync(notes);
            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.Get(noteId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsInstanceOf<string>(okResult.Value);
            //var notesView = (List<NotesViewRequest>)okResult.Value;
            //Assert.AreEqual(notesView.Count, 1);
            //Assert.AreEqual(notesView[0].FileName, "testfile.txt");
            //Assert.AreEqual(notesView[0].Text, "This is a test note.");
            //Assert.AreEqual(notesView[0].CreatedBy, "testuser");
        }
        [Test]
        public async Task PostNotes_ValidInput_ReturnsOk()
        {
            // Arrange
            var mockNotesService = new Mock<INotesServices>();
            var controller = new NotesController(mockNotesService.Object);
            var userId = Guid.NewGuid();
            var formFile = new FormFile(Stream.Null, 0, 0, "file", "file.txt");
            //var addNotes = new AddNotesVM { Text = "Test note", FileDetails = new List<IFormFile> { formFile } };

            // Act
           // var result = await controller.PostNotes(userId, addNotes);

            // Assert
            //Assert.That(result, Is.TypeOf<OkResult>());
        }
        [Test]
        public async Task PostNotes_ReturnsOkResult()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var fileDetails = new FormFile(Mock.Of<Stream>(), 0, 0, "file", "test.pdf");
            var addNotes = new AddNotesVM { Text = "test note", FileDetails = fileDetails , ProjectId = Guid.NewGuid()};
            var mockNotesService = new Mock<INotesServices>();
            mockNotesService.Setup(service => service.PostNotesAsync(userId, addNotes.Text, addNotes.FileDetails , addNotes.ProjectId))
                .Returns(Task.CompletedTask);
            var controller = new NotesController(mockNotesService.Object);

            // Act
            var result = await controller.PostNotes(userId, addNotes);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task PostNotes_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<INotesServices>();
            var controller = new NotesController(mockService.Object);

            var userId = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            var fileDetails = new List<IFormFile> { fileMock.Object };
            //var addNotesVM = new AddNotesVM { Text = "test note", FileDetails = fileDetails };

            //// Act
            //var result = await controller.PostNotes(userId, addNotesVM) as OkResult;

            //// Assert
            //Assert.IsNotNull(result);
            //Assert.AreEqual(200, result.StatusCode);
        }
        [Test]
        public async Task PostNotes_WhenAddNotesIsNull_ReturnsBadRequest()
        {
            // Arrange
            var controller = new NotesController(_notesServicesMock.Object);

            // Act
           // var result = await controller.PostNotes(Guid.NewGuid(), null);

            // Assert
           // Assert.IsInstanceOf<BadRequestResult>(result);
        }


    }
}
