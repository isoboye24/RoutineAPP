using RoutineAPP.Core.Entities;
using RoutineAPP.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Application.Interfaces
{
    public interface ITaskService
    {
        List<Core.Entities.Task> GetAll(int routineId);
        bool Create(Core.Entities.Task task);
        bool Update(Core.Entities.Task task);
        bool Delete(int id);
        bool PermanentDelete(int id);
        int Count();
        List<TaskDTO> GetTaskDetails(int routineId);
    }
}
