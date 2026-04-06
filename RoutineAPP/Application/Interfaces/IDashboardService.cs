using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Interfaces
{
    public interface IDashboardService
    {
        string GetCategoryTimeMonthly(int month, int year, string category);
        string GetCategoryTimeAnually(int year, string category);
        List<Top5ReportDTO> GetTop5MonthlyReport(int month, int year);
        List<Top5ReportDTO> GetTop5AnnualReport(int year);
    }
}
