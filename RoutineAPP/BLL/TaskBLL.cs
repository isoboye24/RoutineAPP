using RoutineAPP.DAL.DAO;
using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.BLL
{
    public class TaskBLL : IBLL<TaskDTO, TaskDetailDTO>
    {
        MonthDAO monthDAO = new MonthDAO();
        public bool Delete(TaskDetailDTO entity)
        {
            throw new NotImplementedException();
        }

        public bool GetBack(TaskDetailDTO entity)
        {
            throw new NotImplementedException();
        }

        public bool Insert(TaskDetailDTO entity)
        {
            throw new NotImplementedException();
        }

        public TaskDTO Select()
        {
            TaskDTO dto = new TaskDTO();
            dto.Months = monthDAO.Select();
            return dto;
        }

        public bool Update(TaskDetailDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
