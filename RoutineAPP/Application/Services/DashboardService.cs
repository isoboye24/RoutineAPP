using RoutineAPP.Application.Interfaces;
using RoutineAPP.Helper;

namespace RoutineAPP.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ITaskRepository _taskRepository;
        public DashboardService(ITaskRepository taskRepository) 
        { 
            _taskRepository = taskRepository;
        }

        public string GetCategoryTimeAnually(int year, string category)
        {
            int totalTimeSpent = _taskRepository.GetCategoryTimeAnually(year, category);

            return GeneralHelper.FormatTimeShort(totalTimeSpent);
        }

        public string GetCategoryTimeMonthly(int month, int year, string category)
        {
            int totalTimeSpent = _taskRepository.GetCategoryTimeMonthly(month, year, category);

            return GeneralHelper.FormatTimeShort(totalTimeSpent);
        }
    }
}
