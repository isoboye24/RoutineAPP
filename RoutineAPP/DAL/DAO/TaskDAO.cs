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
        
        public List<TaskDetailDTO> Select()
        {
            try
            {
                List<TaskDetailDTO> tasks = new List<TaskDetailDTO>();
                var list = (from t in db.TASKs.Where(x => x.isDeleted == false)
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
                    tasks.Add(dto);
                }
                return tasks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
