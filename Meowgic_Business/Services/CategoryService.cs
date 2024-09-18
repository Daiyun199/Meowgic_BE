using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Repositories;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PagedResultResponse<Category>> GetPagedCategory(QueryPagedCategory request)
        {
            return (await _unitOfWork.GetCategoryRepository().GetPagedCategory(request)).Adapt<PagedResultResponse<Category>>();
        }

        public async Task<Category> CreateCategory(CategoryRequest request)
        {
            if (await _unitOfWork.GetCategoryRepository().AnyAsync(s => s.Name == request.Name))
            {
                throw new BadRequestException("This categỏy already exists");
            }

            var category = request.Adapt<Category>();

            await _unitOfWork.GetCategoryRepository().AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category.Adapt<Category>();
        }

        public async Task UpdateCategory(string id, CategoryRequest request)
        {
            var category = await _unitOfWork.GetCategoryRepository().FindOneAsync(s => s.Id == id);

            if (category is not null)
            {
                category = request.Adapt<Category>();

                await _unitOfWork.GetCategoryRepository().UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Not found");
            }
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var category = await _unitOfWork.GetCategoryRepository().GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }
            await _unitOfWork.GetCategoryRepository().DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoryResponse>> GetAll()
        {
            var categories = (await _unitOfWork.GetCategoryRepository().GetAll());
            if (categories != null)
            {
                return categories.Adapt<List<CategoryResponse>>();


            }
            throw new NotFoundException("No category was found");
        }
    }
}
