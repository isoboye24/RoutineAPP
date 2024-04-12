using RoutineAPP.DAL.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.BLL
{
    public class DashboardBLL
    {
        CategoryDAO categoryDAO = new CategoryDAO();
        TaskDAO task = new TaskDAO();
        public int SelectTotalCategory()
        {
            return categoryDAO.TotalCategory();
        }
        public string SelectTimeInMonth(int month, string category)
        {
            return task.SelectTimeInMonth(month, category);
        }
        public string SelectTimeInYear(int year, string category)
        {
            return task.SelectTimeInYear(year, category);
        }
    }
}
