using RoutineAPP.Application.DTO;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Helper;
using System.Collections.Generic;
using System.Linq;

namespace RoutineAPP.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IDailyRoutineRepository _routineRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDailyRoutineRepository _dailyRoutineRepository;
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


        public List<Top5ReportDTO> GetTop5AnnualReport(int year)
        {
            var baseQuery = _taskRepository.GetTasksByYear(year);

            int totalAnnualMinutes = baseQuery.Sum(x => (int?)x.timeSpent) ?? 0;

            var data = (from t in baseQuery
                        join c in _categoryRepository.GetAll()
                            on t.categoryID equals c.categoryID
                        group new { t, c } by new
                        {
                            t.categoryID,
                            c.categoryName
                        }
                        into g
                        let categoryMinutes = g.Sum(x => (int?)x.t.timeSpent) ?? 0
                        orderby categoryMinutes descending
                        select new
                        {
                            CategoryId = g.Key.categoryID,
                            CategoryName = g.Key.categoryName,
                            TotalMinutes = categoryMinutes
                        })
                        .Take(5)
                        .ToList();

            return data.Select(x => new Top5ReportDTO
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                TotalMinutes = x.TotalMinutes,
                FormattedTotalMinutes = GeneralHelper.FormatTime(x.TotalMinutes),
                Percentage = GeneralHelper.CalculatePercentage(x.TotalMinutes, totalAnnualMinutes)
            }).ToList();
        }

        public List<Top5ReportDTO> GetTop5MonthlyReport(int month, int year)
        {
            var baseQuery = _taskRepository.GetTasksByMonth(month, year);

            int totalMonthlyMinutes = baseQuery.Sum(x => (int?)x.timeSpent) ?? 0;

            var data = (from t in baseQuery
                        join c in _categoryRepository.GetAll()
                            on t.categoryID equals c.categoryID
                        group new { t, c } by new
                        {
                            t.categoryID,
                            c.categoryName
                        }
                        into g
                        let categoryMinutes = g.Sum(x => (int?)x.t.timeSpent) ?? 0
                        orderby categoryMinutes descending
                        select new
                        {
                            CategoryId = g.Key.categoryID,
                            CategoryName = g.Key.categoryName,
                            TotalMinutes = categoryMinutes
                        })
                        .Take(5)
                        .ToList();

            return data.Select(x => new Top5ReportDTO
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                TotalMinutes = x.TotalMinutes,
                FormattedTotalMinutes = GeneralHelper.FormatTime(x.TotalMinutes),
                Percentage = GeneralHelper.CalculatePercentage(x.TotalMinutes, totalMonthlyMinutes)
            }).ToList();
        }

    }
}
