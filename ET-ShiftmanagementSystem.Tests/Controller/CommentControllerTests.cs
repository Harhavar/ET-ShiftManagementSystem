using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using FluentAssertions;
using Xunit;
using ET_ShiftManagementSystem.Servises;
using ET_ShiftManagementSystem.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ET_ShiftManagementSystem.Models;
using ET_ShiftManagementSystem.Entities;

namespace ET_ShiftmanagementSystem.Tests.Controller
{
    public class CommentControllerTests
    {
        private readonly IFixture _fixture;
        //private readonly IMapper mapper;
        private readonly Mock<ICommentServices> _service;
        private readonly CommentController _commentController;

        public CommentControllerTests()
        {
            _fixture= new Fixture();
            _service =  _fixture.Freeze<Mock<ICommentServices>>();
            _commentController = new CommentController(_service.Object  ); //creates implementation in memeory
                
        }


        [Fact]
        public async Task GetComments_ShouldReturnOkResponce_WhenData_isFound()
        {
            //Arrange 
            var CommentMock = _fixture.Create<IEnumerable<Comment>>();
            _service.Setup(p => p.GetAllCommentsAsync()).ReturnsAsync(CommentMock);

            //Act
            var Result = await _commentController.GetAllComments().ConfigureAwait(false);

            //Assert
            //Assert.NotNull(Result);
            Result.Should().NotBeNull();
            Result.Should().BeAssignableTo<ActionResult<IEnumerable<CommentDTO>>>();
            Result.Result.Should().BeAssignableTo<OkObjectResult>();
            Result.Result.As<OkObjectResult>().Should().NotBeNull().And.BeOfType(CommentMock.GetType());
            _service.Verify(p => p.GetAllCommentsAsync(), Times.Once());


        }

        [Fact]

        public async Task GetComments_ShouldReturnOkResponce_WhenData_isNotFound()
        {
            //arrange
            List<Comment> responce = null;
            _service.Setup(x => x.GetAllCommentsAsync()).ReturnsAsync(responce);

            //act 
            var Result = await _commentController.GetAllComments().ConfigureAwait(false);

            //assert

            Result.Should().NotBeNull();
            Result.Should().BeAssignableTo<NotFoundResult>();
            _service.Verify(x => x.GetAllCommentsAsync(), Times.Once());
        }

        [Fact]

        public async Task Get_CommentsBy_Id_ShouldReturnOKresponce_WhenValidInput()
        {
            //arrange
            var CommentMock = _fixture.Create<Comment>();
            var id = _fixture.Create<int>();
            _service.Setup(x => x.GetCommentByID(id)).ReturnsAsync(CommentMock);

            //Act
            var Result = await _commentController.GetCommentDetails(id).ConfigureAwait(false);

            //Assert
            //Assert.NotNull(Result);
            Result.Should().NotBeNull();
            Result.Should().BeAssignableTo<ActionResult<CommentDTO>>();
            Result.Should().BeAssignableTo<OkObjectResult>();
            Result.As<OkObjectResult>().Should().NotBeNull().And.BeOfType(CommentMock.GetType());
            _service.Verify(p => p.GetCommentByID(id), Times.Once());

        }

        [Fact]

        public async Task Get_CommentsBy_Id_ShouldReturnOKresponce_When_InValidInput()
        {
            Comment responce = null;
            var id = _fixture.Create<int>();
            _service.Setup(x => x.GetCommentByID(id)).ReturnsAsync(responce);

            //act 
            var Result = await _commentController.GetCommentDetails(id).ConfigureAwait(false);

            //assert

            Result.Should().NotBeNull();
            Result.Should().BeAssignableTo<NotFoundResult>();
            _service.Verify(x => x.GetCommentByID(id), Times.Once());

        }

        [Fact]

        public async Task Get_CommentsBy_Id_ShouldReturnOKresponce_When_Input_IsZERO()
        {
            Comment responce = null;
            var id = 0;
            _service.Setup(x => x.GetCommentByID(id)).ReturnsAsync(responce);

            //act 
            var Result = await _commentController.GetCommentDetails(id).ConfigureAwait(false);

            //assert

            Result.Should().NotBeNull();
            Result.Should().BeAssignableTo<NotFoundResult>();
            _service.Verify(x => x.GetCommentByID(id), Times.Once());

        }

        [Fact]
        public async Task CreateComment_ShouldReturnOKresponce_when_ValidRequest()
        {
            //Arange
            var request = _fixture.Create<Comment>();
            var responce = _fixture.Create<Comment>();
            //_service.Setup(x => x.AddComment(request)).Returns(responce);

            //act 
            //var result = _commentController.AddComment(request).Configure(false);

            //assert
           // result.Should().NotBeNull();

            //result.Should().AssignableTo<ActionResult<ShiftMgtDbContext.Entities.Comment>>();
        }


    }
}
