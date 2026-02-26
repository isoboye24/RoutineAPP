using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface IGraphRepository
    {
        List<GetSingleCategoryViewModel> GetSingleCategoryReport(int year, int categoryId);
        int GetAnnualSingleCategoryTime(int year, int categoryId);
        List<GetAllCategoriesViewModel> GetMonthlyCategoriesReport(int month, int year);
        List<GetAllCategoriesViewModel> GetAllCategoriesAnnualReport(int year);
    }
}
