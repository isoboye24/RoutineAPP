using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly RoutineDBEntities _db;
        public CategoryRepository(RoutineDBEntities db)
        {
            _db = db;
        }
        public List<Category> GetAll()
        {
            return _db.CATEGORies
                .Where(x => !x.isDeleted)
                .OrderBy(x => x.categoryName)
                .ToList()
                .Select(x => Category.Rehydrate(x.categoryID, x.categoryName))
                .ToList();
        }

        public Category GetById(int id)
        {
            var entity = _db.CATEGORies.FirstOrDefault(x => x.categoryID == id && !x.isDeleted);
            if (entity == null) return null;

            var category = new Category(entity.categoryName);
            category.SetId(entity.categoryID);
            return category;
        }

        public bool Insert(Category category)
        {
            _db.CATEGORies.Add(new CATEGORY
            {
                categoryName = category.Name
            });

            _db.SaveChanges();
            return true;
        }

        public bool Update(Category category)
        {
            var entity = _db.CATEGORies.First(x => x.categoryID == category.Id);
            entity.categoryName = category.Name;
            _db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var entity = _db.CATEGORies.First(x => x.categoryID == id);
            entity.isDeleted = true;
            entity.deletedDate = DateTime.Today;
            _db.SaveChanges();
            return true;
        }

        public bool Exists(string name)
        {
            return _db.CATEGORies.Any(x =>
                !x.isDeleted &&
                x.categoryName == name);
        }

        public int Count()
        {
            return _db.CATEGORies.Count(x => !x.isDeleted);
        }
    }
}
