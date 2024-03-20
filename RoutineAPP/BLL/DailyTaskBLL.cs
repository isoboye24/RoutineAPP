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
    public class DailyTaskBLL : IBLL<DailyTaskDTO, DailyTaskDetailDTO>
    {
        DailyTaskDAO dao = new DailyTaskDAO();
        MonthDAO monthDAO = new MonthDAO();
        public bool Delete(DailyTaskDetailDTO entity)
        {
            throw new NotImplementedException();
        }

        public bool GetBack(DailyTaskDetailDTO entity)
        {
            throw new NotImplementedException();
        }

        public bool Insert(DailyTaskDetailDTO entity)
        {
            DAILY_ROUTINE dailyTask = new DAILY_ROUTINE();
            dailyTask.day = entity.Day;
            dailyTask.monthID = entity.MonthID;
            dailyTask.year = entity.Year;
            dailyTask.routineDate = entity.RoutineDate;
            dailyTask.summary = entity.Summary;
            return dao.Insert(dailyTask);
        }

        public int TotalRoutine()
        {
            return dao.TotalRoutine();
        }
        public DailyTaskDTO Select()
        {
            DailyTaskDTO dto = new DailyTaskDTO();
            dto.Months = monthDAO.Select();
            dto.DailyRoutines = dao.Select();
            return dto;
        }

        public bool Update(DailyTaskDetailDTO entity)
        {
            DAILY_ROUTINE dailyTask = new DAILY_ROUTINE();
            dailyTask.dailyRoutineID = entity.DailyTaskID;
            dailyTask.day = entity.Day;
            dailyTask.monthID = entity.MonthID;
            dailyTask.year = entity.Year;
            dailyTask.routineDate = entity.RoutineDate;
            dailyTask.summary = entity.Summary;
            return dao.Update(dailyTask);
        }
    }
}
