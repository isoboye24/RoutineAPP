using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Services
{
    public class MonthService : IMonthService
    {
        private readonly IMonthRepository _repository;

        public MonthService(IMonthRepository repository)
        {
            _repository = repository;
        }

        public List<MonthDTO> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
