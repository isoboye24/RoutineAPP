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
    public class TaskRepository : ITaskRepository
    {

        private readonly RoutineDBEntities _db;
        public TaskRepository(RoutineDBEntities db)
        {
            _db = db;
        }
        public List<Core.Entities.Task> GetAll(int dailyId)
        {
            return _db.TASKs
                .Where(x => !x.isDeleted && x.dailiyRoutineID == dailyId)
                .OrderByDescending(x => x.year).ThenByDescending(x => x.monthID).ThenByDescending(x => x.day)
                .ToList()
                .Select(x => Core.Entities.Task.Rehydrate(x.taskID, x.dailiyRoutineID, x.categoryID, x.timeSpent, x.day, x.monthID, x.year, x.summary))
                .ToList();
        }

        public Core.Entities.Task GetById(int id)
        {
            var entity = _db.TASKs.FirstOrDefault(x => x.taskID == id && !x.isDeleted);
            if (entity == null) return null;

            var task = new Core.Entities.Task(entity.dailiyRoutineID, entity.categoryID, entity.timeSpent, entity.day, entity.monthID, entity.year, entity.summary);
            task.SetId(entity.taskID);
            return task;
        }

        public bool Insert(Core.Entities.Task task)
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

        public bool Update(Core.Entities.Task task)
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

        public bool Exists(int year, int month, int day)
        {
            return _db.TASKs.Any(x =>
                !x.isDeleted &&
                x.year == year &&
                x.monthID == month &&
                x.day == day);
        }

        public int Count()
        {
            return _db.TASKs.Count(x => !x.isDeleted);
        }
    }
}
