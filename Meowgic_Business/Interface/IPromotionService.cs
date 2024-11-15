using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response.Question;
using Meowgic.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meowgic.Data.Models.Request.Promotion;
using Meowgic.Data.Models.Response.Promotion;
using System.Security.Claims;

namespace Meowgic.Business.Interface
{
    public interface IPromotionService
    {
        Task<PagedResultResponse<ListPromotionResponse>> GetPagedPromotion(QueryPagedPromotion request);

        Task<CreatePromotion> CreatePromotion(CreatePromotion request, ClaimsPrincipal claim);

        Task<CreatePromotion> UpdatePromotion(string id, CreatePromotion request, ClaimsPrincipal claim);

        Task<bool> DeletePromotion(string id, ClaimsPrincipal? calim);
        Task<List<PromotionResponse>> GetAll();
    }
}
