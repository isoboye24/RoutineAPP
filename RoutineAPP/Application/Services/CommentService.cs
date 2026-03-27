using RoutineAPP.Application.Interfaces;
using RoutineAPP.Application.DTO;
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

        public List<DailyRoutineDTO> GetCommentById(int Id)
            => _dailyRoutineRepository.GetCommentById(Id);

        public List<DailyRoutineDTO> GetComments(int year)
            => _dailyRoutineRepository.GetComments(year);

        public int GetSummaryCount()
            => _dailyRoutineRepository.GetSummaryCount();

    }
}
