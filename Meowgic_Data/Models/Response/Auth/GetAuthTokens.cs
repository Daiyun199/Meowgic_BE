﻿using Meowgic.Data.Entities;
using Meowgic.Shares.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.Auth
{
    public class GetAuthTokens
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public Roles Role {  get; set; }               
        public string Status { get; set; } = null!;
        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }
    }
}
