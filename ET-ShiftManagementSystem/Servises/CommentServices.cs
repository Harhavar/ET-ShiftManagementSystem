using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET_ShiftManagementSystem.Servises
{
    public interface ICommentServices
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> GetCommentByID(int commentid);

        long  AddComment(Comment comment);

        Task<Comment> DeleteCommentAsync(int id);

        Task<Comment> UpdateCommentAsync(int id, Comment comment);
    }

    public  class CommentServices : ICommentServices
    {
        private readonly ShiftManagementDbContext _commnet;

        public CommentServices(ShiftManagementDbContext comment)
        {
            _commnet= comment;
        }

        public  long AddComment(Comment comment)
        {
             _commnet.Comments.Add(comment);
             _commnet.SaveChanges();
            return comment.CommentID;
        }

        public async Task<Comment> DeleteCommentAsync(int id)
        {
            var comment = await _commnet.Comments.FirstOrDefaultAsync(r => r.CommentID == id);

            if (comment == null)
            {
                return null;
            }

            //delete the region
            _commnet.Comments.Remove(comment);
            await _commnet.SaveChangesAsync();


            return comment;

        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            return await _commnet.Comments.ToListAsync();
        }

        public async  Task<Comment> GetCommentByID(int commentid)
        {
            return await _commnet.Comments.FirstOrDefaultAsync(x => x.CommentID == commentid);
        }

        public async Task<Comment> UpdateCommentAsync(int Id, Comment comment)
        {
            var existingComment = await _commnet.Comments.FirstOrDefaultAsync(x => x.CommentID == Id);

            if (existingComment == null)
            {
                return null;

            }

            existingComment.CommentID = comment.CommentID;
            existingComment.CommentText = comment.CommentText;
            //existingComment.ShiftID= comment.ShiftID;
            existingComment.Shared  = comment.Shared;
            //existingComment.UserID= comment.UserID;
            existingComment.CreatedDate= comment.CreatedDate;

           
            await _commnet.SaveChangesAsync();

            return existingComment;
        }
    }
}
