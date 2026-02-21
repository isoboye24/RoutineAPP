using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Core.Entities
{
    public class Category
    {

        public int Id { get; private set; }
        public string Name { get; private set; }

        public Category(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty");

            Name = name.Trim();
        }

        public static Category Rehydrate(int id, string name)
        {
            return new Category(name)
            {
                Id = id
            };
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Category name cannot be empty");

            Name = newName.Trim();
        }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}
