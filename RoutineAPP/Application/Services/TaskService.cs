using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using RoutineAPP.Helper;
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
            return (from t in _repository.GetTasksByDay(routineId).ToList()
                    join r in _routineRepository.GetAll() on t.dailiyRoutineID equals r.dailyRoutineID
                    join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                    select new TaskDTO
                    {
                        Id = t.taskID,
                        Category = c.categoryName,
                        CategoryId = t.categoryID,
                        DailyRoutineDate = r.routineDate,
                        DailyRoutineId = t.dailiyRoutineID,
                        TimeSpent = t.timeSpent,
                        Summary = t.summary,
                        Day = r.routineDate.Day,
                        MonthID = r.routineDate.Month,
                        Year = r.routineDate.Year,
                        TimeInHoursAndMinutes = GeneralHelper.FormatTime(t.timeSpent)
                    })
            .ToList();
        }
        
        public List<TaskDTO> GetTasksByMonth(int month, int year)
        {
            return (from t in _repository.GetTasksByMonth(month, year).ToList()
                    join r in _routineRepository.GetAll() on t.dailiyRoutineID equals r.dailyRoutineID
                    join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                    where !t.isDeleted
                    select new TaskDTO
                    {
                        Id = t.taskID,
                        Category = c.categoryName,
                        CategoryId = t.categoryID,
                        DailyRoutineDate = r.routineDate,
                        DailyRoutineId = t.dailiyRoutineID,
                        TimeSpent = t.timeSpent,
                        Summary = t.summary,
                        Day = r.routineDate.Day,
                        MonthID = r.routineDate.Month,
                        Year = r.routineDate.Year,
                        TimeInHoursAndMinutes = GeneralHelper.FormatTime(t.timeSpent)
                    })
            .ToList();
        }
        
        public List<TaskDTO> GetTasksByYear(int year)
        {
            return (from t in _repository.GetTasksByYear(year).ToList()
                    join r in _routineRepository.GetAll() on t.dailiyRoutineID equals r.dailyRoutineID
                    join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                    where !t.isDeleted
                    select new TaskDTO
                    {
                        Id = t.taskID,
                        Category = c.categoryName,
                        CategoryId = t.categoryID,
                        DailyRoutineDate = r.routineDate,
                        DailyRoutineId = t.dailiyRoutineID,
                        TimeSpent = t.timeSpent,
                        Summary = t.summary,
                        Day = r.routineDate.Day,
                        MonthID = r.routineDate.Month,
                        Year = r.routineDate.Year,
                        TimeInHoursAndMinutes = GeneralHelper.FormatTime(t.timeSpent)
                    })
            .ToList();
        }

        public List<TaskDTO> GetAll()
        {
            return (from t in _repository.GetAll().ToList()
                    join r in _routineRepository.GetAll() on t.dailiyRoutineID equals r.dailyRoutineID
                    join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                    where !t.isDeleted
                    select new TaskDTO
                    {
                        Id = t.taskID,
                        Category = c.categoryName,
                        CategoryId = t.categoryID,
                        DailyRoutineDate = r.routineDate,
                        DailyRoutineId = t.dailiyRoutineID,
                        TimeSpent = t.timeSpent,
                        Summary = t.summary,
                        Day = r.routineDate.Day,
                        MonthID = r.routineDate.Month,
                        Year = r.routineDate.Year,
                        TimeInHoursAndMinutes = GeneralHelper.FormatTime(t.timeSpent)
                    })
            .ToList();
        }
        
        public List<TaskDTO> GetAllDeletedTasks()
        {
            return (from t in _repository.GetAllDeletedTasks()
                    join r in _routineRepository.GetAll() on t.dailiyRoutineID equals r.dailyRoutineID
                    join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                    where !t.isDeleted
                    select new TaskDTO
                    {
                        Id = t.taskID,
                        Category = c.categoryName,
                        CategoryId = t.categoryID,
                        DailyRoutineDate = r.routineDate,
                        DailyRoutineId = t.dailiyRoutineID,
                        TimeSpent = t.timeSpent,
                        Summary = t.summary,
                        Day = r.routineDate.Day,
                        MonthID = r.routineDate.Month,
                        Year = r.routineDate.Year,
                    })
            .ToList();
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
