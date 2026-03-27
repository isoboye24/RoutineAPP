using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {

        private readonly RoutineDBEntities _db;
        public TaskRepository(RoutineDBEntities db)
        {
            _db = db;
        }

        public IQueryable<TASK> GetAll(int dailyId)
        {
            return _db.TASKs.Where(x => !x.isDeleted && x.dailiyRoutineID == dailyId);
        }
        
        public IQueryable<TASK> GetAllDeletedTasks()
        {
            return _db.TASKs.Where(x => x.isDeleted);
        }
        
        public IQueryable<TASK> GetById(int id)
        {
            return _db.TASKs.Where(x => x.taskID == id && !x.isDeleted);
        }

        public bool Insert(Task task)
        {
            _db.TASKs.Add(new TASK
            {
                dailiyRoutineID = task.DailyRoutineId,
                categoryID = task.CategoryId,
                timeSpent = task.TimeSpent,
                day = task.Day,
                monthID = task.Month,
                year = task.Year,
                summary = task.Summary,
            });

            _db.SaveChanges();
            return true;
        }

        public bool Update(Task task)
        {
            var entity = _db.TASKs.First(x => x.taskID == task.Id);
            entity.dailiyRoutineID = task.DailyRoutineId;
            entity.categoryID = task.CategoryId;
            entity.timeSpent = task.TimeSpent;
            entity.day = task.Day;
            entity.monthID = task.Month;
            entity.year = task.Year;
            entity.summary = task.Summary;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var entity = _db.TASKs.First(x => x.taskID == id);
            entity.isDeleted = true;
            entity.deletedDate = DateTime.Today;
            _db.SaveChanges();
            return true;
        }

        public bool PermanentDelete(int id)
        {
            var entity = _db.TASKs.FirstOrDefault(x => x.taskID == id);

            if (entity == null)
                return false;

            _db.TASKs.Remove(entity);
            _db.SaveChanges();

            return true;
        }

        public bool Exists(int categoryId, int routineId)
        {
            return _db.TASKs.Any(x =>
                !x.isDeleted 
                && x.categoryID == categoryId
                && x.dailiyRoutineID == routineId
                );
        }

        public int Count()
        {
            return _db.TASKs.Count(x => !x.isDeleted);
        }

        public IQueryable<TASK> GetTasksByMonth(int month, int year)
        {
            return _db.TASKs.Where(x => x.monthID == month && x.year == year && !x.isDeleted);
        }

        public IQueryable<TASK> GetTasksByYear(int year)
        {
            return _db.TASKs.Where(x => x.year == year && !x.isDeleted);
        }

        public IQueryable<TASK> GetTotalTasks()
        {
            return _db.TASKs.Where(x => !x.isDeleted);
        }

        public int GetCategoryTimeMonthly(int month, int year, string category)
        {
            return (from t in _db.TASKs
                                join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                                join c in _db.CATEGORies
                                    on t.categoryID equals c.categoryID
                                where !t.isDeleted
                                      && !c.isDeleted
                                      && !d.isDeleted
                                      && d.routineDate.Month == month
                                      && d.routineDate.Year == year
                                      && c.categoryName == category
                                select (int?)t.timeSpent).Sum() ?? 0;

        }

        public int GetCategoryTimeAnually(int year, string category)
        {
            return (from t in _db.TASKs
                                join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                                join c in _db.CATEGORies
                                    on t.categoryID equals c.categoryID
                                where !t.isDeleted
                                      && !c.isDeleted
                                      && !d.isDeleted
                                      && d.routineDate.Year == year
                                      && c.categoryName == category
                                select (int?)t.timeSpent).Sum() ?? 0;

        }
    }
}
