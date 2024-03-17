using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoutineAPP.DAL.DAO
{
    public class CategoryDAO : APPContext, IDAO<CategoryDetailDTO, CATEGORY>
    {
        public bool Delete(CATEGORY entity)
        {
            try
            {
                CATEGORY category = db.CATEGORies.First(x=>x.categoryID==entity.categoryID);
                category.isDeleted = true;
                category.deletedDate = DateTime.Today;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetBack(int ID)
        {
            throw new NotImplementedException();
        }

        public bool Insert(CATEGORY entity)
        {
            try
            {
                db.CATEGORies.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CategoryDetailDTO> Select()
        {
            try
            {
                List<CategoryDetailDTO> categories = new List<CategoryDetailDTO>();
                var list = db.CATEGORies.Where(x => x.isDeleted == false).OrderBy(x=>x.categoryName).ToList();
                foreach (var item in list)
                {
                    CategoryDetailDTO dto = new CategoryDetailDTO();
                    dto.CategoryID = item.categoryID;
                    dto.CategoryName = item.categoryName;
                    categories.Add(dto);
                }
                return categories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CATEGORY entity)
        {
            try
            {
                CATEGORY category = db.CATEGORies.First(x=>x.categoryID == entity.categoryID);
                category.categoryName = entity.categoryName;               
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
