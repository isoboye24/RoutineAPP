using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool Create(int dailyRoutineId, int categoryId, DateTime date, string summary = null)
        {
            if (_repository.Exists(date))
                throw new Exception("Task already exists");

            var task = new Core.Entities.Task(dailyRoutineId, categoryId, date, summary);
            return _repository.Insert(task);
        }

        public bool Delete(int id)
            => _repository.Delete(id);

        public List<Core.Entities.Task> GetAll()
            => _repository.GetAll();

        public bool PermanentDelete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, int dailyRoutineId, int categoryId, DateTime date, string summary)
        {
            var task = _repository.GetById(id);
            if (task == null)
                throw new Exception("Task not found");

            task.Update(dailyRoutineId, categoryId, date, summary);
            return _repository.Update(task);
        }

    }
}
