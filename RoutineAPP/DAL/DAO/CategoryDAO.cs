using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DAO
{
    public class CategoryDAO : APPContext, IDAO<CategoryDetailDTO, CATEGORY>
    {
        public bool Delete(CATEGORY entity)
        {
            throw new NotImplementedException();
        }

        public bool GetBack(int ID)
        {
            throw new NotImplementedException();
        }

        public bool Insert(CATEGORY entity)
        {
            throw new NotImplementedException();
        }

        public List<CategoryDetailDTO> Select()
        {
            throw new NotImplementedException();
        }

        public bool Update(CATEGORY entity)
        {
            throw new NotImplementedException();
        }
    }
}
