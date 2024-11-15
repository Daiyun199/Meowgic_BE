using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.CardMeaning;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.Order;
using Meowgic.Data.Models.Request.Promotion;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Request.Service;
using Meowgic.Data.Models.Response;
using Meowgic.Shares.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Extension
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResultResponse<T>> ToPagedResultResponseAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        where T : class
        {
            return new PagedResultResponse<T>
            {
                TotalCount = await query.CountAsync(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public static IQueryable<Card> ApplyPagedCardFilter(this IQueryable<Card> query, QueryPagedCard request)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(s => s.Name != null && s.Name.ToLower().Contains(request.Name.ToLower()));
            }
            return query;
        }
        public static IQueryable<CardMeaning> ApplyPagedCardMeaningFilter(this IQueryable<CardMeaning> query, QueryPagedCardMeaning request)
        {
            if (!string.IsNullOrEmpty(request.CardId))
            {
                query = query.Where(s => s.CardId != null && s.CardId.ToLower().Contains(request.CardId.ToLower()));
            }
            if (!string.IsNullOrEmpty(request.CategoryId))
            {
                query = query.Where(s => s.CategoryId != null && s.CategoryId.ToLower().Contains(request.CategoryId.ToLower()));
            }
            return query;
        }
        public static IQueryable<Category> ApplyPagedCategoryFilter(this IQueryable<Category> query, QueryPagedCategory request)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(s => s.Name != null && s.Name.ToLower().Contains(request.Name.ToLower()));
            }
            return query;
        }
        public static IQueryable<Question> ApplyPagedQuestionFilter(this IQueryable<Question> query, QueryPagedQuestion request)
        {
            if (!string.IsNullOrEmpty(request.Desciption))
            {
                query = query.Where(s => s.Description != null && s.Description.ToLower().Contains(request.Desciption.ToLower()));
            }
            return query;
        }
        public static IQueryable<Promotion> ApplyPagedPromotionFilter(this IQueryable<Promotion> query, QueryPagedPromotion request)
        {
            if (!string.IsNullOrEmpty(request.Description))
            {
                query = query.Where(s => s.Description != null && s.Description.ToLower().Contains(request.Description.ToLower()));
            }
            return query;
        }
        //public static IQueryable<TarotService> ApplyPagedTarotServiceFilter(this IQueryable<TarotService> query, QueryPagedService request)
        //{
        //    if (!string.IsNullOrEmpty(request.Name))
        //    {
        //        query = query.Where(s => s.Name != null && s.Name.ToLower().Contains(request.Name.ToLower()));
        //    }
        //    if (!string.IsNullOrEmpty(request.Description))
        //    {
        //        query = query.Where(s => s.Description != null && s.Description.ToLower().Contains(request.Description.ToLower()));
        //    }
        //    return query;
        //}
        public static IQueryable<Order> ApplyPagedOrdersFilter(this IQueryable<Order> query, QueryPageOrder request)
        {
            query = query.Where(o => o.Status != OrderStatus.Incart.ToString());

            if (request.StartDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= request.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(request.AccountId))
            {
                query = query.Where(o => o.AccountId == request.AccountId);
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(o => request.Status == o.Status);
            }
            return query;
        }
    }
}
