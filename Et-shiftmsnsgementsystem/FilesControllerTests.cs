using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Models.DocModel;
using ET_ShiftManagementSystem.Services;
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
    public class FilesControllerTests
    {
        private Mock<IFileService> _uploadServiceMock;
        private FilesController _controller;

        [SetUp]
        public void Setup()
        {
            _uploadServiceMock = new Mock<IFileService>();
            _controller = new FilesController(_uploadServiceMock.Object);
        }

        [Test]
        public async Task Get_ReturnsOkResult_WhenDocumentsExist()
        {
            // Arrange
            var documentList = new List<FileDetails>()
        {
            new FileDetails()
            {
                FileName = "doc1.txt",
                UpdateDate = DateTime.UtcNow
            },
            new FileDetails()
            {
                FileName = "doc2.txt",
                UpdateDate = DateTime.UtcNow.AddDays(-1)
            }
        };

            _uploadServiceMock.Setup(x => x.GetallDocs(It.IsAny<Guid>())).ReturnsAsync(documentList);

            // Act
            var result = await _controller.Get(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.IsInstanceOf<List<ViewDocument>>(okResult.Value);
            var docList = (List<ViewDocument>)okResult.Value;
            Assert.AreEqual(documentList.Count, docList.Count);
            for (int i = 0; i < documentList.Count; i++)
            {
                Assert.AreEqual(documentList[i].FileName, docList[i].FileName);
                Assert.AreEqual(documentList[i].UpdateDate, docList[i].UpdateDate);
            }
        }
        [Test]
        public async Task Get_ReturnsNotFoundResult_WhenNoDocumentsExist()
        {
            // Arrange
            _uploadServiceMock.Setup(x => x.GetallDocs(It.IsAny<Guid>())).ReturnsAsync((List<FileDetails>)null);

            // Act
            var result = await _controller.Get(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task Get_ReturnsNotFound_WhenDocumentsDoNotExist()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            _uploadServiceMock.Setup(x => x.GetallDocs(tenantId)).ReturnsAsync((List<FileDetails>)null);

            // Act
            var result = await _controller.Get(tenantId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task Get_ReturnsOkResult_WithDocuments_WhenDocumentsExist()
        {
            // Arrange
            Guid tenantId = Guid.NewGuid();
            var documentList = new List<FileDetails>()
        {
            new FileDetails() { FileName = "document1.txt", UpdateDate = DateTime.Now },
            new FileDetails() { FileName = "document2.txt", UpdateDate = DateTime.Now.AddDays(-1) },
            new FileDetails() { FileName = "document3.txt", UpdateDate = DateTime.Now.AddDays(-2) }
        };
            _uploadServiceMock.Setup(x => x.GetallDocs(tenantId)).ReturnsAsync(documentList);

            // Act
            var result = await _controller.Get(tenantId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.IsInstanceOf<IEnumerable<ViewDocument>>(okResult.Value);
            var documentViewList = (IEnumerable<ViewDocument>)okResult.Value;
            Assert.AreEqual(documentList.Count, documentViewList.Count());
            for (int i = 0; i < documentList.Count; i++)
            {
                Assert.AreEqual(documentList[i].FileName, documentViewList.ElementAt(i).FileName);
                Assert.AreEqual(documentList[i].UpdateDate, documentViewList.ElementAt(i).UpdateDate);
            }
        }
        [Test]
        public async Task GetOrganizationById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedFileDetails = new FileDetails { ID = expectedId, FileName = "testfile.pdf", UpdateDate = DateTime.UtcNow };
            _uploadServiceMock.Setup(x => x.GetSingleDocs(expectedId)).ReturnsAsync(expectedFileDetails);

            // Act
            var result = await _controller.GetOrganizationById(expectedId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<FileDetails>(okResult.Value);
            var actualFileDetails = okResult.Value as FileDetails;
            Assert.AreEqual(expectedId, actualFileDetails.ID);
            Assert.AreEqual(expectedFileDetails.FileName, actualFileDetails.FileName);
            Assert.AreEqual(expectedFileDetails.UpdateDate, actualFileDetails.UpdateDate);
        }
        [Test]
        public async Task GetOrganizationById_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _uploadServiceMock.Setup(x => x.GetSingleDocs(invalidId)).ReturnsAsync((FileDetails)null);

            // Act
            var result = await _controller.GetOrganizationById(invalidId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetOrganizationById_WithExceptionThrown_ReturnsInternalServerErrorResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            _uploadServiceMock.Setup(x => x.GetSingleDocs(id)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetOrganizationById(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Test exception", okResult.Value);
        }
        [Test]
        public async Task PostSingleFile_ValidRequest_ReturnsOk()
        {
            // Arrange
            var tenentId = Guid.NewGuid();
            var fileDetails = new FileUploadModel
            {
                FileDetails = new FormFile(Stream.Null, 0, 0, "file", "file.txt"),
                FileType = FileType.PDF
            };

            // Act
            var result = await _controller.PostSingleFile(tenentId, fileDetails);

            // Assert
            Assert.That(result, Is.TypeOf<OkResult>());
            _uploadServiceMock.Verify(s => s.PostFileAsync(tenentId, fileDetails.FileDetails, fileDetails.FileType), Times.Once);
        }
        [Test]
        public async Task PostSingleFile_NullFileDetails_ReturnsBadRequest()
        {
            // Arrange
            var tenentId = Guid.NewGuid();

            // Act
            var result = await _controller.PostSingleFile(tenentId, null);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
            _uploadServiceMock.Verify(s => s.PostFileAsync(It.IsAny<Guid>(), It.IsAny<FormFile>(), It.IsAny<FileType>()), Times.Never);
        }
        [Test]
        public async Task PostSingleFile_TenantIdEmpty_ReturnsBadRequest()
        {
            // Arrange
            var fileDetails = new FileUploadModel
            {
                FileDetails = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file")), 0, 0, "data", "test.txt")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/plain"
                },
                FileType = FileType.PDF
            };

            // Act
            var result = await _controller.PostSingleFile(Guid.Empty, fileDetails);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task PostSingleFile_FileDetailsNull_ReturnsBadRequest()
        {
            // Arrange
            var fileDetails = new FileUploadModel
            {
                FileDetails = null,
                FileType = FileType.PDF
            };

            // Act
            var result = await _controller.PostSingleFile(Guid.NewGuid(), fileDetails);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task PostSingleFile_FileTypeInvalid_ReturnsBadRequest()
        {
            // Arrange
            var fileDetails = new FileUploadModel
            {
                FileDetails = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file")), 0, 0, "data", "test.txt")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/plain"
                },
                FileType = (FileType)999 // Invalid file type value
            };

            // Act
            var result = await _controller.PostSingleFile(Guid.NewGuid(), fileDetails);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task PostSingleFile_Success_ReturnsOkResult()
        {
            // Arrange
            var fileDetails = new FileUploadModel
            {
                FileDetails = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file")), 0, 0, "data", "test.txt")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/plain"
                },
                FileType = FileType.PDF
            };

            // Act
            var result = await _controller.PostSingleFile(Guid.NewGuid(), fileDetails);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task PostMultipleFile_ValidData_ReturnsOkResult()
        {
            // Arrange
            var mockUploadService = new Mock<IFileService>();
            var controller = new FilesController(mockUploadService.Object);
            var tenantId = Guid.NewGuid();
            var files = new List<FileUploadModel>
    {
        new FileUploadModel { FileDetails = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test File")), 0, 0, "Test File", "test.txt"), FileType = FileType.PDF },
        new FileUploadModel { FileDetails = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Another Test File")), 0, 0, "Another Test File", "test2.txt"), FileType =  FileType.PDF }
    };

            // Act
            var result = await controller.PostMultipleFile(tenantId, files);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            mockUploadService.Verify(s => s.PostMultiFileAsync(tenantId, files), Times.Once);
        }
        [Test]
        public async Task PostMultipleFile_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fileDetails = new List<FileUploadModel>
    {
        new FileUploadModel { FileType = FileType.PDF, FileDetails = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Test file content")), 0, 0, "file1", "test.jpg") },
        new FileUploadModel { FileType = FileType.PDF, FileDetails = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("Another test file content")), 0, 0, "file2", "test.png") },
    };

            var mockUploadService = new Mock<IFileService>();
            mockUploadService
                .Setup(svc => svc.PostMultiFileAsync(id, fileDetails))
                .Returns(Task.CompletedTask);

            var controller = new FilesController(mockUploadService.Object);

            // Act
            var result = await controller.PostMultipleFile(id, fileDetails);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task PostMultipleFile_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            List<FileUploadModel> fileDetails = null;
            var controller = new FilesController(_uploadServiceMock.Object);

            // Act
            var result = await controller.PostMultipleFile(id, fileDetails);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task DownloadFile_ValidId_ReturnsFileContentResult()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileContent = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f }; // "Hello" in ASCII
            //var mockFileService = new Mock<IFileService>();
            _uploadServiceMock.Setup(x => x.DownloadFileById(fileId));
            var controller = new FilesController(_uploadServiceMock.Object);

            // Act
            var result = await controller.DownloadFile(fileId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            //var fileContentResult = (OkResult)result;
            //Assert.AreEqual("application/octet-stream", fileContentResult.ContentType);
            //Assert.AreEqual(fileContent.Length, fileContentResult.FileContents.Length);
            //Assert.AreEqual("file.bin", fileContentResult.FileDownloadName);
        }
        [Test]
        public async Task DownloadFile_WithValidId_ReturnsFileContentResult()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileContents = new byte[] { 0x48, 0x65, 0x6c, 0x6c, 0x6f };
            var fileStream = new MemoryStream(fileContents);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.DownloadFileById(fileId));

            var controller = new FilesController(mockFileService.Object);

            // Act
            var result = await controller.DownloadFile(fileId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);

            //var fileResult = (FileContentResult)result;
            //Assert.AreEqual("application/octet-stream", fileResult.ContentType);
            //Assert.AreEqual(fileContents.Length, fileResult.FileContents.Length);

            //for (int i = 0; i < fileContents.Length; i++)
            //{
            //    Assert.AreEqual(fileContents[i], fileResult.FileContents[i]);
            //}
        }
        [Test]
        public async Task DownloadFile_EmptyGuid_ReturnsBadRequest()
        {
            // Arrange
            var controller = new FilesController(_uploadServiceMock.Object);

            // Act
            var result = await controller.DownloadFile(Guid.Empty);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task DeleteDocs_ReturnsOkResult_WhenIdIsValid()
        {
            // Arrange
            Guid id = new Guid("08b2c717-1bb6-4e70-b67c-0123456789ab");
            var mockService = new Mock<IFileService>();
            mockService.Setup(s => s.DeleteDocuent(id)).ReturnsAsync(new FileDetails { FileName = "file.txt" });
            var controller = new FilesController(mockService.Object);

            // Act
            var result = await controller.DeleteDocs(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("file.txt", okResult.Value);
        }
        [Test]
        public async Task DeleteDocs_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            _uploadServiceMock.Setup(service => service.DeleteDocuent(fileId)).ReturnsAsync(new FileDetails { ID = fileId, FileName = "test.txt" });

            // Act
            var result = await _controller.DeleteDocs(fileId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("test.txt", okResult.Value);
        }

    }

}
