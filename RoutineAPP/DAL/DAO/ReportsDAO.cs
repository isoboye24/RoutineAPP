using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var categories = db.CATEGORies.Where(x => x.isDeleted == false).ToList();            
            foreach (var item in categories)
            {
                int tasksCount = db.TASKs.Count(x => x.isDeleted == false && x.monthID == month && x.year == year && x.categoryID == item.categoryID);
                ReportsDetailDTO dto = new ReportsDetailDTO();
                dto.ReportID += 1;
                dto.CategoryID = item.categoryID;
                dto.Category = item.categoryName;
                if (tasksCount > 0)
                {
                    var allTasks = db.TASKs.Where(x => x.isDeleted == false && x.monthID == month && x.year == year).ToList();
                    foreach (var task in allTasks)
                    {
                        totalTime.Add(task.timeSpent);
                    }
                    int totalTimeSpent = totalTime.Sum();
                    var tasks = db.TASKs.Where(x => x.isDeleted == false && x.monthID == month && x.year == year && x.categoryID == item.categoryID).ToList();
                    foreach (var task in tasks)
                    {
                        totalCategoryTime.Add(task.timeSpent);
                    }
                    double totalTimeSpentInCatgory = totalCategoryTime.Sum();
                    dto.TotalTimeForFormatting = totalTimeSpentInCatgory/ totalTimeSpent;
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
                monthlyReports.Add(dto);
                totalTime.Clear();
                totalCategoryTime.Clear();
            }
            return monthlyReports;
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

        public List<YearlyDetailDTO> SelectYearlyRoutineReports()
        {
            List<YearlyDetailDTO> yearlyRoutineReports = new List<YearlyDetailDTO>();
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
                YearlyDetailDTO dto = new YearlyDetailDTO();
                dto.YearlyReportID += 1;
                dto.Year = yearItem;
                yearlyRoutineReports.Add(dto);                
            }
            return yearlyRoutineReports;
        }

        public List<ReportsDetailDTO> SelectYearlyReports(int year)
        {
            List<ReportsDetailDTO> monthlyReports = new List<ReportsDetailDTO>();
            List<int> totalTime = new List<int>();
            List<int> totalCategoryTime = new List<int>();
            List<int> allTimes = new List<int>();

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
                dto.ReportID += 1;
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
                monthlyReports.Add(dto);
                totalTime.Clear();
                totalCategoryTime.Clear();
            }
            return monthlyReports;
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
    }
}
