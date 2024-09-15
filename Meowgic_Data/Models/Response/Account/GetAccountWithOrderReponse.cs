using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.Account
{
    public class OrderDetailResponseModel
    {
        public string Id { get; set; } = null!;
        public double? Rate { get; set; }

        public string? Feedback { get; set; }

        public  TarotSeviceResponseModel TarotService { get; set; } = null!;

    }
    public class TarotSeviceResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public PromotionResponseModel Promotion { get; set; } = null!;
        public AccountResponseModel Reader { get; set; } = null!;

    }
    public class PromotionResponseModel
    {
        public string Id { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double DiscountPercent { get; set; }
        public double MaxDiscount { get; set; }
        public DateTime ExpireTime { get; set; }
        public string? Status { get; set; }
    }

    public class OrderResponseModel
    {
        public string Id { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public AccountResponseModel Customer { get; set;} = null!;
        public List<OrderDetailResponseModel> OrderDetails { get; set; } = new List<OrderDetailResponseModel>();
    }

    public class AccountResponseModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateOnly? Dob { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
        public string Role { get; set; } = null!;
        public double? Rate { get; set; }
        public bool Premium { get; set; }
        public List<OrderResponseModel> Orders { get; set; } = new List<OrderResponseModel>();
    }
    public class GetAccountWithOrderReponse
    {
        public List<AccountResponseModel> Customers { get; set; } = new List<AccountResponseModel>();

    }
}
