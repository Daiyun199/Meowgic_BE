﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response.PayOS
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public int? Code { get; set; }
        public object? Data { get; set; }
        public string? Message { get; set; }
    }
}
