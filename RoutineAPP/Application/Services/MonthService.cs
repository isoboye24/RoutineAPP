using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.UI.ViewModel;
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

        public List<MonthViewModel> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
