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
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDailyRoutineRepository _routineRepository;
        public GraphService(ITaskRepository taskRepository, ICategoryRepository categoryRepository, IDailyRoutineRepository routineRepository) 
        {
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
            _routineRepository = routineRepository;
        }

        public List<GetAllCategoriesDTO> GetMonthlyCategoriesReport(int month, int year)
        {
            return (from t in _taskRepository.GetAll()
                    join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                    join d in _routineRepository.GetAll() on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted
                          && !c.isDeleted
                          && !d.isDeleted
                          && d.routineDate.Month == month
                          && d.routineDate.Year == year
                    group t by new { c.categoryID, c.categoryName } into g
                    select new GetAllCategoriesDTO
                    {
                        CategoryId = g.Key.categoryID,
                        CategoryName = g.Key.categoryName,
                        TotalMinutes = g.Sum(x => x.timeSpent)
                    })
            .OrderBy(x => x.CategoryName)
            .ToList();
        }

        public List<GetSingleCategoryDTO> GetSingleCategoryReport(int year, int categoryId)
        {
            return (from t in _taskRepository.GetAll()
                    join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                    join d in _routineRepository.GetAll() on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted
                          && t.categoryID == categoryId
                          && !c.isDeleted
                          && !d.isDeleted
                          && d.routineDate.Year == year
                    group t by new { d.monthID, c.categoryName } into g
                    select new GetSingleCategoryDTO
                    {
                        MonthID = g.Key.monthID,
                        TotalMinutes = g.Sum(x => x.timeSpent),
                        CategoryName = g.Key.categoryName,
                    })
            .OrderBy(x => x.MonthID)
            .ToList();
        }

        public int GetAnnualSingleCategoryTime(int year, int categoryId)
        {
            return (from t in _taskRepository.GetAll()
                    join d in _routineRepository.GetAll() on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted
                          && t.categoryID == categoryId
                          && !d.isDeleted
                          && d.routineDate.Year == year
                    select t.timeSpent)
            .Sum();

        }

        public List<GetAllCategoriesDTO> GetAllCategoriesAnnualReport(int year)
        {
            return (from t in _taskRepository.GetAll()
                    join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                    join d in _routineRepository.GetAll() on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted
                          && !c.isDeleted
                          && !d.isDeleted
                          && d.routineDate.Year == year
                    group t by new { c.categoryID, c.categoryName } into g
                    select new GetAllCategoriesDTO
                    {
                        CategoryId = g.Key.categoryID,
                        CategoryName = g.Key.categoryName,
                        TotalMinutes = g.Sum(x => x.timeSpent)
                    })
                    .OrderBy(x => x.CategoryName)
                    .ToList();
        }
    }
}
