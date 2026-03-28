using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Application.DTO;
using RoutineAPP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IDailyRoutineRepository _dailyRoutineRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ReportService(ITaskRepository taskRepository, IDailyRoutineRepository dailyRoutineRepository, ICategoryRepository categoryRepository)
        {
            _taskRepository = taskRepository;
            _dailyRoutineRepository = dailyRoutineRepository;
            _categoryRepository = categoryRepository;
        }


        public string GetTotalHoursInDay()
        {
            return $"24 hours";
        }

        public List<Top5ReportDTO> GetTop5AnnualReport(int year)
        {
            var query = (from t in _taskRepository.GetAll()
                         join r in _dailyRoutineRepository.GetAllByYear(year)
                             on t.dailiyRoutineID equals r.dailyRoutineID
                         join c in _categoryRepository.GetAll()
                             on t.categoryID equals c.categoryID
                         where !t.isDeleted
                         group new { t, c } by new
                         {
                             t.categoryID,
                             c.categoryName
                         }
                         into g
                         let totalMinutes = g.Sum(x => x.t.timeSpent)
                         orderby totalMinutes descending
                         select new Top5ReportDTO
                         {
                             CategoryId = g.Key.categoryID,
                             CategoryName = g.Key.categoryName,
                             TotalMinutes = totalMinutes
                         })
                        .Take(5);

            return query.ToList();
        }

        public List<Top5ReportDTO> GetTop5MonthlyReport(int month, int year)
        {
            var query = (
                from t in _db.TASKs
                join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                join c in _db.CATEGORies on t.categoryID equals c.categoryID
                where !t.isDeleted
                      && !c.isDeleted
                      && !d.isDeleted
                      && d.routineDate.Month == month
                      && d.routineDate.Year == year
                group new { t, c } by new
                {
                    t.categoryID,
                    c.categoryName
                }
                into g
                orderby g.Sum(x => x.t.timeSpent) descending
                select new Top5ReportDTO
                {
                    CategoryId = g.Key.categoryID,
                    CategoryName = g.Key.categoryName,
                    TotalMinutes = g.Sum(x => x.t.timeSpent)
                }
            )
            .Take(5);

            return query.ToList();
        }

        public string GetTotalUsedTimeInDay(int routine)
        {
            var tasks = _taskRepository.GetTasksByDay(routine);

            int totalUsedMinutes = tasks.Sum(x => x.TimeSpent);

            return GeneralHelper.FormatTime(totalUsedMinutes);
        }

        public string GetTotalUnusedTimeInDay(int routine)
        {

            var tasks = _taskRepository.GetTasksByDay(routine);

            int totalUsedMinutes = tasks.Sum(x => x.TimeSpent);

            int totalUnusedMinutes = (24 * 60) - totalUsedMinutes;

            return GeneralHelper.FormatTime(totalUnusedMinutes);
        }


        public List<GetAllMonthsDTO> GetAllMonths()
            => _dailyRoutineRepository.GetAllMonths();

        public List<ReportDTO> GetReportDetailsByMonth(int month, int year)
        {
            var tasks = _taskRepository.GetTasksByMonth(month, year).ToList();

            if (!tasks.Any())
                return new List<ReportDTO>();

            int totalMonthMinutes = tasks.Sum(t => t.TimeSpent);

            var grouped = tasks.GroupBy(t => t.CategoryId)
                                .Select(g => new
                                {
                                    CategoryId = g.Key,
                                    TotalMinutes = g.Sum(x => x.TimeSpent)
                                })
                                .ToList();

            var categories = _categoryRepository.GetAll().ToDictionary(c => c.CategoryID, c => c.CategoryName);

            int counter = 0;

            return grouped
                .Select(g =>
                {
                    double percentage = (double)g.TotalMinutes / totalMonthMinutes * 100;

                    

                    string formattedTime = GeneralHelper.FormatTime(g.TotalMinutes);

                    return new ReportDTO
                    {
                        ReportID = ++counter,
                        CategoryID = g.CategoryId,
                        Category = categories.TryGetValue(g.CategoryId, out var name)
                                    ? name
                                    : "Unknown",
                        CompletePercentage = percentage,
                        TotalTimeUsed = formattedTime,
                        Year = year,
                        PercentageOfUsedTime = percentage.ToString("0.00") + " %"
                    };
                })
                .OrderByDescending(x => x.CompletePercentage)
                .ToList();
        }
        
        public string GetTotalUnusedTimeInMonth(int month, int year)
        {
            var days = _dailyRoutineRepository.CountByMonth(month, year);
            int overallTotalMinutes = days * 24 * 60;

            var tasks = _taskRepository.GetTasksByMonth(month, year);

            int totalUsedMinutes = tasks.Sum(x => x.TimeSpent);

            int totalUnusedMinutes = overallTotalMinutes - totalUsedMinutes;

            return GeneralHelper.FormatTime(totalUnusedMinutes);
        }

        public string GetTotalUsedTimeInMonth(int month, int year)
        {
            var days = _dailyRoutineRepository.CountByMonth(month, year);
            int overallTotalMinutes = days * 24 * 60;

            var tasks = _taskRepository.GetTasksByMonth(month, year);

            int totalUsedMinutes = tasks.Sum(x => x.TimeSpent);

            return GeneralHelper.FormatTime(totalUsedMinutes);
        }

        public string GetTotalHoursInMonth(int month, int year)
        {
            var days = _dailyRoutineRepository.CountByMonth(month, year);
            int overallTotalHours = days * 24;

            return $"{overallTotalHours} hours";
        }


        public List<ReportDTO> GetReportDetailsByYear(int year)
        {
            var tasks = _taskRepository.GetTasksByYear(year).ToList();

            if (!tasks.Any())
                return new List<ReportDTO>();

            int totalMonthMinutes = tasks.Sum(t => t.TimeSpent);

            var grouped = tasks.GroupBy(t => t.CategoryId)
                                .Select(g => new
                                {
                                    CategoryId = g.Key,
                                    TotalMinutes = g.Sum(x => x.TimeSpent)
                                })
                                .ToList();

            var categories = _categoryRepository.GetAll().ToDictionary(c => c.CategoryID, c => c.CategoryName);

            int counter = 0;

            return grouped
                .Select(g =>
                {
                    double percentage = (double)g.TotalMinutes / totalMonthMinutes * 100;

                    

                    string formattedTime = GeneralHelper.FormatTime(g.TotalMinutes);

                    return new ReportDTO
                    {
                        ReportID = ++counter,
                        CategoryID = g.CategoryId,
                        Category = categories.TryGetValue(g.CategoryId, out var name)
                                    ? name
                                    : "Unknown",
                        CompletePercentage = percentage,
                        TotalTimeUsed = formattedTime,
                        PercentageOfUsedTime = percentage.ToString("0.00") + " %",
                        Year = year,
                    };
                })
                .OrderByDescending(x => x.CompletePercentage)
                .ToList();
        }

        public string GetTotalHoursInYear(int year)
        {
            var days = _dailyRoutineRepository.CountByYear(year);
            int overallTotalHours = days * 24;

            return $"{overallTotalHours} hours";
        }

        public string GetTotalUsedTimeInYear(int year)
        {
            var tasks = _taskRepository.GetTasksByYear(year);

            int totalUsedMinutes = tasks.Sum(x => x.TimeSpent);

            return GeneralHelper.FormatTime(totalUsedMinutes);
        }

        public string GetTotalUnusedTimeInYear(int year)
        {
            var days = _dailyRoutineRepository.CountByYear(year);
            int overallTotalMinutes = days * 24 * 60;

            var tasks = _taskRepository.GetTasksByYear(year);

            int totalUsedMinutes = tasks.Sum(x => x.TimeSpent);

            int totalUnusedMinutes = overallTotalMinutes - totalUsedMinutes;

            return GeneralHelper.FormatTime(totalUnusedMinutes);
        }


        public List<ReportDTO> GetOverallReportDetails()
        {
            var tasks = _taskRepository.GetTotalTasks().ToList();

            if (!tasks.Any())
                return new List<ReportDTO>();

            int totalMonthMinutes = tasks.Sum(t => t.TimeSpent);

            var grouped = tasks.GroupBy(t => t.CategoryId)
                                .Select(g => new
                                {
                                    CategoryId = g.Key,
                                    TotalMinutes = g.Sum(x => x.TimeSpent)
                                })
                                .ToList();

            var categories = _categoryRepository.GetAll().ToDictionary(c => c.CategoryID, c => c.CategoryName);

            int counter = 0;

            return grouped
                .Select(g =>
                {
                    double percentage = (double)g.TotalMinutes / totalMonthMinutes * 100;

                    string formattedTime = GeneralHelper.FormatTime(g.TotalMinutes);

                    return new ReportDTO
                    {
                        ReportID = ++counter,
                        CategoryID = g.CategoryId,
                        Category = categories.TryGetValue(g.CategoryId, out var name)
                                    ? name
                                    : "Unknown",
                        CompletePercentage = percentage,
                        TotalTimeUsed = formattedTime,
                        PercentageOfUsedTime = percentage.ToString("0.00") + " %"
                    };
                })
                .OrderByDescending(x => x.CompletePercentage)
                .ToList();
        }

        public string GetTotalOverallHours()
        {
            var days = _dailyRoutineRepository.Count();
            int overallTotalHours = days * 24;

            return $"{overallTotalHours} hours";
        }

        public string GetTotalOverallUsedTime()
        {
            var tasks = _taskRepository.GetTotalTasks();

            int totalUsedMinutes = tasks.Sum(x => x.TimeSpent);

            return GeneralHelper.FormatTime(totalUsedMinutes);
        }

        public string GetTotalOverallUnusedTime()
        {
            var days = _dailyRoutineRepository.Count();
            int overallTotalMinutes = days * 24 * 60;

            var tasks = _taskRepository.GetTotalTasks();

            int totalUsedMinutes = tasks.Sum(x => x.TimeSpent);

            int totalUnusedMinutes = overallTotalMinutes - totalUsedMinutes;

            int totalHours = totalUnusedMinutes / 60;
            int remainingMinutes = totalUnusedMinutes % 60;

            if (totalHours < 1)
                return $"{remainingMinutes} min";

            return $"{totalHours} hr{(totalHours > 1 ? "s" : "")} {remainingMinutes} min";
        }

        public string GetDateRange()
        {
            var (firstDate, lastDate) = _dailyRoutineRepository.GetDateRange();

            if (!firstDate.HasValue || !lastDate.HasValue)
                return "No data available";

            return $"{firstDate:MMMM dd, yyyy} - {lastDate:MMMM dd, yyyy}";
        }

        public List<Top5ReportDTO> GetFormattedTop5MonthlyReport(int month, int year)
        {
            var data = _taskRepository.GetTop5MonthlyReport(month, year);

            if (!data.Any())
                return new List<Top5ReportDTO>();

            int totalMinutes = data.Sum(x => x.TotalMinutes);

            return data.Select(x => new Top5ReportDTO
            {
                CategoryName = x.CategoryName,
                FormattedTotalMinutes = GeneralHelper.FormatTime(x.TotalMinutes),
                Percentage = GeneralHelper.CalculatePercentage(x.TotalMinutes, totalMinutes)
            }).ToList();
        }

        public List<Top5ReportDTO> GetFormattedTop5AnnualReport(int year)
        {
            var data = _taskRepository.GetTop5AnnualReport(year);

            if (!data.Any())
                return new List<Top5ReportDTO>();

            int totalMinutes = data.Sum(x => x.TotalMinutes);

            return data.Select(x => new Top5ReportDTO
            {
                CategoryName = x.CategoryName,
                FormattedTotalMinutes = GeneralHelper.FormatTime(x.TotalMinutes),
                Percentage = GeneralHelper.CalculatePercentage(x.TotalMinutes, totalMinutes)
            }).ToList();
        }
    }
    
}
