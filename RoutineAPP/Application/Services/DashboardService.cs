using RoutineAPP.Application.Interfaces;
using RoutineAPP.Helper;
using System.Linq;

namespace RoutineAPP.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IDailyRoutineRepository _routineRepository;
        private readonly ICategoryRepository _categoryRepository;
        public DashboardService(ITaskRepository taskRepository, IDailyRoutineRepository routineRepository, ICategoryRepository categoryRepository) 
        { 
            _taskRepository = taskRepository;
            _routineRepository = routineRepository;
            _categoryRepository = categoryRepository;
        }

        public string GetCategoryTimeMonthly(int month, int year, string category)
        {
            int totalTimeSpent = (from t in _taskRepository.GetAll()
                    join d in _routineRepository.GetAll() on t.dailiyRoutineID equals d.dailyRoutineID
                    join c in _categoryRepository.GetAll()
                        on t.categoryID equals c.categoryID
                    where !t.isDeleted
                          && !c.isDeleted
                          && !d.isDeleted
                          && d.routineDate.Month == month
                          && d.routineDate.Year == year
                          && c.categoryName == category
                    select (int?)t.timeSpent).Sum() ?? 0;

            return GeneralHelper.FormatTimeShort(totalTimeSpent);

        }

        public string GetCategoryTimeAnually(int year, string category)
        {
            int totalTimeSpent = (from t in _taskRepository.GetAll()
                    join d in _routineRepository.GetAll() on t.dailiyRoutineID equals d.dailyRoutineID
                    join c in _categoryRepository.GetAll()
                        on t.categoryID equals c.categoryID
                    where !t.isDeleted
                          && !c.isDeleted
                          && !d.isDeleted
                          && d.routineDate.Year == year
                          && c.categoryName == category
                    select (int?)t.timeSpent).Sum() ?? 0;

            return GeneralHelper.FormatTimeShort(totalTimeSpent);
        }
    }
}
