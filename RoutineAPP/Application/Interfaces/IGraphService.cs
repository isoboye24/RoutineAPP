using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Interfaces
{
    public interface IGraphService
    {
        List<GetAllCategoriesDTO> GetDailyReport(int routineId);
        List<GetSingleCategoryDTO> GetSingleCategoryReport(int year, int categoryId);
        int GetAnnualSingleCategoryTime(int year, int categoryId);
        List<GetAllCategoriesDTO> GetMonthlyCategoriesReport(int month, int year);
        List<GetAllCategoriesDTO> GetAllCategoriesAnnualReport(int year);
    }
}
