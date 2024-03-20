using RoutineAPP.DAL;
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
        CategoryDAO categoryDAO = new CategoryDAO();
        TaskDAO dao = new TaskDAO();
        public bool Delete(TaskDetailDTO entity)
        {
            TASK task = new TASK();
            task.taskID = entity.TaskID;
            return dao.Delete(task);
        }

        public bool GetBack(TaskDetailDTO entity)
        {
            throw new NotImplementedException();
        }

        public int TotalTasks(int ID)
        {
            return dao.TotalTasks(ID);
        }
        public decimal TotalUsedHours(int ID)
        {
            return dao.TotalUsedHours(ID);
        }

        public bool Insert(TaskDetailDTO entity)
        {
            TASK task = new TASK();
            task.categoryID = entity.CategoryID;
            task.timeSpent = entity.TimeSpent;
            task.day = entity.Day;
            task.monthID = entity.MonthID;
            task.year = entity.Year;
            task.dailiyRoutineID = entity.DailyRoutineID;
            return dao.Insert(task);
        }

        public TaskDTO Select()
        {
            throw new NotImplementedException();
        }
        
        public TaskDTO Select(int ID)
        {
            TaskDTO dto = new TaskDTO();
            dto.Months = monthDAO.Select();
            dto.Categories = categoryDAO.Select();
            dto.Tasks = dao.Select( ID);
            return dto;
        }

        public bool Update(TaskDetailDTO entity)
        {
            TASK task = new TASK();
            task.taskID = entity.TaskID;
            task.categoryID = entity.CategoryID;
            task.timeSpent = entity.TimeSpent;
            task.day = entity.Day;
            task.monthID = entity.MonthID;
            task.year = entity.Year;
            task.dailiyRoutineID = entity.DailyRoutineID;
            return dao.Update(task);
        }
    }
}
