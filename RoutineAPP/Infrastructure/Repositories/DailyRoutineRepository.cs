using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class DailyRoutineRepository : IDailyRoutineRepository
    {

        private readonly RoutineDBEntities _db;
        public DailyRoutineRepository(RoutineDBEntities db)
        {
            _db = db;
        }

        public IQueryable<DAILY_ROUTINE> GetAll()
        {
            return _db.DAILY_ROUTINE.Where(x => !x.isDeleted);
        }

        public IQueryable<DAILY_ROUTINE> GetAllDeletedRoutines()
        {
            return _db.DAILY_ROUTINE.Where(x => x.isDeleted);
        }

        public IQueryable<DAILY_ROUTINE> GetById(int id)
        {
            return _db.DAILY_ROUTINE.Where(x => !x.isDeleted && x.dailyRoutineID == id);
        }

        public bool Insert(DailyRoutine routine)
        {
            _db.DAILY_ROUTINE.Add(new DAILY_ROUTINE
            {
                routineDate = routine.Date,
                summary = routine.Summary,
            });

            _db.SaveChanges();
            return true;
        }

        public bool Update(DailyRoutine routine)
        {
            var entity = _db.DAILY_ROUTINE.First(x => x.dailyRoutineID == routine.Id);
            entity.routineDate = routine.Date;
            entity.summary = routine.Summary;
            _db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var entity = _db.DAILY_ROUTINE.First(x => x.dailyRoutineID == id);
            entity.isDeleted = true;
            entity.deletedDate = DateTime.Today;
            _db.SaveChanges();
            return true;
        }

        public bool PermanentDelete(int id)
        {
            var entity = _db.DAILY_ROUTINE.FirstOrDefault(x => x.dailyRoutineID == id);

            if (entity == null)
                return false;

            _db.DAILY_ROUTINE.Remove(entity);
            _db.SaveChanges();

            return true;
        }

        public bool Exists(DateTime date)
        {
            DateTime onlyDate = date.Date;

            return _db.DAILY_ROUTINE.Any(x =>
                !x.isDeleted &&
                x.routineDate.Year == onlyDate.Year &&
                x.routineDate.Month == onlyDate.Month &&
                x.routineDate.Day == onlyDate.Day);
        }

        public int Count()
        {
            return _db.DAILY_ROUTINE.Count(x => !x.isDeleted);
        }

        public int CountByMonth(int month, int year)
        {
            return _db.DAILY_ROUTINE.Count(x => !x.isDeleted && x.routineDate.Month == month && x.routineDate.Year == year);
        }

        public List<DailyRoutineDTO> GetComments(int year)
        {
            return _db.DAILY_ROUTINE
                    .Where(x => !x.isDeleted && x.summary != null && x.year == year)
                    .OrderByDescending(x => x.routineDate)
                    .Select(x => new DailyRoutineDTO
                    {
                        Id = x.dailyRoutineID,
                        RoutineDate = x.routineDate,
                        Summary = x.summary,
                        Day = x.routineDate.Day,
                        MonthID = x.routineDate.Month,
                        Year = x.routineDate.Year
                    })
                    .ToList();
        }

        public List<DailyRoutineDTO> GetCommentById(int Id)
        {
            return _db.DAILY_ROUTINE
                    .Where(x => !x.isDeleted && x.dailyRoutineID == Id && x.summary != null)
                    .OrderByDescending(x => x.routineDate)
                    .Select(x => new DailyRoutineDTO
                    {
                        Id = x.dailyRoutineID,
                        RoutineDate = x.routineDate,
                        Summary = x.summary,
                        Day = x.routineDate.Day,
                        MonthID = x.routineDate.Month,
                        Year = x.routineDate.Year
                    })
                    .ToList();
        }

        public List<YearDTO> GetOnlyYears()
        {
            return _db.DAILY_ROUTINE
                .Where(x => !x.isDeleted)
                .Select(x => x.routineDate.Year)
                .Distinct()
                .OrderByDescending(x => x)
                .Select(x => new YearDTO
                {
                    YearID = x,
                    Year = x
                })                
                .ToList();
        }

        public int CountByYear(int year)
        {
            return _db.DAILY_ROUTINE.Count(x => !x.isDeleted && x.routineDate.Year == year);
        }

        public int GetSummaryCount(int year)
        {
            return _db.DAILY_ROUTINE.Count(x => !x.isDeleted && x.summary != null);
        }

        public List<GetAllMonthsDTO> GetAllMonths()
        {
            return _db.DAILY_ROUTINE
                .Where(x => !x.isDeleted)
                .Select(x => new
                {
                    Year = x.routineDate.Year,
                    Month = x.routineDate.Month
                })
                .Distinct()
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToList()
                .Select(x => new GetAllMonthsDTO
                {
                    Year = x.Year,
                    MonthID = x.Month, 
                })
                .ToList();
        }

        public (DateTime? FirstDate, DateTime? LastDate) GetDateRange()
        {
            var result = _db.DAILY_ROUTINE
                .Where(x => !x.isDeleted)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    FirstDate = g.Min(x => x.routineDate),
                    LastDate = g.Max(x => x.routineDate)
                })
                .FirstOrDefault();

            if (result == null)
                return (null, null);

            return (result.FirstDate, result.LastDate);
        }

        public int GetSummaryCount()
        {
            throw new NotImplementedException();
        }
    }
}
