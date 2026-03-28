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
        List<TaskDTO> GetTasksByDay(int routine);
        List<TaskDTO> GetAllDeletedTasks();
        bool Create(Core.Entities.Task task);
        bool Update(Core.Entities.Task task);
        bool Delete(int id);
        bool PermanentDelete(int id);
        int Count();

        List<TaskDTO> GetTasksByMonth(int month, int year);
        List<TaskDTO> GetTasksByYear(int year);
        List<TaskDTO> GetAll();
    }
}
