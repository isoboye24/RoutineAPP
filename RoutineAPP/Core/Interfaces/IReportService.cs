using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface IReportService
    {
        string GetTotalHoursInDay();
        string GetTotalUsedTimeInDay(int routine);
        string GetTotalUnusedTimeInDay(int routine);

        List<GetAllMonthsViewModel> GetAllMonths();
        List<ReportDetailsViewModel> GetReportDetailsByMonth(int month, int year);
        string GetTotalUnusedTimeInMonth(int month, int year);
        string GetTotalUsedTimeInMonth(int month, int year);
        string GetTotalHoursInMonth(int month, int year);

        List<ReportDetailsViewModel> GetReportDetailsByYear(int year);
        string GetTotalHoursInYear(int year);
        string GetTotalUsedTimeInYear(int year);
        string GetTotalUnusedTimeInYear(int year);

        List<ReportDetailsViewModel> GetOverallReportDetails();
        string GetTotalOverallHours();
        string GetTotalOverallUsedTime();
        string GetTotalOverallUnusedTime();

        string GetDateRange();

        List<Top5ReportViewModel> GetFormattedTop5MonthlyReport(int month, int year);
        List<Top5ReportViewModel> GetFormattedTop5AnnualReport(int year);
    }
}
