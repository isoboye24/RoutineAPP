using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.DAL.DAO
{
    public class TaskDAO : APPContext, IDAO<TaskDetailDTO, TASK>
    {
        public bool Delete(TASK entity)
        {
            throw new NotImplementedException();
        }

        public bool GetBack(int ID)
        {
            throw new NotImplementedException();
        }

        public bool Insert(TASK entity)
        {
            throw new NotImplementedException();
        }

        public List<TaskDetailDTO> Select()
        {
            throw new NotImplementedException();
        }

        public bool Update(TASK entity)
        {
            throw new NotImplementedException();
        }
    }
}
