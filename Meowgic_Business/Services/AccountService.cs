using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Data.Repositories;
using Meowgic.Shares.Enum;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            string hashedPassword = string.Concat(hashBytes.Select(b => $"{b:x2}"));

            return hashedPassword;
        }

        public async Task UpdateCustomerInfo(string id, UpdateAccount request)
        {
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(id);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }

            if (request.Name != null)
            {
                account.Name = request.Name;
            }
            if (request.Password != null)
            {
                account.Password = HashPassword(request.Password);
            }
            if (request.Dob != null)
            {
                account.Dob = new DateOnly(request.Dob.Value.Year, request.Dob.Value.Month, request.Dob.Value.Day);
            }
            if (request.Gender != null)
            {
                account.Gender = request.Gender;
            }
            if (request.Phone != null)
            {
                account.Phone = request.Phone;
            }
            if (string.IsNullOrEmpty(account.Role.ToString()))
            {
                account.Role = request.Role;
            }
            account.LastUpdatedTime = DateTime.Now;

            await _unitOfWork.GetAccountRepository.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> DeleteAccountAsync(string id)
        {
            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(id);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            account.Status = UserStatus.Unactive.ToString();
            account.DeletedTime = DateTime.Now;
            account.IsDeleted = true;
            await _unitOfWork.GetAccountRepository.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Account> GetCustomerInfo(string accountId)
        {
            var account = await _unitOfWork.GetAccountRepository.FindOneAsync(a => a.Id == accountId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            //string pass = HashPassword(account.Password);
            //account.Password = pass;

            return account.Adapt<Account>();
        }

        public async Task<PagedResultResponse<AccountResponse>> GetPagedAccounts(QueryPagedAccount request)
        {
            return (await _unitOfWork.GetAccountRepository.GetPagedAccount(request)).Adapt<PagedResultResponse<AccountResponse>>();
        }
        public async Task<ServiceResult<string>> ConfirmEmailUser(ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;
            var userExist = await _unitOfWork.GetAccountRepository.FindOneAsync(a => a.Id == userId);
            if (userExist != null)
            {
                userExist.EmailConfirmed = true;
                userExist.isConfirmed = true;
                await _unitOfWork.GetAccountRepository.UpdateAsync(userExist);
                await _unitOfWork.SaveChangesAsync();
            }

            var result = new ServiceResult<string>();
            result.Status = 1;
            result.IsSuccess = true;
            result.ErrorMessage = "Confirm Email Successfully";

            return result;
        }
        public async Task<ServiceResult<string>> ConfirmEmailUserProMax(string id)
        {
         
            var userExist = await _unitOfWork.GetAccountRepository.FindOneAsync(a => a.Id == id);
            if (userExist != null)
            {
                userExist.EmailConfirmed = true;
                userExist.isConfirmed = true;
                await _unitOfWork.GetAccountRepository.UpdateAsync(userExist);
                await _unitOfWork.SaveChangesAsync();
            }

            var result = new ServiceResult<string>();
            result.Status = 1;
            result.IsSuccess = true;
            result.ErrorMessage = "Confirm Email Successfully";

            return result;
        }
        public async Task<ServiceResult<string>> ResetPasswordAsync(ResetPassword resetPasswordDTO)
        {
        

            if (resetPasswordDTO.newPassword !=resetPasswordDTO.confirmPassword)
            {
                throw new BadRequestException("Passwords do not match");
            }

            // Tìm tài khoản theo email
            var account = await _unitOfWork.GetAccountRepository.FindOneAsync(a => a.Email ==resetPasswordDTO.email);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }

            if (VerifyPassword(resetPasswordDTO.newPassword, account.Password))
            {
                throw new BadRequestException("New password cannot be the same as the old password");
            }
                // Xác thực mã OTP (giả định bạn có một phương thức để thực hiện việc này)
                bool isOtpValid = await ValidateOtpAsync(resetPasswordDTO.email, resetPasswordDTO.otpResetPassword); // Bạn cần triển khai phương thức ValidateOtpAsync
            if (!isOtpValid)
            {
                throw new BadRequestException("Invalid OTP");
            }

            // Cập nhật mật khẩu
            account.Password = HashPassword(resetPasswordDTO.newPassword); // Sử dụng phương thức HashPassword đã có
            await _unitOfWork.GetAccountRepository.UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();

            var result = new ServiceResult<string>
            {
                Status = 1,
                IsSuccess = true,
                ErrorMessage = "Password reset successfully"
            };
            return result;
        }

        // Giả định bạn có phương thức để xác thực mã OTP
        private async Task<bool> ValidateOtpAsync(string email, string otp)
        {
            var account = await _unitOfWork.GetAccountRepository.FindOneAsync(a =>a.Email ==email);
            if (account.otpResetPassword != otp)
            {
                return false;
            }
            // Thực hiện kiểm tra mã OTP. 
            // Bạn có thể thêm logic xác thực mã OTP ở đây.
            // Trả về true nếu hợp lệ, false nếu không hợp lệ.
            return true; // Thay đổi theo logic xác thực của bạn
        }
        private bool VerifyPassword(string newPassword, string oldPasswordHash)
        {
            // Implement your password verification logic here
            return HashPassword(newPassword) == oldPasswordHash;
        }

            //private async Task<String> getPasswordOfAccount(string email)
            //{
            //    var account = await _unitOfWork.GetAccountRepository.FindOneAsync(a=> a.Email == email);
            //    return account.Password;
            //}
        }

    }
