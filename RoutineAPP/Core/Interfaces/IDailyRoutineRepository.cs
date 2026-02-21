using RoutineAPP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface IDailyRoutineRepository
    {
        List<DailyRoutine> GetAll();
        DailyRoutine GetById(int id);
        bool Insert(DailyRoutine routine);
        bool Update(DailyRoutine routine);
        bool Delete(int id);
        bool PermanentDelete(int id);
        bool Exists(DateTime date);
        int Count();
    }
}
