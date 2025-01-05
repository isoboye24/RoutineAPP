using RoutineAPP.DAL.DAO;
using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.BLL
{
    public class GraphBLL
    {
        MonthDAO monthDAO = new MonthDAO();
        YearDAO yearDAO = new YearDAO();
        CategoryDAO categoryDAO = new CategoryDAO();
        public GraphDTO Select()
        {
            GraphDTO dto = new GraphDTO();
            dto.Months = monthDAO.Select();
            dto.Years = yearDAO.Select();
            dto.Categories = categoryDAO.Select();
            return dto;
        }
    }
}
