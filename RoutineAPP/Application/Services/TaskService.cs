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
using System.Windows.Forms;
using System.Xml.Linq;

namespace RoutineAPP.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public int Count()
            => _repository.Count();

        public bool Create(Core.Entities.Task task)
        {
            if (_repository.Exists(task.Year, task.Month, task.Day))
                throw new Exception("Task already exists");

            return _repository.Insert(task);
        }

        public bool Delete(int id)
            => _repository.Delete(id);

        public List<Core.Entities.Task> GetAll(int routineId)
            => _repository.GetAll(routineId);

        public bool PermanentDelete(int id)
            => _repository.PermanentDelete(id);

        public bool Update(Core.Entities.Task task)
        {
            var existing = _repository.GetById(task.Id);

            if (existing == null)
                throw new Exception("Task not found");

            existing.Update(
                task.DailyRoutineId,
                task.CategoryId,
                task.TimeSpent,
                task.Day,
                task.Month,
                task.Year,
                task.Summary);

            return _repository.Update(existing);
        }

        public List<TaskViewModel> GetTaskDetails(int dailyId)
        {
            return _repository.GetTaskDetails(dailyId);
        }

    }
}
