using RoutineAPP.Core.Entities;
using RoutineAPP.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Interfaces
{
    public interface ITaskService
    {
        List<Entities.Task> GetAll(int routineId);
        bool Create(Entities.Task task);
        bool Update(Entities.Task task);
        bool Delete(int id);
        bool PermanentDelete(int id);
        int Count();
        List<TaskViewModel> GetTaskDetails(int routineId);
    }
}
