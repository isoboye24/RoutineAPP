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
        private readonly IGraphRepository _graphRepository;
        public GraphService(IGraphRepository graphRepository) 
        {
            _graphRepository = graphRepository;
        }

        public int GetAnnualSingleCategoryTime(int year, int categoryId)
            => _graphRepository.GetAnnualSingleCategoryTime(year, categoryId);

        public List<GetAllCategoriesViewModel> GetMonthlyCategoriesReport(int month, int year)
            => _graphRepository.GetMonthlyCategoriesReport(month, year);

        public List<GetSingleCategoryViewModel> GetSingleCategoryReport(int year, int categoryId)
            => _graphRepository.GetSingleCategoryReport(year, categoryId);

        public List<GetAllCategoriesViewModel> GetAllCategoriesAnnualReport(int year)
            => _graphRepository.GetAllCategoriesAnnualReport(year);
    }
}
