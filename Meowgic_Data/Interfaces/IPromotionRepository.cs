using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Promotion;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IPromotionRepository : IGenericRepository<Promotion>
    {
        Task<PagedResultResponse<Promotion>> GetPagedPromotion(QueryPagedPromotion request);
        Task<List<Promotion>> GetAll();
    }
}
