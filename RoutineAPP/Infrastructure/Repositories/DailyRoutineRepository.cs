using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class DailyRoutineRepository : IDailyRoutineRepository
    {

        private readonly RoutineDBEntities _db;
        public DailyRoutineRepository(RoutineDBEntities db)
        {
            _db = db;
        }
        public List<DailyRoutine> GetAll()
        {
            return _db.DAILY_ROUTINE
                .Where(x => !x.isDeleted)
                .OrderByDescending(x => x.routineDate.Year).ThenByDescending(x => x.routineDate.Month).ThenByDescending(x => x.routineDate.Day)
                .ToList()
                .Select(x => DailyRoutine.Rehydrate(x.dailyRoutineID, x.routineDate, x.summary))
                .ToList();
        }

        public DailyRoutine GetById(int id)
        {
            var entity = _db.DAILY_ROUTINE.FirstOrDefault(x => x.dailyRoutineID == id && !x.isDeleted);
            if (entity == null) return null;

            var routine = new DailyRoutine(entity.routineDate);
            routine.SetId(entity.dailyRoutineID);
            return routine;
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
    }
}
