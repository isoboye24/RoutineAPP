using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.Infrastructure.Data;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class GraphRepository : IGraphRepository
    {
        private readonly RoutineDBEntities _db;

        public GraphRepository(RoutineDBEntities db)
        {            
            _db = db;
        }

        public List<GetAllCategoriesViewModel> GetMonthlyCategoriesReport(int month, int year)
        {
            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted
                          && !c.isDeleted
                          && !d.isDeleted
                          && d.routineDate.Month == month
                          && d.routineDate.Year == year
                    group t by new { c.categoryID, c.categoryName } into g
                    select new GetAllCategoriesViewModel
                    {
                        CategoryId = g.Key.categoryID,
                        CategoryName = g.Key.categoryName,
                        TotalMinutes = g.Sum(x => x.timeSpent)
                    })
            .OrderBy(x => x.CategoryName)
            .ToList();
        }

        public List<GetSingleCategoryViewModel> GetSingleCategoryReport(int year, int categoryId)
        {
            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted
                          && t.categoryID == categoryId
                          && !c.isDeleted
                          && !d.isDeleted
                          && d.routineDate.Year == year
                    group t by new { d.monthID, c.categoryName } into g
                    select new GetSingleCategoryViewModel
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
            return (from t in _db.TASKs
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted
                          && t.categoryID == categoryId
                          && !d.isDeleted
                          && d.routineDate.Year == year
                    select t.timeSpent)
            .Sum();

        }

        public List<GetAllCategoriesViewModel> GetAllCategoriesAnnualReport(int year)
        {
            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted
                          && !c.isDeleted
                          && !d.isDeleted
                          && d.routineDate.Year == year
                    group t by new { c.categoryID, c.categoryName } into g
                    select new GetAllCategoriesViewModel
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
