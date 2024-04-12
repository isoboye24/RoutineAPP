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
        public ReportDTO Select()
        {
            ReportDTO dto = new ReportDTO();
            dto.MonthlyRoutineReports = dao.SelectMonthlyRoutineReports();
            dto.YearlyRoutineReports = dao.SelectYearlyRoutineReports();
            dto.Months = monthdao.Select();
            return dto;
        }
        public ReportDTO SelectMonthlyReports(int month, int year)
        {
            ReportDTO dto = new ReportDTO();
            dto.MonthlyReports = dao.SelectMonthlyReports(month, year);
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

        public ReportDTO SelectYearlyReports(int year)
        {
            ReportDTO dto = new ReportDTO();
            dto.YearlyReports = dao.SelectYearlyReports(year);
            return dto;
        }
        public string SelectTotalHoursUsedInAYear(int year)
        {
            return dao.SelectTotalHoursUsedInAYear(year);
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
