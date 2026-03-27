using RoutineAPP.Application.Interfaces;
using RoutineAPP.Application.DTO;
using System.Collections.Generic;
using System.Linq;

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
            return _repository.GetAll()
                .OrderBy(x => x.monthID)
                .Select(x => new MonthDTO
                {
                    MonthID = x.monthID,
                    MonthName = x.monthName
                })
                .ToList();
        }
    }
}
