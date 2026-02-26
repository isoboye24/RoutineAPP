using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Services
{
    public class GraphService : IGraphService
    {
        private readonly IGraphService _graphService;
        public GraphService(IGraphService graphService) 
        { 
            _graphService = graphService;
        }

        public int GetAnnualSingleCategoryTime(int year, int categoryId)
            => _graphService.GetAnnualSingleCategoryTime(year, categoryId);

        public List<GetAllCategoriesViewModel> GetMonthlyCategoriesReport(int month, int year)
            => _graphService.GetMonthlyCategoriesReport(month, year);

        public List<GetSingleCategoryViewModel> GetSingleCategoryReport(int year, int categoryId)
            => _graphService.GetSingleCategoryReport(year, categoryId);

        public List<GetAllCategoriesViewModel> GetAllCategoriesAnnualReport(int year)
            => _graphService.GetAllCategoriesAnnualReport(year);
    }
}
