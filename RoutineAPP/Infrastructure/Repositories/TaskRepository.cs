using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.DAL.DTO;
using RoutineAPP.Infrastructure.Data;
using RoutineAPP.UI.ViewModel;
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

        public List<TaskViewModel> GetTasksByDay(int routineId)
        {
            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted && d.dailyRoutineID == routineId
                    select new TaskViewModel
                    {
                        Id = t.taskID,
                        Category = c.categoryName,
                        CategoryId = t.categoryID,
                        DailyRoutineDate = d.routineDate,
                        DailyRoutineId = t.dailiyRoutineID,
                        TimeSpent = t.timeSpent,
                        Summary = t.summary,
                        Day = d.routineDate.Day,
                        Month = d.routineDate.Month,
                        MonthName = General.ConventIntToMonth(d.routineDate.Month),
                        Year = d.routineDate.Year,
                    })
            .ToList();
        }

        public List<TaskViewModel> GetTaskDetails(int dailyId)
        {
            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted && t.dailiyRoutineID == dailyId
                    select new TaskViewModel
                    {
                        Id = t.taskID,
                        Category = c.categoryName,
                        CategoryId = t.categoryID,
                        DailyRoutineDate = d.routineDate,
                        DailyRoutineId = t.dailiyRoutineID,
                        TimeSpent = t.timeSpent,
                        Summary = t.summary,
                        Day = d.routineDate.Day,
                        Month = d.routineDate.Month,
                        MonthName = General.ConventIntToMonth(d.routineDate.Month),
                        Year = d.routineDate.Year,
                        TimeInHoursAndMinutes = t.timeSpent / 60 >= 1 ? $"{t.timeSpent / 60}h {t.timeSpent % 60}m" : $"{t.timeSpent % 60}m"
                    })
            .ToList();
        }

        public List<TaskViewModel> GetTasksByMonth(int month, int year)
        {
            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted && d.routineDate.Month == month && d.routineDate.Year == year
                    select new TaskViewModel
                    {
                        Id = t.taskID,
                        Category = c.categoryName,
                        CategoryId = t.categoryID,
                        DailyRoutineDate = d.routineDate,
                        DailyRoutineId = t.dailiyRoutineID,
                        TimeSpent = t.timeSpent,
                        Summary = t.summary,
                        Day = d.routineDate.Day,
                        Month = d.routineDate.Month,
                        MonthName = General.ConventIntToMonth(d.routineDate.Month),
                        Year = d.routineDate.Year,
                    })
            .ToList();
        }


        public List<TaskViewModel> GetTasksByYear(int year)
        {
            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted && d.routineDate.Year == year
                    select new TaskViewModel
                    {
                        Id = t.taskID,
                        Category = c.categoryName,
                        CategoryId = t.categoryID,
                        DailyRoutineDate = d.routineDate,
                        DailyRoutineId = t.dailiyRoutineID,
                        TimeSpent = t.timeSpent,
                        Summary = t.summary,
                        Day = d.routineDate.Day,
                        Month = d.routineDate.Month,
                        MonthName = General.ConventIntToMonth(d.routineDate.Month),
                        Year = d.routineDate.Year,
                    })
            .ToList();
        }

        public List<TaskViewModel> GetTotalTasks()
        {
            return (from t in _db.TASKs
                    join c in _db.CATEGORies on t.categoryID equals c.categoryID
                    join d in _db.DAILY_ROUTINE on t.dailiyRoutineID equals d.dailyRoutineID
                    where !t.isDeleted
                    select new TaskViewModel
                    {
                        Id = t.taskID,
                        Category = c.categoryName,
                        CategoryId = t.categoryID,
                        DailyRoutineDate = d.routineDate,
                        DailyRoutineId = t.dailiyRoutineID,
                        TimeSpent = t.timeSpent,
                        Summary = t.summary,
                        Day = d.routineDate.Day,
                        Month = d.routineDate.Month,
                        MonthName = General.ConventIntToMonth(d.routineDate.Month),
                        Year = d.routineDate.Year,
                    })
            .ToList();
        }

    }
}
