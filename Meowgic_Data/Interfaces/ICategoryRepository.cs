using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<PagedResultResponse<Category>> GetPagedCategory(QueryPagedCategory request);
        void Update(Category category);
        Task<List<Category>> GetAll();
    }
}
