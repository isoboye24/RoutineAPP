using RoutineAPP.DAL.DAO;
using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.BLL
{
    public class YearBLL
    {
        YearDAO dao = new YearDAO();
        public YearDTO Select()
        {
            YearDTO dto = new YearDTO();
            dto.Years = dao.Select();
            return dto;
        }
    }
}
