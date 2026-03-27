using RoutineAPP.Core.Entities;
using RoutineAPP.Application.DTO;
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
    }
}
