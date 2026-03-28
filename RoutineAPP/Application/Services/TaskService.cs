using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Application.DTO;
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

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
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
            return (from t in _repository.GetTasksByDay(routineId)
                    join r in _routineRepository.GetAll() on t.dailiyRoutineID equals r.dailyRoutineID
                    join c in _categoryRepository.GetAll() on t.categoryID equals c.categoryID
                    where !t.isDeleted && r.dailyRoutineID == routineId
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
        
        public List<TaskDTO> GetTasksByMonth(int month, int year)
        {
            return (from t in _repository.GetTasksByMonth(month, year)
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
        
        public List<TaskDTO> GetTasksByYear(int year)
        {
            return (from t in _repository.GetTasksByYear(year)
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

        public List<TaskDTO> GetAll()
        {
            return (from t in _repository.GetAll()
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
            var existing = _repository.GetById(task.Id);

            if (existing == null)
                throw new Exception("Task not found");

            task.Update(
                task.DailyRoutineId,
                task.CategoryId,
                task.TimeSpent,
                task.Day,
                task.Month,
                task.Year,
                task.Summary);

            return _repository.Update(task);
        }
    }
}
