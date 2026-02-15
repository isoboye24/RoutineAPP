using RoutineAPP.DAL.DAO;
using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.BLL
{
    public class ReportsBLL
    {
        ReportsDAO dao = new ReportsDAO();
        MonthDAO monthdao = new MonthDAO();
        CategoryDAO categoryDAO = new CategoryDAO();
        YearDAO yearDAO = new YearDAO();
        public ReportDTO Select()
        {
            throw new NotImplementedException();
        }
        public ReportDTO Select(int year)
        {
            ReportDTO dto = new ReportDTO();
            dto.MonthlyRoutineReports = dao.SelectMonthlyRoutineReports();
            dto.YearlyRoutineReports = dao.SelectYearlyRoutineReports();
            dto.Months = monthdao.Select();
            dto.Years = yearDAO.Select();
            dto.YearlyReports = dao.SelectYearlyReports(year);
            dto.TotalReports = dao.SelectTotalReport();
            dto.Categories = categoryDAO.Select();

            return dto;
        }
        public ReportDTO SelectMonthlyReports(int month, int year)
        {
            ReportDTO dto = new ReportDTO();
            dto.MonthlyReports = dao.SelectMonthlyReports(month, year);
            dto.MonthlyTop5Reports = dao.SelectMonthlyTop5Reports(month, year);
            dto.YearlyTop5Reports = dao.SelectYearlyTop5Reports(year);
            dto.Categories = categoryDAO.Select();
            return dto;
        }
        public string SelectTotalHoursUsedInMonth(int month, int year)
        {
            return dao.SelectTotalHoursUsedInMonth(month, year);
        }
        public decimal SelectTotalHoursInMonth(int month, int year)
        {
            return dao.SelectTotalHoursInMonth(month, year);
        }

        public string SelectTotalHoursUsedInAYear(int year)
        {
            return dao.SelectTotalHoursUsedInAYear(year);
        }

        public int SelectOverallTotalHours()
        {
            return dao.SelectOverallTotalHours();
        }

        public string SelectOverallTotalUsedHours()
        {
            return dao.SelectOverallTotalUsedHours();
        }

        public string SelectOverallTotalUnusedHours()
        {
            return dao.SelectOverallTotalUnusedHours();
        }

        public string SelectReportDateRange()
        {
            return dao.SelectReportDateRange();
        }

        public decimal SelectTotalHoursInYear(int year)
        {
            return dao.SelectTotalHoursInYear(year);
        }

        public int SelectTotalMonths()
        {
            return dao.SelectTotalMonths();
        }
    }
}
