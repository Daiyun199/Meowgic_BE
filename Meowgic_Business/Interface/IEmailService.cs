using Meowgic.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IEmailService
    {
        void SendEmail(Message message);
        string GetEmailTemplate(string template);
        Task<ServiceResult<string>> SendConfirmEmailAsync(string email);
        Task<ServiceResult<string>> SendPasswordEmailAsync(string email, string password);
        Task<ServiceResult<string>> SendResetPasswordAsync(string email);
    }
}
