using RoutineAPP.Core.Entities;
using RoutineAPP.Infrastructure.Data;
using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoutineAPP.Application.Interfaces
{
    public interface IDailyRoutineRepository
    {
        IQueryable<DAILY_ROUTINE> GetAll();
        IQueryable<DAILY_ROUTINE> GetAllDeletedRoutines();
        IQueryable<DAILY_ROUTINE> GetById(int id);
        bool Insert(DailyRoutine routine);
        bool Update(DailyRoutine routine);
        bool Delete(int id);
        bool PermanentDelete(int id);
        bool Exists(DateTime date);
        int Count();

        int CountByMonth(int month, int year);
        int CountByYear(int year);
        List<YearDTO> GetOnlyYears();
        List<GetAllMonthsDTO> GetAllMonths();
        (DateTime? FirstDate, DateTime? LastDate) GetDateRange();

        List<DailyRoutineDTO> GetComments(int year);
        List<DailyRoutineDTO> GetCommentById(int Id);

        int GetSummaryCount();
    }
}
