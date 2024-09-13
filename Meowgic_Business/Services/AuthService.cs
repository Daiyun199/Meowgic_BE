using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response;
using Meowgic.Shares.Enum;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;

        public AuthService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
        }

        public async Task<GetAuthTokens> Login(Login loginDto)
        {
            var account = await _unitOfWork.GetAccountRepository().FindOneAsync(a => a.Email == loginDto.Email
            && a.Password == HashPassword(loginDto.Password));

            if (account is null)
            {
                throw new UnauthorizedException("Wrong email or password");
            }

            string accessToken = _serviceFactory.GetTokenService().GenerateAccessToken(account.Id, account.Role);
            string refreshToken = _serviceFactory.GetTokenService().GenerateRefreshToken();

            return new GetAuthTokens
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }

        public async Task Register(Register registerDto)
        {
            var accountWithEmail = await _unitOfWork.GetAccountRepository().FindOneAsync(a => a.Email == registerDto.Email);
            if (accountWithEmail is not null)
            {
                throw new BadRequestException($"Account with email {registerDto.Email} is already exists");
            }

            var account = new Account();
            account.Email = registerDto.Email;
            account.Password = registerDto.Password;
            account.Name = registerDto.Name;
            account.Phone = registerDto.Phone;
            account.Gender = registerDto.Gender;
            if (registerDto.Dob != null)
            {
                DateTime? dateTime = registerDto.Dob;
                account.Dob = dateTime.HasValue ? new DateOnly(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day) : null;
            }
            
            account.Role = Roles.Customer.ToString();
            account.Premium = false;
            account.IsDeleted = false;
            account.isConfirmed = true;
            account.EmailConfirmed = true;
            account.PhoneNumberConfirmed = true;
            account.TwoFactorEnabled = true;
            account.LockoutEnabled = true;
            account.Status = UserStatus.Active.ToString();
            account.Password = HashPassword(registerDto.Password);

            await _unitOfWork.GetAccountRepository().AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            // Convert the password string to bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Compute the hash
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            // Convert the hash to a hexadecimal string
            string hashedPassword = string.Concat(hashBytes.Select(b => $"{b:x2}"));

            return hashedPassword;
        }
    }
}
