using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface ICommentService
    {
        List<DailyRoutineViewModel> GetComments();
        List<DailyRoutineViewModel> GetCommentById(int Id);

        int GetSummaryCount();
    }
}
