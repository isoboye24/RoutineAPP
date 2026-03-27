using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Application.DTO;
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

        public List<GetAllCategoriesDTO> GetMonthlyCategoriesReport(int month, int year)
        {
            var data = _graphRepository.GetMonthlyCategoriesReport(month, year);

            return data.Select(x => new GetAllCategoriesDTO
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                TotalHours = x.TotalMinutes / 60
            }).ToList();
        }

        public List<GetSingleCategoryDTO> GetSingleCategoryReport(int year, int categoryId)
        {
            var data = _graphRepository.GetSingleCategoryReport(year, categoryId);

            return data.Select(x => new GetSingleCategoryDTO
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                TotalHours = x.TotalMinutes / 60
            }).ToList();
        }

        public List<GetAllCategoriesDTO> GetAllCategoriesAnnualReport(int year)
        {
            var data = _graphRepository.GetAllCategoriesAnnualReport(year);

            return data.Select(x => new GetAllCategoriesDTO
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                TotalHours = x.TotalMinutes / 60
            }).ToList();
        }
    }
}
