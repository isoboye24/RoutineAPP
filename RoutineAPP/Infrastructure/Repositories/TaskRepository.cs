using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Infrastructure.Data;
using System;
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

        public IQueryable<TASK> GetAll()
        {
            return _db.TASKs.Where(x => !x.isDeleted);
        }

        public IQueryable<TASK> GetAllDeletedTasks()
        {
            return _db.TASKs.Where(x => x.isDeleted);
        }

        public IQueryable<TASK> GetTasksByDay(int dailyId)
        {
            return _db.TASKs.Where(x => !x.isDeleted && x.dailiyRoutineID == dailyId);
        }

        public IQueryable<TASK> GetTasksByMonth(int month, int year)
        {
            return _db.TASKs.Where(x => x.monthID == month && x.year == year && !x.isDeleted);
        }

        public IQueryable<TASK> GetTasksByYear(int year)
        {
            return _db.TASKs.Where(x => x.year == year && !x.isDeleted);
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
                day = task.TaskDate.Day,
                monthID = task.TaskDate.Month,
                year = task.TaskDate.Year,
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
            entity.day = task.TaskDate.Day;
            entity.monthID = task.TaskDate.Month;
            entity.year = task.TaskDate.Year;
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

    }
}
