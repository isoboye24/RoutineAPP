using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Interfaces
{
    public interface IReportService
    {
        string GetTotalHoursInDay();
        string GetTotalUsedTimeInDay(int routine);
        string GetTotalUnusedTimeInDay(int routine);

        List<GetAllMonthsDTO> GetAllMonths();
        List<ReportDTO> GetReportDetailsByMonth(int month, int year);
        string GetTotalUnusedTimeInMonth(int month, int year);
        string GetTotalUsedTimeInMonth(int month, int year);
        string GetTotalHoursInMonth(int month, int year);

        List<ReportDTO> GetReportDetailsByYear(int year);
        string GetTotalHoursInYear(int year);
        string GetTotalUsedTimeInYear(int year);
        string GetTotalUnusedTimeInYear(int year);

        List<ReportDTO> GetOverallReportDetails();
        string GetTotalOverallHours();
        string GetTotalOverallUsedTime();
        string GetTotalOverallUnusedTime();

        string GetDateRange();

        List<Top5ReportDTO> GetFormattedTop5MonthlyReport(int month, int year);
        List<Top5ReportDTO> GetFormattedTop5AnnualReport(int year);
    }
}
