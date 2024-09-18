using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Extension;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        private Expression<Func<Category, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "name" => card => card.Name == null ? card.Id : card.Name,
                _ => card => card.Id,
            };
        }

        public async Task<PagedResultResponse<Category>> GetPagedCategory(QueryPagedCategory request)
        {
            var query = _context.Categories.AsQueryable();
            query = query.ApplyPagedCategoryFilter(request);
            //Sort
            query = request.OrderByDesc ? query.OrderByDescending(GetSortProperty(request.SortColumn))
                                        : query.OrderBy(GetSortProperty(request.SortColumn));
            //Paging
            return await query.ToPagedResultResponseAsync(request.PageNumber, request.PageSize);
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }
        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}
