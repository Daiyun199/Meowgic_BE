using Meowgic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface ICategoryRepository
    {
        //Task<IEnumerable<Category>> GetAllCategoriesAsync();
        //Task<Category?> GetCategoryByIdAsync(string id);
        //Task<Category> CreateCategoryAsync(Category category);
        //Task<Category?> UpdateCategoryAsync(string id, Category category);
        //Task<bool> DeleteCategoryAsync(string id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(string id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category?> UpdateCategoryAsync(string id, Category category);
        Task<bool> DeleteCategoryAsync(string id);
    }
}
