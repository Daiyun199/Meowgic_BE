using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Extension;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Promotion;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response;
using Meowgic.Shares.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Repositories
{
    public class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
    {
        private readonly AppDbContext _context;

        public PromotionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        private Expression<Func<Promotion, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "description" => promotion => promotion.Description == null ? promotion.Id : promotion.Description,
                _ => promotion => promotion.Id,
            };
        }

        public async Task<PagedResultResponse<Promotion>> GetPagedPromotion(QueryPagedPromotion request)
        {
            var query = _context.Promotions.AsQueryable();
            query = query.ApplyPagedPromotionFilter(request);
            //Sort
            query = request.OrderByDesc ? query.OrderByDescending(GetSortProperty(request.SortColumn))
                                        : query.OrderBy(GetSortProperty(request.SortColumn));
            //Paging
            return await query.ToPagedResultResponseAsync(request.PageNumber, request.PageSize);
        }

        public async Task<List<Promotion>> GetAll()
        {
            return await _context.Promotions.Where(pr => pr.Status == PromotionStatus.Active.ToString()).ToListAsync();
        }
    }
}
