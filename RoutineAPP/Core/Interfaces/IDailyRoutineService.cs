using RoutineAPP.Core.Entities;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface IDailyRoutineService
    {
        List<DailyRoutine> GetAll();
        bool Create(DateTime date, string summary);
        bool Update(int id, DateTime date, string summary);
        bool Delete(int id);
        bool PermanentDelete(int id);
        int Count();
        List<YearViewModel> GetOnlyYears();
    }
}
