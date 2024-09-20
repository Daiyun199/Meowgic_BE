using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(string id);
        Task<Category> CreateCategoryAsync(CategoryRequestDTO category);
        Task<Category?> UpdateCategoryAsync(string id, CategoryRequestDTO category);
        Task<bool> DeleteCategoryAsync(string id);
    }
}
