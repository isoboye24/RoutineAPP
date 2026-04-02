using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Infrastructure.Data;
using System;
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


        public IQueryable<DAILY_ROUTINE> GetAll()
        {
            return _db.DAILY_ROUTINE.Where(x => !x.isDeleted);
        }
        
        public IQueryable<DAILY_ROUTINE> GetAllByYear(int year)
        {
            return _db.DAILY_ROUTINE.Where(x => !x.isDeleted && x.routineDate.Year == year);
        }

        public IQueryable<DAILY_ROUTINE> GetAllByMonth(int month, int year)
        {
            return _db.DAILY_ROUTINE.Where(x => !x.isDeleted && x.routineDate.Month == month && x.routineDate.Year == year);
        }

        public IQueryable<DAILY_ROUTINE> GetAllDeletedRoutines()
        {
            return _db.DAILY_ROUTINE.Where(x => x.isDeleted);
        }

        public IQueryable<DAILY_ROUTINE> GetById(int id)
        {
            return _db.DAILY_ROUTINE.Where(x => !x.isDeleted && x.dailyRoutineID == id);
        }

        public IQueryable<DAILY_ROUTINE> GetComments(int year)
        {
            return _db.DAILY_ROUTINE.Where(x => !x.isDeleted && x.summary != null && x.year == year);
        }
        
        public IQueryable<DAILY_ROUTINE> GetAllComments()
        {
            return _db.DAILY_ROUTINE.Where(x => !x.isDeleted && x.summary != null);
        }

        public IQueryable<DAILY_ROUTINE> GetCommentById(int id)
        {
            return _db.DAILY_ROUTINE.Where(x => !x.isDeleted && x.dailyRoutineID == id && x.summary != null);
        }

        public IQueryable<int> GetYears()
        {
            return _db.DAILY_ROUTINE.Select(x => x.routineDate.Year).Distinct();
        }


        public int CountByMonth(int month, int year)
        {
            return _db.DAILY_ROUTINE.Count(x => !x.isDeleted && x.routineDate.Month == month && x.routineDate.Year == year);
        }

        public int CountByYear(int year)
        {
            return _db.DAILY_ROUTINE.Count(x => !x.isDeleted && x.routineDate.Year == year);
        }

        public int GetSummaryCountByYear(int year)
        {
            return _db.DAILY_ROUTINE.Count(x => !x.isDeleted && x.summary != null && x.year == year);
        }

        public int GetSummaryCount()
        {
            return _db.DAILY_ROUTINE.Count(x => !x.isDeleted && x.summary != null);
        }

        
    }
}
