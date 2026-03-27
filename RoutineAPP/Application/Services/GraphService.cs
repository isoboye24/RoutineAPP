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

        public List<GetAllCategoriesDTO> GetMonthlyCategoriesReport(int month, int year)
        {
            var data = (from f in _repository.GetAll()
                        join m in _memberRepository.GetAll() on f.memberID equals m.MemberId
                        join g in _genderRepository.GetAll() on m.PersonalInfo.GenderId equals g.genderID
                        join p in _positionRepository.GetAll() on m.MembershipInfo.PositionId equals p.positionID
                        join c in _constitutionRepository.GetAll() on f.constitutionID equals c.constitutionID
                        select new FinedMemberDTO
                        {

                            FinedMemberId = f.finedMemberID,
                            AmountPaid = (decimal)f.amountPaid,
                            FormattedAmountPaid = (f.amountPaid + " €").ToString(),
                            Summary = f.summary,
                            ConstitutionId = f.constitutionID,
                            Section = c.section,
                            ShortDescription = c.ShortDescription,
                            AmountExpected = (c.fine + " €").ToString(),
                            Balance = (c.fine - f.amountPaid).ToString(),
                            MemberId = f.memberID,
                            FirstName = m.PersonalInfo.FirstName,
                            LastName = m.PersonalInfo.LastName,
                            ImagePath = m.PersonalInfo.ImagePath,
                            GenderId = m.PersonalInfo.GenderId,
                            GenderName = g.genderName,
                            PositionId = m.MembershipInfo.PositionId,
                            PositionName = p.positionName,
                            Status = f.amountPaid <= 0 ? "Not Paid" : (f.amountPaid > 0 && f.amountPaid < c.fine) ? "Not Completed" : f.amountPaid == c.fine ? "Completed" : ((f.amountPaid - c.fine) + " € Extra").ToString(),
                            FineDate = f.fineDate,
                            FormattedFineDate = f.fineDate.ToString("dd.MM.yyyy"),
                        }).OrderByDescending(x => x.FineDate.Year).ThenByDescending(x => x.FineDate.Month).ThenByDescending(x => x.FineDate.Day).ThenBy(x => x.FirstName).ToList();

            return data;



            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
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
            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
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
            return (from t in _db.TASKs
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted
                          && t.categoryID == categoryId
                          && !d.isDeleted
                          && d.routineDate.Year == year
                    select t.timeSpent)
            .Sum();

        }

        public List<GetAllCategoriesDTO> GetAllCategoriesAnnualReport(int year)
        {
            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
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
