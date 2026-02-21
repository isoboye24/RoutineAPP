using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Entities
{
    public class Month
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        private Month() { }

        public Month(string name)
        {
            Name = name;
        }

        public static Month Rehydrate(int id, string name)
        {
            return new Month(name)
            {
                Id = id
            };
        }
    }
}
