﻿using AutoMapper;
using ET_ShiftManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftManagementServises.Servises;
using ShiftMgtDbContext.Data;
using ShiftMgtDbContext.Entities;
using System.Data;
using System.Drawing.Drawing2D;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentServices commentServices;
        //private readonly IMapper mapper;

        public CommentController(ICommentServices commentServices )
        {
            this.commentServices = commentServices;
            //this.mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
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
                var regionDTO = new Models.CommentDTO()
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
        [Route("/allComment")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments()
        {
            var comment = await commentServices.GetAllCommentsAsync();

            //retur dto regions
            var RegionDTO = new List<Models.CommentDTO>();
            comment.ToList().ForEach(comment =>
            {
                var regionDTO = new Models.CommentDTO()
                {
                    CommentID = comment.CommentID,
                    CommentText = comment.CommentText,
                    CreatedDate = comment.CreatedDate,
                    Shared = comment.Shared,
                    ShiftID  = comment.ShiftID,
                    UserID = comment.UserID,
                    //Population = regions.Population

                };
                RegionDTO.Add(regionDTO);


            });

            //var CommentDTO = mapper.Map<List<Models.CommentDTO>>(comment);

            return Ok(RegionDTO);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult AddComment(ShiftMgtDbContext.Entities.Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var com = new ShiftMgtDbContext.Entities.Comment()
            {
                ShiftID= comment.ShiftID,
                CommentText = comment.CommentText,
                Shared = comment.Shared,
                CreatedDate = DateTime.Now,

            };
            commentServices.AddComment(comment);

            return Ok(com);

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
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var delete = await commentServices.DeleteCommentAsync(id);

            if (delete == null)
            {
                return NotFound();
            }

            var DeleteDTO = new Models.CommentDTO()
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
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateComment(int id ,Models.UpdateCommentRequest comment)
        {
            var com = new ShiftMgtDbContext.Entities.Comment()
            {
                CommentID= comment.CommentID,
                CommentText = comment.CommentText,
                Shared = comment.Shared,
                CreatedDate= DateTime.Now,
            };

            await commentServices.UpdateCommentAsync(id, com);

            return Ok(com);
        }
    }
}
