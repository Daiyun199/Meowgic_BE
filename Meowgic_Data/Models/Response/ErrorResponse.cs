using Meowgic.Shares.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Models.Response
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.InternalServerError;

        [EnumDataType(typeof(ErrorType))]
        public required string ErrorType { get; set; }

        public required string ErrorMessage { get; set; }

        public string? StackTrace { get; set; }
    }
}
