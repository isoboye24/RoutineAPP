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
        private readonly IDailyRoutineRepository _dailyRoutineRepository;
        public CommentService(IDailyRoutineRepository dailyRoutineRepository)
        {
            _dailyRoutineRepository = dailyRoutineRepository;
        }

        public List<DailyRoutineViewModel> GetCommentById(int Id)
            => _dailyRoutineRepository.GetCommentById(Id);

        public List<DailyRoutineViewModel> GetComments(int year)
            => _dailyRoutineRepository.GetComments(year);

        public int GetSummaryCount()
            => _dailyRoutineRepository.GetSummaryCount();

    }
}
