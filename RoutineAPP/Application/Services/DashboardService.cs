using RoutineAPP.Core.Interfaces;
using RoutineAPP.HelperService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
