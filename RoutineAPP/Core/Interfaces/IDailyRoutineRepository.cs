using RoutineAPP.Core.Entities;
using RoutineAPP.Infrastructure.Data;
using RoutineAPP.UI.ViewModel;
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
        int CountByMonth(int month, int year);
        int CountByYear(int year);
        List<int> GetOnlyYears();
        List<GetAllMonthsViewModel> GetAllMonths();
        string GetDateRange();

        List<DailyRoutineViewModel> GetComments();
        List<DailyRoutineViewModel> GetCommentById(int Id);

        int GetSummaryCount();
    }
}
