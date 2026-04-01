using RoutineAPP.Application.Interfaces;
using RoutineAPP.Application.DTO;
using RoutineAPP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var baseQuery =
                from t in _taskRepository.GetTasksByYear(year)
                join r in _dailyRoutineRepository.GetAll()
                    on t.dailiyRoutineID equals r.dailyRoutineID
                where !t.isDeleted && r.year == year
                select t;

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

        public string GetTotalUsedTimeInDay(int routine)
        {
            var tasks = _taskRepository.GetTasksByDay(routine);

            if (!tasks.Any())
                return "0 min";

            int totalUsedMinutes = tasks.Sum(x => x.timeSpent);

            return GeneralHelper.FormatTime(totalUsedMinutes);
        }

        public string GetTotalUnusedTimeInDay(int routine)
        {

            var tasks = _taskRepository.GetTasksByDay(routine);

            if (!tasks.Any())
                return "24 hrs";

            int totalUsedMinutes = tasks.Sum(x => x.timeSpent);

            int totalUnusedMinutes = (24 * 60) - totalUsedMinutes;

            return GeneralHelper.FormatTime(totalUnusedMinutes);
        }


        public List<GetAllMonthsDTO> GetAllMonths()
        {
            var result = _dailyRoutineRepository.GetAll()
                                                .GroupBy(r => new
                                                {
                                                    Year = r.routineDate.Year,
                                                    Month = r.routineDate.Month
                                                })
                                                .Select(g => new GetAllMonthsDTO
                                                {
                                                    Year = g.Key.Year,
                                                    Month = new DateTime(1, g.Key.Month, 1).ToString("MMMM"),
                                                    MonthID = g.Key.Month
                                                })
                                                .OrderByDescending(x => x.Year)
                                                .ThenByDescending(x => x.Month)
                                                .ToList();

            return result;
        }

        public List<ReportDTO> GetReportDetailsByMonth(int month, int year)
        {
            var tasks = _taskRepository.GetTasksByMonth(month, year).ToList();

            if (!tasks.Any())
                return new List<ReportDTO>();

            int totalMonthMinutes = tasks.Sum(t => t.timeSpent);

            var grouped = tasks.GroupBy(t => t.categoryID)
                                .Select(g => new
                                {
                                    CategoryId = g.Key,
                                    TotalMinutes = g.Sum(x => x.timeSpent)
                                })
                                .ToList();

            var categories = _categoryRepository.GetAll().ToDictionary(c => c.categoryID, c => c.categoryName);

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

            if (!tasks.Any())
                return GeneralHelper.FormatTime(overallTotalMinutes);

            int totalUsedMinutes = tasks.Sum(x => x.timeSpent);

            int totalUnusedMinutes = overallTotalMinutes - totalUsedMinutes;

            return GeneralHelper.FormatTime(totalUnusedMinutes);
        }

        public string GetTotalUsedTimeInMonth(int month, int year)
        {
            var tasks = _taskRepository.GetTasksByMonth(month, year);

            if (!tasks.Any())
                return "0 min";

            int totalUsedMinutes = tasks.Sum(x => x.timeSpent);

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

            int totalMonthMinutes = tasks.Sum(t => t.timeSpent);

            var grouped = tasks.GroupBy(t => t.categoryID)
                                .Select(g => new
                                {
                                    CategoryId = g.Key,
                                    TotalMinutes = g.Sum(x => x.timeSpent)
                                })
                                .ToList();

            var categories = _categoryRepository.GetAll().ToDictionary(c => c.categoryID, c => c.categoryName);

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

            if (!tasks.Any())
                return "0 min";

            int totalUsedMinutes = tasks.Sum(x => x.timeSpent);

            return GeneralHelper.FormatTime(totalUsedMinutes);
        }

        public string GetTotalUnusedTimeInYear(int year)
        {
            var days = _dailyRoutineRepository.CountByYear(year);
            int overallTotalMinutes = days * 24 * 60;

            var tasks = _taskRepository.GetTasksByYear(year);

            if (!tasks.Any())
                return GeneralHelper.FormatTime(overallTotalMinutes);

            int totalUsedMinutes = tasks.Sum(x => x.timeSpent);

            int totalUnusedMinutes = overallTotalMinutes - totalUsedMinutes;

            return GeneralHelper.FormatTime(totalUnusedMinutes);
        }


        public List<ReportDTO> GetOverallReportDetails()
        {
            var tasks = _taskRepository.GetAll().ToList();

            if (!tasks.Any())
                return new List<ReportDTO>();

            int totalMonthMinutes = tasks.Sum(t => t.timeSpent);

            var grouped = tasks.GroupBy(t => t.categoryID)
                                .Select(g => new
                                {
                                    CategoryId = g.Key,
                                    TotalMinutes = g.Sum(x => x.timeSpent)
                                })
                                .ToList();

            var categories = _categoryRepository.GetAll().ToDictionary(c => c.categoryID, c => c.categoryName);

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
            var tasks = _taskRepository.GetAll();

            if (!tasks.Any())
                return "0 min";

            int totalUsedMinutes = tasks.Sum(x => x.timeSpent);

            return GeneralHelper.FormatTime(totalUsedMinutes);
        }

        public string GetTotalOverallUnusedTime()
        {
            var days = _dailyRoutineRepository.Count();
            int overallTotalMinutes = days * 24 * 60;

            var tasks = _taskRepository.GetAll();

            if (!tasks.Any())
                return GeneralHelper.FormatTime(overallTotalMinutes);

            int totalUsedMinutes = tasks.Sum(x => x.timeSpent);

            int totalUnusedMinutes = overallTotalMinutes - totalUsedMinutes;

            int totalHours = totalUnusedMinutes / 60;
            int remainingMinutes = totalUnusedMinutes % 60;

            if (totalHours < 1)
                return $"{remainingMinutes} min";

            return $"{totalHours} hr{(totalHours > 1 ? "s" : "")} {remainingMinutes} min";
        }

        public string GetDateRange()
        {
            var query = _dailyRoutineRepository.GetAll();

            if (!query.Any())
                return "No data available";

            var firstDate = query.Min(x => x.routineDate);
            var lastDate = query.Max(x => x.routineDate);

            return $"{firstDate:MMMM dd, yyyy} - {lastDate:MMMM dd, yyyy}";
        }

    }
    
}
