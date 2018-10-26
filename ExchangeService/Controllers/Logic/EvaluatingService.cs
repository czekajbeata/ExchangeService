using ExchangeService.Controllers.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Controllers.Logic
{
    public class EvaluatingService
    {
        private readonly IUserProfiles userProfiles;
        private readonly IUnitOfWork unitOfWork;

        public EvaluatingService(IUserProfiles userProfiles, IUnitOfWork unitOfWork)
        {
            this.userProfiles = userProfiles;
            this.unitOfWork = unitOfWork;
        }
        
        public bool AddComment(CommentDto comment, int leavingUserId, int receivingUserId)
        {
            Comment newComment = new Comment()
            {
                ReceivingUserId = receivingUserId,
                LeavingUserId = leavingUserId,
                CommentDate = DateTime.Today,
                Mark = comment.Mark,
                Text = comment.Text
            };
            userProfiles.AddComment(newComment);
            unitOfWork.CompleteWork();
            return newComment.CommentId != 0;
        }

        public IEnumerable<CommentDto> GetComments(int userId)
        {
            var userComments = userProfiles.GetAllComments(userId);
            List<CommentDto> commentDtos = new List<CommentDto>();
            foreach (var comment in userComments)
            {
                commentDtos.Add(new CommentDto()
                {
                    CommentDate = comment.CommentDate,
                    Mark = comment.Mark,
                    Text = comment.Text
                });
            }
            return commentDtos;
        }

        public double GetAvgMark(int userId)
        {
            return userProfiles.GetAvgMark(userId);
        }
    }
}
