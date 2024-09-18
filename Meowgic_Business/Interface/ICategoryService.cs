using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface ICategoryService
    {
        Task<PagedResultResponse<Category>> GetPagedCategory(QueryPagedCategory request);

        Task<Category> CreateCategory(CategoryRequest request);

        Task UpdateCategory(string id, CategoryRequest request);

        Task<bool> DeleteCategoryAsync(string id);
        Task<List<CategoryResponse>> GetAll();
    }
}
