using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DAO
{
    public class YearDAO : APPContext
    {
        public List<AllYearsDetailDTO> Select()
        {
            try
            {
                List<AllYearsDetailDTO> AllYears = new List<AllYearsDetailDTO>();
                List<int> yearsCollection = new List<int>();
                List<int> years = new List<int>();

                var tasksYear = db.TASKs.Where(x => x.isDeleted == false).ToList();
                foreach (var item in tasksYear)
                {
                    yearsCollection.Add(item.year);
                }
                years = yearsCollection.Distinct().OrderByDescending(year => year).ToList();
                foreach (var yearItem in years)
                {
                    AllYearsDetailDTO dto = new AllYearsDetailDTO();
                    dto.YearID += 1;
                    dto.Year = yearItem.ToString();
                    AllYears.Add(dto);
                }
                return AllYears;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
