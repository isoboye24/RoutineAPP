using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using RoutineAPP.Helper;

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

        public bool Create(DailyRoutine routine)
        {
            if (_repository.Exists(routine.Date))
                throw new Exception("Date already exists");

            return _repository.Insert(routine);
        }

        public bool Delete(int id)
            => _repository.Delete(id);

        public List<DailyRoutineDTO> GetAll()
        {
            return _repository.GetAll()
                .Select(x => new DailyRoutineDTO
                {
                    Id = x.dailyRoutineID,
                    RoutineDate = x.routineDate,
                    Summary = x.summary,
                    Day = x.routineDate.Day,
                    MonthID = x.routineDate.Month,
                    MonthName = GeneralHelper.ConventIntToMonth(x.routineDate.Day),
                    Year = x.routineDate.Year,
                })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.MonthID).ThenByDescending(x => x.Day)
                .ToList();
        }
        
        public List<DailyRoutineDTO> GetAllDeletedRoutines()
        {
            return _repository.GetAllDeletedRoutines()
                .Select(x => new DailyRoutineDTO
                {
                    Id = x.dailyRoutineID,
                    RoutineDate = x.routineDate,
                    Summary = x.summary,
                    Day = x.routineDate.Day,
                    MonthID = x.routineDate.Month,
                    MonthName = GeneralHelper.ConventIntToMonth(x.routineDate.Day),
                    Year = x.routineDate.Year,
                })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.MonthID).ThenByDescending(x => x.Day)
                .ToList();
        }

        public List<YearDTO> GetOnlyYears()
            => _repository.GetOnlyYears();
        

        public bool PermanentDelete(int id)
            => _repository.PermanentDelete(id);

        public bool Update(DailyRoutine routine)
        {
            var check = _repository.GetById(routine.Id);
            if (check == null)
                throw new Exception("Daily not found");

            routine.Update(routine.Date, routine.Summary);
            return _repository.Update(routine);
        }
        
    }
}
