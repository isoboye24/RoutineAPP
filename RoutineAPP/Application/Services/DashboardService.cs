using RoutineAPP.Core.Interfaces;
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

        public string GetCategoryAnually(int year, string category)
            => _taskRepository.GetCategoryAnually(year, category);

        public string GetCategoryMonthly(int month, int year, string category)
            => _taskRepository.GetCategoryMonthly(month, year, category);
    }
}
