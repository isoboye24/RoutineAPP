using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DAO
{
    public class ReportsDAO:APPContext
    {
        public List<MonthlyRoutinesDetailDTO> SelectMonthlyRoutineReports()
        {
            List<MonthlyRoutinesDetailDTO> monthlyRoutineReports = new List<MonthlyRoutinesDetailDTO>();
            List<int> monthIDCollection = new List<int>();
            List<int> monthIDs = new List<int>();
            List<int> yearsCollection = new List<int>();
            List<int> years = new List<int>();

            var tasksYear = db.TASKs.Where(x => x.isDeleted == false).ToList();
            foreach (var item in tasksYear)
            {
                yearsCollection.Add(item.year);
            }
            years = yearsCollection.Distinct().OrderByDescending(year => year).ToList();
            foreach (var yearItem in years)
            {
                var tasksMonth = db.TASKs.Where(x => x.isDeleted == false && x.year == yearItem).ToList();
                foreach (var item in tasksMonth)
                {
                    monthIDCollection.Add(item.monthID);
                }
                monthIDs = monthIDCollection.Distinct().OrderByDescending(monthID => monthID).ToList();
                monthIDCollection.Clear();
                foreach (var monthItem in monthIDs)
                {
                    MonthlyRoutinesDetailDTO dto = new MonthlyRoutinesDetailDTO();
                    dto.MonthlyReportID += 1;
                    dto.MonthID = monthItem;
                    dto.MonthName = General.ConventIntToMonth(monthItem);
                    dto.Year = yearItem;
                    monthlyRoutineReports.Add(dto);
                }
            }
            return monthlyRoutineReports;
        }
        public List<ReportsDetailDTO> SelectMonthlyReports(int month, int year)
        {
            List<ReportsDetailDTO> monthlyReports = new List<ReportsDetailDTO>();          
            List<int> totalTime = new List<int>();
            List<int> totalCategoryTime = new List<int>();
            List<int> populatedCategories = new List<int>();
            int counter = 0;

            var categories = db.CATEGORies.Where(x => x.isDeleted == false).ToList();            
            foreach (var category in categories)
            {
                int tasksCount = db.TASKs.Count(x => x.isDeleted == false && x.monthID == month && x.year == year && x.categoryID == category.categoryID);
                if (tasksCount > 0)
                {
                    populatedCategories.Add(category.categoryID);
                }                
            }
            foreach (var category in populatedCategories)
            {
                int tasksCount = db.TASKs.Count(x => x.isDeleted == false && x.monthID == month && x.year == year && x.categoryID == category);
                ReportsDetailDTO dto = new ReportsDetailDTO();
                dto.ReportID = ++counter;
                dto.CategoryID = category;
                CATEGORY categoryName = db.CATEGORies.First(x => x.isDeleted == false && x.categoryID == category);
                dto.Category = categoryName.categoryName;
                
                var allTasks = db.TASKs.Where(x => x.isDeleted == false && x.monthID == month && x.year == year).ToList();
                foreach (var task in allTasks)
                {
                    totalTime.Add(task.timeSpent);
                }
                int totalTimeSpent = totalTime.Sum();
                var tasks = db.TASKs.Where(x => x.isDeleted == false && x.monthID == month && x.year == year && x.categoryID == category).ToList();
                foreach (var task in tasks)
                {
                    totalCategoryTime.Add(task.timeSpent);
                }
                double totalTimeSpentInCatgory = totalCategoryTime.Sum();
                dto.TotalTimeForFormatting = totalTimeSpentInCatgory / totalTimeSpent;
                int hours = (int)Math.Floor(totalTimeSpentInCatgory / 60);
                int minutes = Convert.ToInt32(totalTimeSpentInCatgory % 60);
                if (hours < 1)
                {
                    dto.TotalTimeUsed = minutes + " min" + (minutes > 1 ? "s" : "");
                }
                else
                {
                    dto.TotalTimeUsed = hours + " hr" + (hours > 1 ? "s " : " ") + minutes + " min" + (minutes > 1 ? "s" : "");
                }
                dto.PercentageOfUsedTime = ((totalTimeSpentInCatgory / totalTimeSpent) * 100).ToString("0.00") + " %";
                
                monthlyReports.Add(dto);
                totalTime.Clear();
                totalCategoryTime.Clear();
            }
            var orderedList = monthlyReports
                                .OrderByDescending(x => x.TotalTimeForFormatting).ToList();

            return orderedList;            
        }

        public string SelectTotalHoursUsedInMonth(int month, int year)
        {
            List<decimal> minutes = new List<decimal>();
            var list = db.TASKs.Where(x => x.isDeleted == false && x.monthID == month && x.year == year).ToList();
            foreach (var item in list)
            {
                minutes.Add(item.timeSpent);
            }
            decimal totalMinutes = minutes.Sum();
            int totalHours = (int)Math.Floor(totalMinutes / 60);
            int min = Convert.ToInt32(totalMinutes % 60);
            string totalUsedTime;
            if (totalHours < 1)
            {
                totalUsedTime = totalMinutes + " min" + (min > 1 ? "s" : "");
            }
            else
            {
                totalUsedTime = totalHours + " hr" + (totalHours > 1 ? "s " : " ") + min + " min" + (min > 1 ? "s" : "");
            }
            return totalUsedTime;
        }
        public decimal SelectTotalHoursInMonth(int month, int year)
        {
            int days = db.DAILY_ROUTINE.Count(x => x.isDeleted == false && x.monthID == month && x.year == year);
            return days * 24;
        }

        public List<YearDetailDTO> SelectYearlyRoutineReports()
        {
            List<YearDetailDTO> yearlyRoutineReports = new List<YearDetailDTO>();
            List<int> yearsCollection = new List<int>();
            List<int> years = new List<int>();

            var tasksYear = db.TASKs.Where(x => x.isDeleted == false).ToList();
            foreach (var item in tasksYear)
            {
                yearsCollection.Add(item.year);
            }
            years = yearsCollection.Distinct().OrderByDescending(year => year).ToList();
            foreach (var yearItem in years)
            {
                YearDetailDTO dto = new YearDetailDTO();
                dto.YearID += 1;
                dto.Year = yearItem;
                yearlyRoutineReports.Add(dto);                
            }

            return yearlyRoutineReports;
        }

        public List<ReportsDetailDTO> SelectYearlyReports(int year)
        {
            List<ReportsDetailDTO> yearlyReports = new List<ReportsDetailDTO>();
            List<int> totalTime = new List<int>();
            List<int> totalCategoryTime = new List<int>();
            List<int> allTimes = new List<int>();
            int counter = 0;

            var allTime = db.TASKs.Where(x => x.isDeleted == false && x.year == year).ToList();
            foreach (var time in allTime)
            {
                allTimes.Add(time.timeSpent);
            }
            int totalAllTimes = allTimes.Sum();
            var categories = db.CATEGORies.Where(x => x.isDeleted == false).ToList();
            foreach (var item in categories)
            {
                int tasksCount = db.TASKs.Count(x => x.isDeleted == false && x.year == year && x.categoryID == item.categoryID);
                ReportsDetailDTO dto = new ReportsDetailDTO();
                dto.ReportID = ++counter;
                dto.CategoryID = item.categoryID;
                dto.Category = item.categoryName;
                dto.Year = year;
                if (tasksCount > 0)
                {
                    var allTasks = db.TASKs.Where(x => x.isDeleted == false && x.year == year).ToList();
                    foreach (var task in allTasks)
                    {
                        totalTime.Add(task.timeSpent);
                    }
                    int totalTimeSpent = totalTime.Sum();
                    var tasks = db.TASKs.Where(x => x.isDeleted == false && x.year == year && x.categoryID == item.categoryID).ToList();
                    foreach (var task in tasks)
                    {
                        totalCategoryTime.Add(task.timeSpent);
                    }
                    double totalTimeSpentInCatgory = totalCategoryTime.Sum();
                    dto.TotalTimeForFormatting = totalTimeSpentInCatgory / totalTimeSpent;
                    int hours = (int)Math.Floor(totalTimeSpentInCatgory / 60);
                    int minutes = Convert.ToInt32(totalTimeSpentInCatgory % 60);
                    if (hours < 1)
                    {
                        dto.TotalTimeUsed = minutes + " min" + (minutes > 1 ? "s" : "");
                    }
                    else
                    {
                        dto.TotalTimeUsed = hours + " hr" + (hours > 1 ? "s " : " ") + minutes + " min" + (minutes > 1 ? "s" : "");
                    }
                    dto.PercentageOfUsedTime = ((totalTimeSpentInCatgory / totalTimeSpent) * 100).ToString("0.00") + " %";
                }
                else
                {
                    dto.TotalTimeUsed = "0 min";
                    dto.PercentageOfUsedTime = "0 %";
                    dto.TotalTimeForFormatting = 0;
                }
                yearlyReports.Add(dto);
                totalTime.Clear();
                totalCategoryTime.Clear();
            }
            var orderedList = yearlyReports
                               .OrderByDescending(x => x.TotalTimeForFormatting).ToList();
            return orderedList;
        }

        public string SelectTotalHoursUsedInAYear(int year)
        {
            List<decimal> minutes = new List<decimal>();
            var list = db.TASKs.Where(x => x.isDeleted == false && x.year == year).ToList();
            foreach (var item in list)
            {
                minutes.Add(item.timeSpent);
            }
            decimal totalMinutes = minutes.Sum();
            int totalHours = (int)Math.Floor(totalMinutes / 60);
            int min = Convert.ToInt32(totalMinutes % 60);
            string totalUsedTime;
            if (totalHours < 1)
            {
                totalUsedTime = totalMinutes + " min" + (min > 1 ? "s" : "");
            }
            else
            {
                totalUsedTime = totalHours + " hr" + (totalHours > 1 ? "s " : " ") + min + " min" + (min > 1 ? "s" : "");
            }
            return totalUsedTime;
        }

        public string SelectTotalHoursUnusedInAYear(int year)
        {
            int days = db.DAILY_ROUTINE.Count(x => x.isDeleted == false && x.year == year);
            int overallTotalMinutes = days * 24 * 60;

            List<int> minutes = new List<int>();
            var list = db.TASKs.Where(x => x.isDeleted == false && x.year == year).ToList();
            foreach (var item in list)
            {
                minutes.Add(item.timeSpent);
            }
            int totalUsedMinutes = minutes.Sum();

            int totalUnusedMinutes = overallTotalMinutes - totalUsedMinutes;


            int totalHours = totalUnusedMinutes / 60;
            int remainingMinutes = Convert.ToInt32(totalUnusedMinutes % 60);

            string totalUsedTime;
            if (totalHours < 1)
            {
                totalUsedTime = remainingMinutes + " min" + (remainingMinutes > 1 ? "s" : "");
            }
            else
            {
                totalUsedTime = totalHours + " hr" + (totalHours > 1 ? "s " : " ") + remainingMinutes + " min" + (remainingMinutes > 1 ? "s" : "");
            }
            return totalUsedTime;
        }

        public int SelectOverallTotalHours()
        {
            int days = db.DAILY_ROUTINE.Count(x => x.isDeleted == false);
            return days * 24;
        }

        public string SelectOverallTotalUnusedHours()
        {
            int days = db.DAILY_ROUTINE.Count(x => x.isDeleted == false);
            int overallTotalMinutes = days * 24 * 60;

            List<int> minutes = new List<int>();
            var list = db.TASKs.Where(x => x.isDeleted == false).ToList();
            foreach (var item in list)
            {
                minutes.Add(item.timeSpent);
            }
            int totalUsedMinutes = minutes.Sum();

            int totalUnusedMinutes = overallTotalMinutes - totalUsedMinutes;


            int totalHours = totalUnusedMinutes / 60;
            int remainingMinutes = Convert.ToInt32(totalUnusedMinutes % 60);

            string totalUsedTime;
            if (totalHours < 1)
            {
                totalUsedTime = remainingMinutes + " min" + (remainingMinutes > 1 ? "s" : "");
            }
            else
            {
                totalUsedTime = totalHours + " hr" + (totalHours > 1 ? "s " : " ") + remainingMinutes + " min" + (remainingMinutes > 1 ? "s" : "");
            }
            return totalUsedTime;
        }

        public string SelectOverallTotalUsedHours()
        {
            List<decimal> minutes = new List<decimal>();
            var list = db.TASKs.Where(x => x.isDeleted == false).ToList();
            foreach (var item in list)
            {
                minutes.Add(item.timeSpent);
            }
            decimal totalMinutes = minutes.Sum();
            int totalHours = (int)Math.Floor(totalMinutes / 60);
            int min = Convert.ToInt32(totalMinutes % 60);
            string totalUsedTime;
            if (totalHours < 1)
            {
                totalUsedTime = totalMinutes + " min" + (min > 1 ? "s" : "");
            }
            else
            {
                totalUsedTime = totalHours + " hr" + (totalHours > 1 ? "s " : " ") + min + " min" + (min > 1 ? "s" : "");
            }
            return totalUsedTime;
        }


        public decimal SelectTotalHoursInYear(int year)
        {
            int days = db.DAILY_ROUTINE.Count(x => x.isDeleted == false && x.year == year);
            return days * 24;
        }

        public int SelectTotalMonths()
        {
            List<MonthlyRoutinesDetailDTO> monthlyRoutineReports = new List<MonthlyRoutinesDetailDTO>();
            List<int> monthIDCollection = new List<int>();
            List<int> monthIDs = new List<int>();
            List<int> yearsCollection = new List<int>();
            List<int> years = new List<int>();
            List<int> totalMonths = new List<int>();

            var tasksYear = db.DAILY_ROUTINE.Where(x => x.isDeleted == false).ToList();
            foreach (var item in tasksYear)
            {
                yearsCollection.Add(item.year);
            }
            years = yearsCollection.Distinct().OrderByDescending(year => year).ToList();
            foreach (var yearItem in years)
            {
                var tasksMonth = db.TASKs.Where(x => x.isDeleted == false && x.year == yearItem).ToList();
                foreach (var item in tasksMonth)
                {
                    monthIDCollection.Add(item.monthID);
                }
                monthIDs = monthIDCollection.Distinct().OrderByDescending(monthID => monthID).ToList();
                monthIDCollection.Clear();                
                foreach (var monthItem in monthIDs)
                {
                    totalMonths.Add(1);
                }
            }
            int count = totalMonths.Count();
            return count;
        }

        public string SelectReportDateRange()
        {
            var first = db.DAILY_ROUTINE
                .Where(x => x.isDeleted == false)
                .OrderBy(x => x.year)
                .ThenBy(x => x.monthID)
                .ThenBy(x => x.day)
                .Select(x => new { x.year, x.monthID, x.day })
                .FirstOrDefault();

            var last = db.DAILY_ROUTINE
                .Where(x => x.isDeleted == false)
                .OrderByDescending(x => x.year)
                .ThenByDescending(x => x.monthID)
                .ThenByDescending(x => x.day)
                .Select(x => new { x.year, x.monthID, x.day })
                .FirstOrDefault();

            if (first == null || last == null)
                return "No data available";

            var firstDate = new DateTime(first.year, first.monthID, first.day);
            var lastDate = new DateTime(last.year, last.monthID, last.day);

            return $"{firstDate:MMMM dd, yyyy} - {lastDate:MMMM dd, yyyy}";
        }


        public List<ReportsDetailDTO> SelectTotalReport()
        {
            List<ReportsDetailDTO> totalReports = new List<ReportsDetailDTO>();
            List<int> totalTime = new List<int>();
            List<int> totalCategoryTime = new List<int>();
            List<int> allTimes = new List<int>();
            int counter = 0;

            var allTime = db.TASKs.Where(x => x.isDeleted == false).ToList();
            foreach (var time in allTime)
            {
                allTimes.Add(time.timeSpent);
            }
            int totalAllTimes = allTimes.Sum();
            var categories = db.CATEGORies.Where(x => x.isDeleted == false).ToList();
            foreach (var item in categories)
            {
                int tasksCount = db.TASKs.Count(x => x.isDeleted == false && x.categoryID == item.categoryID);
                ReportsDetailDTO dto = new ReportsDetailDTO();
                dto.ReportID = ++counter;
                dto.CategoryID = item.categoryID;
                dto.Category = item.categoryName;
                dto.Year = 0;
                if (tasksCount > 0)
                {
                    var allTasks = db.TASKs.Where(x => x.isDeleted == false).ToList();
                    foreach (var task in allTasks)
                    {
                        totalTime.Add(task.timeSpent);
                    }
                    int totalTimeSpent = totalTime.Sum();
                    var tasks = db.TASKs.Where(x => x.isDeleted == false && x.categoryID == item.categoryID).ToList();
                    foreach (var task in tasks)
                    {
                        totalCategoryTime.Add(task.timeSpent);
                    }
                    double totalTimeSpentInCatgory = totalCategoryTime.Sum();
                    dto.TotalTimeForFormatting = totalTimeSpentInCatgory / totalTimeSpent;
                    int hours = (int)Math.Floor(totalTimeSpentInCatgory / 60);
                    int minutes = Convert.ToInt32(totalTimeSpentInCatgory % 60);
                    if (hours < 1)
                    {
                        dto.TotalTimeUsed = minutes + " min" + (minutes > 1 ? "s" : "");
                    }
                    else
                    {
                        dto.TotalTimeUsed = hours + " hr" + (hours > 1 ? "s " : " ") + minutes + " min" + (minutes > 1 ? "s" : "");
                    }
                    dto.PercentageOfUsedTime = ((totalTimeSpentInCatgory / totalTimeSpent) * 100).ToString("0.00") + " %";
                }
                else
                {
                    dto.TotalTimeUsed = "0 min";
                    dto.PercentageOfUsedTime = "0 %";
                    dto.TotalTimeForFormatting = 0;
                }
                totalReports.Add(dto);
                totalTime.Clear();
                totalCategoryTime.Clear();
            }
            var orderedList = totalReports
                               .OrderByDescending(x => x.TotalTimeForFormatting).ToList();
            return orderedList;
        }

        // Well improved code
        public List<ReportsDetailDTO> SelectMonthlyTop5Reports(int month, int year)
        {
            List<ReportsDetailDTO> monthlyReports = new List<ReportsDetailDTO>();
            int counter = 0;

            var allTasks = db.TASKs
                .Where(x => !x.isDeleted && x.monthID == month && x.year == year)
                .ToList();

            if (!allTasks.Any())
                return monthlyReports;

            int totalTimeSpent = allTasks.Sum(x => x.timeSpent);

            var categoryGroups = allTasks
                .GroupBy(t => t.categoryID)
                .Select(g => new
                {
                    CategoryID = g.Key,
                    TotalCategoryTime = g.Sum(t => t.timeSpent)
                })
                .ToList();

            foreach (var group in categoryGroups)
            {
                var category = db.CATEGORies.FirstOrDefault(x => !x.isDeleted && x.categoryID == group.CategoryID);
                if (category == null) continue;

                ReportsDetailDTO dto = new ReportsDetailDTO
                {
                    ReportID = ++counter,
                    CategoryID = category.categoryID,
                    Category = category.categoryName
                };

                double categoryTime = group.TotalCategoryTime;
                dto.TotalTimeForFormatting = categoryTime / totalTimeSpent;

                int hours = (int)Math.Floor(categoryTime / 60);
                int minutes = Convert.ToInt32(categoryTime % 60);

                dto.TotalTimeUsed = hours < 1
                    ? $"{minutes} min{(minutes != 1 ? "s" : "")}"
                    : $"{hours} hr{(hours != 1 ? "s " : " ")}{minutes} min{(minutes != 1 ? "s" : "")}";

                dto.PercentageOfUsedTime = ((categoryTime / totalTimeSpent) * 100).ToString("0.00") + " %";

                monthlyReports.Add(dto);
            }

            // Return only top 5
            var top5 = monthlyReports
                .OrderByDescending(x => x.TotalTimeForFormatting)
                .Take(5)
                .ToList();

            return top5;
        }


        public List<ReportsDetailDTO> SelectYearlyTop5Reports( int year)
        {
            List<ReportsDetailDTO> monthlyReports = new List<ReportsDetailDTO>();
            int counter = 0;

            var allTasks = db.TASKs
                .Where(x => !x.isDeleted && x.year == year)
                .ToList();

            if (!allTasks.Any())
                return monthlyReports;

            int totalTimeSpent = allTasks.Sum(x => x.timeSpent);

            var categoryGroups = allTasks
                .GroupBy(t => t.categoryID)
                .Select(g => new
                {
                    CategoryID = g.Key,
                    TotalCategoryTime = g.Sum(t => t.timeSpent)
                })
                .ToList();

            foreach (var group in categoryGroups)
            {
                var category = db.CATEGORies.FirstOrDefault(x => !x.isDeleted && x.categoryID == group.CategoryID);
                if (category == null) continue;

                ReportsDetailDTO dto = new ReportsDetailDTO
                {
                    ReportID = ++counter,
                    CategoryID = category.categoryID,
                    Category = category.categoryName
                };

                double categoryTime = group.TotalCategoryTime;
                dto.TotalTimeForFormatting = categoryTime / totalTimeSpent;

                int hours = (int)Math.Floor(categoryTime / 60);
                int minutes = Convert.ToInt32(categoryTime % 60);

                dto.TotalTimeUsed = hours < 1
                    ? $"{minutes} min{(minutes != 1 ? "s" : "")}"
                    : $"{hours} hr{(hours != 1 ? "s " : " ")}{minutes} min{(minutes != 1 ? "s" : "")}";

                dto.PercentageOfUsedTime = ((categoryTime / totalTimeSpent) * 100).ToString("0.00") + " %";

                monthlyReports.Add(dto);
            }

            // ✅ Return only top 5
            var top5 = monthlyReports
                .OrderByDescending(x => x.TotalTimeForFormatting)
                .Take(5)
                .ToList();

            return top5;
        }
    }
}
