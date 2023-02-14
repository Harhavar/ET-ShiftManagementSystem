using AutoMapper;
using ET_ShiftManagementSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ET_ShiftManagementSystem.Servises;
using ET_ShiftManagementSystem.Data;
using System.Data;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Cors;
using ET_ShiftManagementSystem.Models.CommentModel;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("CorePolicy")]
    public class CommentController : Controller
    {
        private readonly ICommentServices commentServices;
        //private readonly IMapper mapper;

        public CommentController(ICommentServices commentServices)
        {
            this.commentServices = commentServices;
            //this.mapper = mapper;
        }

        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<ActionResult<Comment>> GetCommentDetails(int id)
        {
            var comment = await commentServices.GetCommentByID(id);

            if (comment == null)
            {
                return NotFound();
            }
            //var RegionDTO = new Models.CommentDTO();
            //comment.ToList().ForEach(comment =>
            //{
            var regionDTO = new Models.CommentModel.CommentDTO()
            {
                CommentID = comment.CommentID,
                CommentText = comment.CommentText,
                CreatedDate = comment.CreatedDate,
                Shared = comment.Shared,
                ShiftID = comment.ShiftID,
                UserID = comment.UserID,
                //Population = regions.Population

            };
            //Add(regionDTO);


            //});
            //var CommentDTO = mapper.Map<Models.CommentDTO>(commnet);

            return Ok(regionDTO);
        }

        [HttpGet]
        //[Route("allComment")]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments()
        {
            var comment = await commentServices.GetAllCommentsAsync();
            if (comment == null)
            {
                return NotFound();
            }
            //retur dto Comments
            var RegionDTO = new List<CommentDTO>();
            comment.ToList().ForEach(comment =>
            {
                var regionDTO = new Models.CommentModel.CommentDTO()
                {
                    CommentID = comment.CommentID,
                    CommentText = comment.CommentText,
                    CreatedDate = comment.CreatedDate,
                    Shared = comment.Shared,
                    ShiftID = comment.ShiftID,
                    UserID = comment.UserID,
                    //Population = regions.Population

                };
                RegionDTO.Add(regionDTO);


            });

            //var CommentDTO = mapper.Map<List<Models.CommentDTO>>(comment);

            return Ok(RegionDTO);
        }

        [HttpPost]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public IActionResult AddComment(Comment comment)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var com = new ET_ShiftManagementSystem.Entities.Comment()
                {
                    ShiftID = comment.ShiftID,
                    CommentText = comment.CommentText,
                    Shared = comment.Shared,
                    CreatedDate = DateTime.Now,

                };
                commentServices.AddComment(comment);

                return Ok(com);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            //var AddComment = new ShiftMgtDbContext.Entities.Comment()
            //{
            //    CommentText = comment.CommentText,
            //    Shared = comment.Shared,
            //    ShiftID = comment.ShiftID,
            //    UserID = comment.UserID
            //};

            //comment = await (ET_ShiftManagementSystem.Models.AddComments)commentServices.AddComment(AddComment);

            //var CommentDTO = new Models.Comment()
            //{
            //    CommentText = comment.CommentText,
            //    ShiftID = comment.ShiftID,
            //    CreatedDate = DateTime.Now,
            //    Shared = comment.Shared,
            //    UserID = comment.UserID
            //};

            //return Ok(CommentDTO);
        }

        [HttpDelete]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var delete = await commentServices.DeleteCommentAsync(id);

            if (delete == null)
            {
                return NotFound();
            }

            var DeleteDTO = new Models.CommentModel.CommentDTO()
            {

                CommentText = delete.CommentText,
                CreatedDate = DateTime.Now,
                ShiftID = delete.ShiftID,
                CommentID = delete.CommentID,
                Shared = delete.Shared,
                UserID = delete.UserID,


            };

            return Ok(DeleteDTO);
        }

        [HttpPut]
        //[Authorize(Roles = "SuperAdmin,Admin,User")]
        public async Task<IActionResult> UpdateComment(int id, UpdateCommentRequest comment)
        {
            try
            {
                var com = new ET_ShiftManagementSystem.Entities.Comment()
                {
                    CommentID = comment.CommentID,
                    CommentText = comment.CommentText,
                    Shared = comment.Shared,
                    CreatedDate = DateTime.Now,
                };

                await commentServices.UpdateCommentAsync(id, com);

                return Ok(com);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
