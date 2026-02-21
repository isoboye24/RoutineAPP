using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Services
{
    public class YearsService : IYearsService
    {
        private readonly IYearsRepository _repository;

        public YearsService(IYearsRepository repository)
        {
            _repository = repository;
        }

        public List<Years> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
