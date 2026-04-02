using RoutineAPP.Application.DTO;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Core.Entities;
using RoutineAPP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
namespace RoutineAPP.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IDailyRoutineRepository _routineRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TaskService(ITaskRepository repository, IDailyRoutineRepository routineRepository, ICategoryRepository categoryRepository)
        {
            _repository = repository;
            _routineRepository = routineRepository;
            _categoryRepository = categoryRepository;
        }

        public int Count()
            => _repository.Count();

        public bool Create(Task task)
        {
            if (_repository.Exists(task.CategoryId, task.DailyRoutineId))
                throw new Exception("Task already exists");

            return _repository.Insert(task);
        }

        public bool Delete(int id)
            => _repository.Delete(id);

        public List<TaskDTO> GetTasksByDay(int routineId)
        {
            var data = (from t in _repository.GetTasksByDay(routineId)
                        join r in _routineRepository.GetAll()
                            on t.dailiyRoutineID equals r.dailyRoutineID
                        join c in _categoryRepository.GetAll()
                            on t.categoryID equals c.categoryID
                        where !t.isDeleted
                        select new
                        {
                            t.taskID,
                            t.categoryID,
                            c.categoryName,
                            r.routineDate,
                            t.dailiyRoutineID,
                            t.timeSpent,
                            t.summary
                        })
                        .ToList();

            return data.Select(x => new TaskDTO
            {
                Id = x.taskID,
                Category = x.categoryName,
                CategoryId = x.categoryID,
                DailyRoutineDate = x.routineDate,
                DailyRoutineId = x.dailiyRoutineID,
                TimeSpent = x.timeSpent,
                Summary = x.summary,
                Day = x.routineDate.Day,
                MonthID = x.routineDate.Month,
                Year = x.routineDate.Year,
                TimeInHoursAndMinutes = GeneralHelper.FormatTime(x.timeSpent)
            })
            .OrderByDescending(x => x.TimeSpent)
            .ToList();
        }

        public List<TaskDTO> GetTasksByMonth(int month, int year)
        {
            var data = (from t in _repository.GetTasksByMonth(month, year)
                        join r in _routineRepository.GetAll()
                            on t.dailiyRoutineID equals r.dailyRoutineID
                        join c in _categoryRepository.GetAll()
                            on t.categoryID equals c.categoryID
                        where !t.isDeleted
                        select new
                        {
                            t.taskID,
                            t.categoryID,
                            c.categoryName,
                            r.routineDate,
                            t.dailiyRoutineID,
                            t.timeSpent,
                            t.summary
                        })
                        .ToList();

            return data.Select(x => new TaskDTO
            {
                Id = x.taskID,
                Category = x.categoryName,
                CategoryId = x.categoryID,
                DailyRoutineDate = x.routineDate,
                DailyRoutineId = x.dailiyRoutineID,
                TimeSpent = x.timeSpent,
                Summary = x.summary,
                Day = x.routineDate.Day,
                MonthID = x.routineDate.Month,
                Year = x.routineDate.Year,
                TimeInHoursAndMinutes = GeneralHelper.FormatTime(x.timeSpent)
            })
            .OrderByDescending(x => x.TimeSpent)
            .ToList();
        }
        
        public List<TaskDTO> GetTasksByYear(int year)
        {
            var data = (from t in _repository.GetTasksByYear(year)
                        join r in _routineRepository.GetAll() on t.dailiyRoutineID equals r.dailyRoutineID
                        join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                        where !t.isDeleted
                        select new
                        {
                            t.taskID,
                            t.categoryID,
                            c.categoryName,
                            r.routineDate,
                            t.dailiyRoutineID,
                            t.timeSpent,
                            t.summary
                        })
                        .ToList();

            return data.Select(x => new TaskDTO
            {
                Id = x.taskID,
                Category = x.categoryName,
                CategoryId = x.categoryID,
                DailyRoutineDate = x.routineDate,
                DailyRoutineId = x.dailiyRoutineID,
                TimeSpent = x.timeSpent,
                Summary = x.summary,
                Day = x.routineDate.Day,
                MonthID = x.routineDate.Month,
                Year = x.routineDate.Year,
                TimeInHoursAndMinutes = GeneralHelper.FormatTime(x.timeSpent)
            })
            .OrderByDescending(x => x.TimeSpent).ToList();
        }

        public List<TaskDTO> GetAll()
        {
            var data = (from t in _repository.GetAll()
                        join r in _routineRepository.GetAll() on t.dailiyRoutineID equals r.dailyRoutineID
                        join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                        where !t.isDeleted
                        select new
                        {
                            t.taskID,
                            t.categoryID,
                            c.categoryName,
                            r.routineDate,
                            t.dailiyRoutineID,
                            t.timeSpent,
                            t.summary
                        })
                        .ToList();

            return data.Select(x => new TaskDTO
            {
                Id = x.taskID,
                Category = x.categoryName,
                CategoryId = x.categoryID,
                DailyRoutineDate = x.routineDate,
                DailyRoutineId = x.dailiyRoutineID,
                TimeSpent = x.timeSpent,
                Summary = x.summary,
                Day = x.routineDate.Day,
                MonthID = x.routineDate.Month,
                Year = x.routineDate.Year,
                TimeInHoursAndMinutes = GeneralHelper.FormatTime(x.timeSpent)
            })
            .OrderByDescending(x => x.TimeSpent).ToList();
        }

        public List<TaskDTO> GetAllDeletedTasks()
        {
            var data = (from t in _repository.GetAllDeletedTasks()
                        join r in _routineRepository.GetAll() on t.dailiyRoutineID equals r.dailyRoutineID
                        join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                        where !t.isDeleted
                        select new
                        {
                            t.taskID,
                            t.categoryID,
                            c.categoryName,
                            r.routineDate,
                            t.dailiyRoutineID,
                            t.timeSpent,
                            t.summary
                        })
                        .ToList();

            return data.Select(x => new TaskDTO
            {
                Id = x.taskID,
                Category = x.categoryName,
                CategoryId = x.categoryID,
                DailyRoutineDate = x.routineDate,
                DailyRoutineId = x.dailiyRoutineID,
                TimeSpent = x.timeSpent,
                Summary = x.summary,
                Day = x.routineDate.Day,
                MonthID = x.routineDate.Month,
                Year = x.routineDate.Year,
                TimeInHoursAndMinutes = GeneralHelper.FormatTime(x.timeSpent)
            })
            .OrderByDescending(x => x.TimeSpent).ToList();
        }

        public bool PermanentDelete(int id)
            => _repository.PermanentDelete(id);

        public bool Update(Task task)
        {
            var check = _repository.GetById(task.Id);
            if (check == null)
                throw new Exception("Task not found");

            task.UpdateCategory(task.CategoryId);
            task.UpdateTimeSpent(task.TimeSpent);
            task.UpdateTaskDate(task.TaskDate);
            task.UpdateSummary(task.Summary);

            return _repository.Update(task);
        }
    }
}
