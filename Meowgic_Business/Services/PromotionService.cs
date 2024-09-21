using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response.Question;
using Meowgic.Data.Models.Response;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meowgic.Data.Models.Request.Promotion;
using Meowgic.Shares.Enum;
using Meowgic.Data.Models.Response.Promotion;
using Firebase.Auth;

namespace Meowgic.Business.Services
{
    public class PromotionService(IUnitOfWork unitOfWork) : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PagedResultResponse<ListPromotionResponse>> GetPagedPromotion(QueryPagedPromotion request)
        {
            return (await _unitOfWork.GetPromotionRepository().GetPagedPromotion(request)).Adapt<PagedResultResponse<ListPromotionResponse>>();
        }

        public async Task<CreatePromotion> CreatePromotion(CreatePromotion request)
        {
            var promotion = new Promotion();
            promotion.Description = request.Description;
            promotion.DiscountPercent = request.DiscountPercent;
            promotion.MaxDiscount = request.MaxDiscount;
            promotion.ExpireTime = request.ExpireTime;
            promotion.Status = PromotionStatus.Active.ToString();
            promotion.CreatedBy = request.UserId;
            promotion.CreatedTime = DateTime.Now;

            await _unitOfWork.GetPromotionRepository().AddAsync(promotion);
            await _unitOfWork.SaveChangesAsync();

            return request;
        }

        public async Task<CreatePromotion> UpdatePromotion(string id, CreatePromotion request)
        {
            var promotion = await _unitOfWork.GetPromotionRepository().FindOneAsync(s => s.Id == id);

            if (promotion is not null)
            {
                promotion.Description = request.Description;
                promotion.DiscountPercent = request.DiscountPercent;
                promotion.MaxDiscount = request.MaxDiscount;
                promotion.ExpireTime = request.ExpireTime;
                promotion.LastUpdatedBy = request.UserId;
                promotion.LastUpdatedTime = DateTime.Now;

                await _unitOfWork.GetPromotionRepository().UpdateAsync(promotion);
                await _unitOfWork.SaveChangesAsync();
                return request;
            }
            else
            {
                throw new NotFoundException("Not found");
            }
        }

        public async Task<bool> DeletePromotion(string id, string? userId)
        {
            var promotion = await _unitOfWork.GetPromotionRepository().GetByIdAsync(id);
            if (promotion == null)
            {
                return false;
            }
            promotion.Status = PromotionStatus.Expired.ToString();
            if (userId != null)
            {
                promotion.DeletedBy = userId;
            }
            promotion.DeletedTime = DateTime.Now;
            var services = await _unitOfWork.GetServiceRepository().GetAllTarotServicesAsync();
            foreach (var service in services)
            {
                if (service.PromotionId == id)
                {
                    service.PromotionId = null;
                    await _unitOfWork.GetServiceRepository().UpdateTarotServiceAsync(service.Id,service);
                }
            }
            await _unitOfWork.GetPromotionRepository().UpdateAsync(promotion);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<List<PromotionResponse>> GetAll()
        {
            var promotions = (await _unitOfWork.GetPromotionRepository().GetAll());
            if (promotions != null)
            {
                return promotions.Adapt<List<PromotionResponse>>();


            }
            throw new NotFoundException("No promotion was found");
        }
    }
}
