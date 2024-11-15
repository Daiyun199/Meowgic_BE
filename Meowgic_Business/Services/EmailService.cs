using MailKit.Net.Smtp;
using Meowgic.Business.Interface;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IUnitOfWork _unitOfWork;
        public EmailService(EmailConfiguration emailConfig, IUnitOfWork unitOfWork)
        {
            _emailConfig = emailConfig;
            _unitOfWork = unitOfWork;
        }
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Meowgic ", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(message.IsHtml ? MimeKit.Text.TextFormat.Html : MimeKit.Text.TextFormat.Text)
            {
                Text = message.Content
            };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception or both.
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        public async Task<ServiceResult<string>> SendConfirmEmailAsync(string email)
        {
            var baseDirectory = AppContext.BaseDirectory;
            var templatePath = Path.Combine(baseDirectory, "EmailTemplate", "EmailConfirm.html");
            var htmlContent = GetEmailTemplate(templatePath);

            //var userExist = await _userRepository.GetUserByEmail(email);
            var userExist = await _unitOfWork.GetAccountRepository.FindOneAsync(x => x.Email == email);
            if (userExist == null)
            {
                throw new Exception("User does not exist");
            }

            var token = userExist.VerificationToken;

            var link = $"https://meowgic.azurewebsites.net/api/Account/emailConfirm?id={userExist.Id}";
            htmlContent = htmlContent.Replace("{{UserName}}", userExist.Name).Replace("{{Link}}", link);

            var message = new Message(new string[] { userExist.Email }, "[Confirm Email] Please verify your account in Meowgic", htmlContent, true);
            SendEmail(message);

            var result = new ServiceResult<string>();
            result.Status = 1;
            result.IsSuccess = true;
            result.Data = null;
            result.ErrorMessage = "Confirm Email Successfully";

            return result;
        }
        public async Task<ServiceResult<string>> SendResetPasswordAsync(string email)
        {
            var baseDirectory = AppContext.BaseDirectory;
            var templatePath = Path.Combine(baseDirectory, "EmailTemplate", "ResetPassword.html");
            var htmlContent = GetEmailTemplate(templatePath);

            //var userExist = await _userRepository.GetUserByEmail(email);
            var userExist = await _unitOfWork.GetAccountRepository.FindOneAsync(x => x.Email == email);
            if (userExist == null)
            {
                throw new Exception("User does not exist");
            }

            var token = userExist.VerificationToken;
            var otp = GenerateRandomOTP(6);
            userExist.otpResetPassword = otp;
           
            htmlContent = htmlContent.Replace("{{UserName}}", userExist.Name).Replace("{{OTP}}", otp);

            var message = new Message(new string[] { userExist.Email }, "Reset Password for Meowgic", htmlContent, true);
            SendEmail(message);
            await _unitOfWork.GetAccountRepository.UpdateAsync(userExist);
            await _unitOfWork.SaveChangesAsync();
            var result = new ServiceResult<string>();
            result.Status = 1;
            result.IsSuccess = true;
            result.Data = null;
            result.ErrorMessage = "Confirm Email Successfully";

            return result;

        }
        public static string GenerateRandomOTP(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            Random random = new Random();
            StringBuilder otpBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                otpBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return otpBuilder.ToString();
        }
        public string GetEmailTemplate(string templatePath)
        {
            if (File.Exists(templatePath))
            {
                return File.ReadAllText(templatePath);
            }
            throw new FileNotFoundException("Email template not found", templatePath);
        }
        public async Task<ServiceResult<string>> SendPasswordEmailAsync(string email, string password)
        {
            var baseDirectory = AppContext.BaseDirectory;
            var templatePath = Path.Combine(baseDirectory, "EmailTemplate", "EmailPassword.html");
            var htmlContent = GetEmailTemplate(templatePath);

            var userExist = await _unitOfWork.GetAccountRepository.FindOneAsync(x => x.Email == email);

            if (userExist == null)
            {
                throw new Exception("User does not exist");
            }

            htmlContent = htmlContent.Replace("{{UserName}}", userExist.UserName).Replace("{{Password}}", password);

            var message = new Message(new string[] { userExist.Email }, "[Password] Your password for Meowgic Gate", htmlContent, true);
            SendEmail(message);

            var result = new ServiceResult<string>();
            result.Status = 1;
            result.IsSuccess = true;
            result.Data = null;
            result.ErrorMessage = "Password sent successfully";
            return result;
        }
    }
}
