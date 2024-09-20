using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class CategoryService : ICategoryService
    {
   
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService( ICategoryRepository categoryRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(string id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(CategoryRequestDTO categoryRequest)
        {
            // Chuyển đổi từ CategoryRequestDTO sang Category
            var category = _mapper.Map<Category>(categoryRequest);
        

            // Gọi repository để lưu category vào cơ sở dữ liệu
            return await _categoryRepository.CreateCategoryAsync(category);
        }

        public async Task<Category?> UpdateCategoryAsync(string id, CategoryRequestDTO categoryRequest)
        {
            var category = _mapper.Map<Category>(categoryRequest);
            return await _categoryRepository.UpdateCategoryAsync(id, category);
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            // Có thể thêm logic nghiệp vụ nếu cần
            return await _categoryRepository.DeleteCategoryAsync(id);
        }
    }
}
