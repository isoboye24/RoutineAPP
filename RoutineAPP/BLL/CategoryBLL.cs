using RoutineAPP.DAL;
using RoutineAPP.DAL.DAO;
using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutineAPP.BLL
{
    public class CategoryBLL : IBLL<CategoryDTO, CategoryDetailDTO>
    {
        CategoryDAO dao = new CategoryDAO();
        public bool Delete(CategoryDetailDTO entity)
        {
            CATEGORY category = new CATEGORY();
            category.categoryID = entity.CategoryID;
            return dao.Delete(category);
        }

        public bool GetBack(CategoryDetailDTO entity)
        {
            throw new NotImplementedException();
        }

        public bool Insert(CategoryDetailDTO entity)
        {
            CATEGORY category = new CATEGORY();
            category.categoryName = entity.CategoryName;
            return dao.Insert(category);
        }

        public int TotalCategory()
        {
            return dao.TotalCategory();
        }

        public CategoryDTO Select()
        {
            CategoryDTO dto = new CategoryDTO();
            dto.Categories = dao.Select();
            return dto;
        }

        public bool Update(CategoryDetailDTO entity)
        {
            CATEGORY category = new CATEGORY();
            category.categoryID = entity.CategoryID;
            category.categoryName = entity.CategoryName;
            return dao.Update(category);
        }
    }
}
