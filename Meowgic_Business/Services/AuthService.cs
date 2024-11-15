using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Account;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Data.Models.Response.Auth;
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
    public class AuthService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IServiceFactory _serviceFactory = serviceFactory;

        public async Task<GetAuthTokens?> Login(Login loginDto)
        {
            var account = await _unitOfWork.GetAccountRepository.FindOneAsync(a => a.Email == loginDto.Email
            && a.Password == HashPassword(loginDto.Password));

            if (account is not null)
            {
                string accessToken = _serviceFactory.GetTokenService.GenerateAccessToken(account.Id, account.Role, account.Name);
                string refreshToken = _serviceFactory.GetTokenService.GenerateRefreshToken();

                return new GetAuthTokens
                {
                    Status = account.Status,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }

            return null;

        }
        public async Task<GetAuthTokens?> LoginWithoutPassword(string email)
        {
            var account = await _unitOfWork.GetAccountRepository.FindOneAsync(a => a.Email == email);

            if (account is not null)
            {
                string accessToken = _serviceFactory.GetTokenService.GenerateAccessToken(account.Id, account.Role, account.Name);
                string refreshToken = _serviceFactory.GetTokenService.GenerateRefreshToken();

                return new GetAuthTokens
                {
                    Status = account.Status,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }

            return null;

        }

        public async Task<Register?> Register(Register registerDto)
        {
            var accountWithEmail = await _unitOfWork.GetAccountRepository.FindOneAsync(a => a.Email == registerDto.Email);
            if (accountWithEmail is not null)
            {
                return null;
            }

            var account = new Account
            {
                Email = registerDto.Email,
                Password = registerDto.Password,
                Name = registerDto.Name,
                Phone = registerDto.Phone,
                Gender = registerDto.Gender,
                Role = registerDto.Roles 
            };
            if (registerDto.Dob != null)
            {
                DateTime? dateTime = registerDto.Dob;
                account.Dob = dateTime.HasValue ? new DateOnly(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day) : null;
            }
            
            //account.Role = Roles.Customer.ToString();
            account.Premium = false;
            account.IsDeleted = false;
            account.isConfirmed = false;
            account.EmailConfirmed = false;
            account.PhoneNumberConfirmed = true;
            account.TwoFactorEnabled = true;
            account.LockoutEnabled = true;
            account.Status = UserStatus.Active.ToString();
            account.Password = HashPassword(registerDto.Password);

            await _unitOfWork.GetAccountRepository.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            return registerDto;
        }
        public async Task<GetAuthTokens?> RegisterByGG(RegisterByGG request)
        {
            var accountWithEmail = await _unitOfWork.GetAccountRepository.FindOneAsync(a => a.Email == request.Email);
            if (accountWithEmail is not null)
            {
                return null;
            }

            var account = new Account
            {
                Email = request.Email,
                Name = request.Name,
                Gender = request.Gender
            };
            if (request.Dob != null)
            {
                DateTime? dateTime = request.Dob;
                account.Dob = dateTime.HasValue ? new DateOnly(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day) : null;
            }
            if (request.Phone != null)
            {
                account.Phone = request.Phone;
            }
            account.Role = Roles.Customer;
            account.Premium = false;
            account.IsDeleted = false;
            account.isConfirmed = false;
            account.EmailConfirmed = false;
            account.PhoneNumberConfirmed = true;
            account.TwoFactorEnabled = true;
            account.LockoutEnabled = true;
            account.Status = UserStatus.Active.ToString();

            await _unitOfWork.GetAccountRepository.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            string accessToken = _serviceFactory.GetTokenService.GenerateAccessToken(account.Id, account.Role, account.Name);
            string refreshToken = _serviceFactory.GetTokenService.GenerateRefreshToken();

            return new GetAuthTokens { 
                Status = account.Status,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AccountResponse?> GetAuthAccountInfo(ClaimsPrincipal claims)
        {
            var accountId = claims.FindFirst(c => c.Type == "aid")?.Value;

            if (accountId is null)
            {
                throw new UnauthorizedException("Unauthorized ");
            }

            var account = await _unitOfWork.GetAccountRepository.FindOneAsync(a => a.Id == accountId);

            if (account is null)
            {
                return null;
            }

            return account.Adapt<AccountResponse>();
        }


        private static string HashPassword(string password)
        {
            // Convert the password string to bytes
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Compute the hash
            byte[] hashBytes = SHA256.HashData(passwordBytes);

            // Convert the hash to a hexadecimal string
            string hashedPassword = string.Concat(hashBytes.Select(b => $"{b:x2}"));

            return hashedPassword;
        }
    }
}
