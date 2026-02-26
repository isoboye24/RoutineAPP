using RoutineAPP.Core.Interfaces;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentService _commentService;
        public CommentService(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public List<DailyRoutineViewModel> GetCommentById(int Id)
            => _commentService.GetCommentById(Id);

        public List<DailyRoutineViewModel> GetComments()
            => _commentService.GetComments();

        public int GetSummaryCount()
            => _commentService.GetSummaryCount();

    }
}
