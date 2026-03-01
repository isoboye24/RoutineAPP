using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.HelperService;
using RoutineAPP.Infrastructure.Data;
using RoutineAPP.Infrastructure.Repositories;
using RoutineAPP.UI.ViewModel;
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


        public List<GetAllMonthsViewModel> GetAllMonths()
            => _dailyRoutineRepository.GetAllMonths();

        public List<ReportDetailsViewModel> GetReportDetailsByMonth(int month, int year)
        {
            var tasks = _taskRepository.GetTasksByMonth(month, year).ToList();

            if (!tasks.Any())
                return new List<ReportDetailsViewModel>();

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

                    return new ReportDetailsViewModel
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


        public List<ReportDetailsViewModel> GetReportDetailsByYear(int year)
        {
            var tasks = _taskRepository.GetTasksByYear(year).ToList();

            if (!tasks.Any())
                return new List<ReportDetailsViewModel>();

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

                    return new ReportDetailsViewModel
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


        public List<ReportDetailsViewModel> GetOverallReportDetails()
        {
            var tasks = _taskRepository.GetTotalTasks().ToList();

            if (!tasks.Any())
                return new List<ReportDetailsViewModel>();

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

                    return new ReportDetailsViewModel
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

        public List<Top5ReportViewModel> GetFormattedTop5MonthlyReport(int month, int year)
        {
            var data = _taskRepository.GetTop5MonthlyReport(month, year);

            if (!data.Any())
                return new List<Top5ReportViewModel>();

            int totalMinutes = data.Sum(x => x.TotalMinutes);

            return data.Select(x => new Top5ReportViewModel
            {
                CategoryName = x.CategoryName,
                FormattedTotalMinutes = GeneralHelper.FormatTime(x.TotalMinutes),
                Percentage = GeneralHelper.CalculatePercentage(x.TotalMinutes, totalMinutes)
            }).ToList();
        }

        public List<Top5ReportViewModel> GetFormattedTop5AnnualReport(int year)
        {
            var data = _taskRepository.GetTop5AnnualReport(year);

            if (!data.Any())
                return new List<Top5ReportViewModel>();

            int totalMinutes = data.Sum(x => x.TotalMinutes);

            return data.Select(x => new Top5ReportViewModel
            {
                CategoryName = x.CategoryName,
                FormattedTotalMinutes = GeneralHelper.FormatTime(x.TotalMinutes),
                Percentage = GeneralHelper.CalculatePercentage(x.TotalMinutes, totalMinutes)
            }).ToList();
        }
    }
    
}
