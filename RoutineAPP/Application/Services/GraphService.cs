using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.Infrastructure.Data;
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
        {
            var data = _graphRepository.GetMonthlyCategoriesReport(month, year);

            return data.Select(x => new GetAllCategoriesViewModel
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                TotalHours = x.TotalMinutes / 60
            }).ToList();
        }

        public List<GetSingleCategoryViewModel> GetSingleCategoryReport(int year, int categoryId)
        {
            var data = _graphRepository.GetSingleCategoryReport(year, categoryId);

            return data.Select(x => new GetSingleCategoryViewModel
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                TotalHours = x.TotalMinutes / 60
            }).ToList();
        }

        public List<GetAllCategoriesViewModel> GetAllCategoriesAnnualReport(int year)
        {
            var data = _graphRepository.GetAllCategoriesAnnualReport(year);

            return data.Select(x => new GetAllCategoriesViewModel
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                TotalHours = x.TotalMinutes / 60
            }).ToList();
        }
    }
}
