using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.Infrastructure.Data;
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

        public bool Create(int dailyRoutineId, int categoryId, int timeSpent, int day, int month, int year, string summary = null)
        {
            if (_repository.Exists(year, month, day))
                throw new Exception("Task already exists");

            var task = new Core.Entities.Task(dailyRoutineId, categoryId, timeSpent, day, month, year, summary);
            return _repository.Insert(task);
        }

        public bool Delete(int id)
            => _repository.Delete(id);

        public List<Core.Entities.Task> GetAll()
            => _repository.GetAll();

        public bool PermanentDelete(int id)
            => _repository.PermanentDelete(id);

        public bool Update(int id, int dailyRoutineId, int categoryId, int timeSpent, int day, int month, int year, string summary)
        {
            var task = _repository.GetById(id);
            if (task == null)
                throw new Exception("Task not found");

            task.Update(dailyRoutineId, categoryId, timeSpent, day, month, year, summary);
            return _repository.Update(task);
        }

    }
}
