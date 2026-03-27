using RoutineAPP.Application.DTO;
using RoutineAPP.Core.Entities;
using System;
using System.Collections.Generic;

namespace RoutineAPP.Application.Interfaces
{
    public interface IDailyRoutineService
    {
        List<DailyRoutineDTO> GetAll();
        List<DailyRoutineDTO> GetAllDeletedRoutines();
        bool Create(DailyRoutine routine);
        bool Update(DailyRoutine routine);
        bool Delete(int id);
        bool PermanentDelete(int id);
        int Count();
        List<YearDTO> GetOnlyYears();

        List<DailyRoutineDTO> GetCommentById(int id);
        List<DailyRoutineDTO> GetComments(int year);

        List<GetAllMonthsDTO> GetAllMonths();

        (DateTime? FirstDate, DateTime? LastDate) GetDateRange();
    }
}
