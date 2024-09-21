using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface ICategoryService
    {
        Task<PagedResultResponse<ListCategoryResponse>> GetPagedCategory(QueryPagedCategory request);

        Task<Category> GetDetailCategory(string id);
        Task<CategoryRequest> CreateCategory(CategoryRequest request);

        Task<CategoryRequest> UpdateCategory(string id, CategoryRequest request);

        Task<bool> DeleteCategoryAsync(string id, string userId);
        Task<List<CategoryResponse>> GetAll();
    }
}
