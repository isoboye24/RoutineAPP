using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoutineAPP.DAL.DAO
{
    public class DailyTaskDAO : APPContext, IDAO<DailyTaskDetailDTO, DAILY_ROUTINE>
    {
        public bool Delete(DAILY_ROUTINE entity)
        {
            throw new NotImplementedException();
        }

        public bool GetBack(int ID)
        {
            throw new NotImplementedException();
        }

        public bool Insert(DAILY_ROUTINE entity)
        {
            try
            {
                db.DAILY_ROUTINE.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalRoutine()
        {
            try
            {
                int total = db.DAILY_ROUTINE.Count(x=>x.isDeleted==false);
                return total;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DailyTaskDetailDTO> Select()
        {
            try
            {
                List<DailyTaskDetailDTO> dailyTasks = new List<DailyTaskDetailDTO>();
                var list = (from d in db.DAILY_ROUTINE.Where(x => x.isDeleted == false)
                            join m in db.MONTHs on d.monthID equals m.monthID
                            select new
                            {
                                dailyRoutineID = d.dailyRoutineID,
                                summary = d.summary,
                                routineDate = d.routineDate,
                                day = d.day,
                                monthID = d.monthID,
                                monthName = m.monthName,
                                year = d.year,
                            }).OrderByDescending(x => x.year).ThenByDescending(x => x.monthID).ThenByDescending(x => x.day).ToList();
                foreach (var item in list)
                {
                    DailyTaskDetailDTO dto = new DailyTaskDetailDTO();
                    dto.DailyTaskID = item.dailyRoutineID;
                    dto.Summary = item.summary;
                    dto.RoutineDate = item.routineDate;
                    dto.Day = item.day;
                    dto.MonthID = item.monthID;
                    dto.MonthName = item.monthName;
                    dto.Year = item.year;
                    dailyTasks.Add(dto);
                }
                return dailyTasks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(DAILY_ROUTINE entity)
        {
            try
            {
                DAILY_ROUTINE dailyTask = db.DAILY_ROUTINE.First(x=>x.dailyRoutineID == entity.dailyRoutineID);
                dailyTask.dailyRoutineID = entity.dailyRoutineID;
                dailyTask.day = entity.day;
                dailyTask.monthID = entity.monthID;
                dailyTask.year = entity.year;
                dailyTask.routineDate = entity.routineDate;
                dailyTask.summary = entity.summary;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
