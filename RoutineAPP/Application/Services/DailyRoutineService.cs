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

        public List<DailyRoutineDTO> GetComments(int year)
        {
            return _repository.GetComments(year)
                 .Select(x => new DailyRoutineDTO
                 {
                     Id = x.dailyRoutineID,
                     RoutineDate = x.routineDate,
                     FormattedRoutineDate = x.routineDate.ToString("dd.MMM.YYYY"),
                     Summary = x.summary,
                     Day = x.routineDate.Day,
                     MonthID = x.routineDate.Month,
                     MonthName = GeneralHelper.ConventIntToMonth(x.routineDate.Day),
                     Year = x.routineDate.Year,
                 })
                 .OrderByDescending(x => x.Year).ThenByDescending(x => x.MonthID).ThenByDescending(x => x.Day)
                 .ToList();
        }
        
        public List<DailyRoutineDTO> GetCommentById(int id)
        {
            return _repository.GetCommentById(id)
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
        {
            return _repository.GetYears()
                .OrderByDescending(x => x)
                .Select(x => new YearDTO
                {
                    YearID = x,
                    Year = x
                })
                .ToList();
        }

        public List<GetAllMonthsDTO> GetAllMonths()
        {
            var data = _repository.GetAll()
                        .Select(x => new
                        {
                            Year = x.routineDate.Year,
                            Month = x.routineDate.Month
                        })
                        .Distinct()
                        .OrderByDescending(x => x.Year)
                        .ThenByDescending(x => x.Month)
                        .ToList();

                            return data.Select(x => new GetAllMonthsDTO
                            {
                                Year = x.Year,
                                MonthID = x.Month,
                                Month = new DateTime(x.Year, x.Month, 1).ToString("MMMM")
                            }).ToList();
        }

        public (DateTime? FirstDate, DateTime? LastDate) GetDateRange()
        {
            var query = _repository.GetAll();

            if (!query.Any())
                return (null, null);

            return (
                query.Min(x => x.routineDate),
                query.Max(x => x.routineDate)
            );
        }

    }
}
