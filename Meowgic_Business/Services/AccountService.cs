using Firebase.Auth;
using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Data.Repositories;
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
            var account = await _unitOfWork.GetAccountRepository().FindOneAsync(a => a.Id == id);

            if (account is null)
            {
                throw new UnauthorizedException("Account not found");
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
            if (request.Role != null)
            {
                account.Role = request.Role;
            }
            await _unitOfWork.GetAccountRepository().UpdateAsync(account);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<AccountResponse> GetCustomerInfo(string id)
        {
            var account = await _unitOfWork.GetAccountRepository().FindOneAsync(a => a.Id == id);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            //string pass = HashPassword(account.Password);
            //account.Password = pass;

            return account.Adapt<AccountResponse>();
        }
    }
}
