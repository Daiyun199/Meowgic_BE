﻿using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IAccountService
    {
        Task UpdateCustomerInfo(string id, UpdateAccount request);
        Task<AccountResponse> GetCustomerInfo(string id);
    }
}
