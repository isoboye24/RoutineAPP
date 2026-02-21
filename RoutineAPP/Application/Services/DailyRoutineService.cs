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
    public class DailyRoutineService : IDailyRoutineService
    {
        private readonly IDailyRoutineRepository _repository;

        public DailyRoutineService(IDailyRoutineRepository repository)
        {
            _repository = repository;
        }

        public int Count()
            => _repository.Count();

        public bool Create(DateTime date, string summary = null)
        {
            if (_repository.Exists(date))
                throw new Exception("Date already exists");

            var routine = new DailyRoutine(date, summary);
            return _repository.Insert(routine);
        }

        public bool Delete(int id)
            => _repository.Delete(id);

        public List<DailyRoutine> GetAll()
            => _repository.GetAll();

        public bool PermanentDelete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, DateTime date, string summary)
        {
            var routine = _repository.GetById(id);
            if (routine == null)
                throw new Exception("Daily not found");

            routine.Update(date, summary);
            return _repository.Update(routine);
        }
    }
}
