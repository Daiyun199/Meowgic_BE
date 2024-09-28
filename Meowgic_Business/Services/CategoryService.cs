using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Repositories;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService( ICategoryRepository categoryRepository,IMapper mapper, IAccountRepository accountRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(string id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(CategoryRequestDTO categoryRequest, ClaimsPrincipal claim)
        {
            var accountId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(accountId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            // Chuyển đổi từ CategoryRequestDTO sang Category
            var category = _mapper.Map<Category>(categoryRequest);
            category.CreatedBy = accountId;
            category.CreatedTime = DateTime.Now;
        

            // Gọi repository để lưu category vào cơ sở dữ liệu
            return await _categoryRepository.CreateCategoryAsync(category);
        }

        public async Task<Category?> UpdateCategoryAsync(string id, CategoryRequestDTO categoryRequest, ClaimsPrincipal claim)
        {
            var accountId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(accountId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var existingCategory = _categoryRepository.GetCategoryByIdAsync(id);
            if (existingCategory is null)
            {
                throw new BadRequestException("Category not found");
            }
            var category = _mapper.Map<Category>(categoryRequest);
            category.LastUpdatedBy = accountId;
            category.LastUpdatedTime = DateTime.Now;
            return await _categoryRepository.UpdateCategoryAsync(id, category);
        }

        public async Task<bool> DeleteCategoryAsync(string id, ClaimsPrincipal claim)
        {
            var accountId = claim.FindFirst("aid")?.Value;

            var account = await _accountRepository.GetCustomerDetailsInfo(accountId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category is null)
            {
                throw new BadRequestException("Category not found");
            }
            // Có thể thêm logic nghiệp vụ nếu cần
            category.DeletedBy = accountId;
            category.DeletedTime = DateTime.Now;
            await _categoryRepository.UpdateCategoryAsync(id, category);
            return true;
        }
    }
}
