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
        public string SelectCategoryInMonth(int month, int year, string category)
        {
            return task.SelectCategoryInMonth(month, year, category);
        }
        public string SelectCategoryInYear(int year, string category)
        {
            return task.SelectCategoryInYear(year, category);
        }
    }
}
