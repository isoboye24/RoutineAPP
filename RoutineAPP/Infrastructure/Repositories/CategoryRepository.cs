using RoutineAPP.Core.Entities;
using RoutineAPP.Application.Interfaces;
using RoutineAPP.Infrastructure.Data;
using System;
using System.Linq;

namespace RoutineAPP.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly RoutineDBEntities _db;
        public CategoryRepository(RoutineDBEntities db)
        {
            _db = db;
        }

        public IQueryable<CATEGORY> GetAll()
        {
            return _db.CATEGORies.Where(x => !x.isDeleted);
        }
        
        public IQueryable<CATEGORY> GetAllDeletedCategories()
        {
            return _db.CATEGORies.Where(x => x.isDeleted);
        }

        public IQueryable<CATEGORY> GetById(int id)
        {
            return _db.CATEGORies.Where(x => !x.isDeleted && x.categoryID == id);
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

        public bool PermanentDelete(int id)
        {
            var entity = _db.CATEGORies.FirstOrDefault(x => x.categoryID == id);

            if (entity == null)
                return false;

            _db.CATEGORies.Remove(entity);
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
