using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoutineAPP.DAL.DAO
{
    public class TaskDAO : APPContext, IDAO<TaskDetailDTO, TASK>
    {
        public bool Delete(TASK entity)
        {
            try
            {
                TASK task = db.TASKs.First(x=>x.taskID == entity.taskID);
                task.isDeleted = true;
                task.deletedDate = DateTime.Today;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetBack(int ID)
        {
            throw new NotImplementedException();
        }

        public int TotalTasks(int ID)
        {
            try
            {
                int total = db.TASKs.Count(x => x.isDeleted == false && x.dailiyRoutineID == ID);
                return total;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public decimal TotalUsedHours(int ID)
        {
            try
            {
                List<decimal> totalUsedHours = new List<decimal>();
                int taskCount = db.TASKs.Count(x => x.isDeleted == false && x.dailiyRoutineID == ID);
                if (taskCount > 0)
                {
                    var usedHours = db.TASKs.Where(x => x.isDeleted == false && x.dailiyRoutineID == ID);
                    foreach (var time in usedHours)
                    {
                        totalUsedHours.Add(time.timeSpent);
                    }
                    decimal total = totalUsedHours.Sum();                    
                    return total;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(TASK entity)
        {
            try
            {
                db.TASKs.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<TaskDetailDTO> Select(int ID)
        {
            try
            {
                List<TaskDetailDTO> tasks = new List<TaskDetailDTO>();
                var list = (from t in db.TASKs.Where(x => x.isDeleted == false && x.dailiyRoutineID== ID)
                            join m in db.MONTHs on t.monthID equals m.monthID
                            join c in db.CATEGORies.Where(x => x.isDeleted == false) on t.categoryID equals c.categoryID
                            select new
                            {
                                taskID = t.taskID,
                                categoryID = t.categoryID,
                                categoryName = c.categoryName,
                                timeSpent = t.timeSpent,
                                day = t.day,
                                monthID = t.monthID,
                                monthName = m.monthName,
                                year = t.year,
                                dailyRoutineID = t.dailiyRoutineID,
                            }).OrderByDescending(x => x.year).ThenByDescending(x => x.monthID).ThenByDescending(x => x.day).ThenBy(x => x.categoryName).ToList();
                foreach (var item in list)
                {
                    TaskDetailDTO dto = new TaskDetailDTO();
                    dto.TaskID = item.taskID;
                    dto.CategoryID = item.categoryID;
                    dto.CategoryName = item.categoryName;
                    dto.TimeSpent = item.timeSpent;
                    dto.Day = item.day;
                    dto.MonthID = item.monthID;
                    dto.MonthName = item.monthName;
                    dto.Year = item.year;
                    dto.DailyRoutineID = item.dailyRoutineID;
                    tasks.Add(dto);
                }
                return tasks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TaskDetailDTO> Select()
        {
            throw new NotImplementedException();
        }

        public bool Update(TASK entity)
        {
            try
            {
                TASK task = db.TASKs.First(x=>x.taskID==entity.taskID);                
                task.categoryID = entity.categoryID;
                task.timeSpent = entity.timeSpent;
                task.day = entity.day;
                task.monthID = entity.monthID;
                task.year = entity.year;
                task.dailiyRoutineID = entity.dailiyRoutineID;
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
