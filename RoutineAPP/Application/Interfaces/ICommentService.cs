using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Interfaces
{
    public interface ICommentService
    {
        List<DailyRoutineDTO> GetComments(int year);
        List<DailyRoutineDTO> GetCommentById(int Id);

        int GetSummaryCount();
    }
}
