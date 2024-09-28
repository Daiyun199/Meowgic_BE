using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Question;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class QuestionService(IUnitOfWork unitOfWork) : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PagedResultResponse<Question>> GetPagedQuestion(QueryPagedQuestion request)
        {
            return (await _unitOfWork.GetQuestionRepository.GetPagedQuestion(request)).Adapt<PagedResultResponse<Question>>();
        }

        public async Task<Question> CreateQuestion(QuestionRequest request, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var question = request.Adapt<Question>();
            question.CreatedBy = userId;
            question.CreatedTime = DateTime.Now;

            await _unitOfWork.GetQuestionRepository.AddAsync(question);
            await _unitOfWork.SaveChangesAsync();

            return question.Adapt<Question>();
        }

        public async Task UpdateQuestion(string id, QuestionRequest request, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var question = await _unitOfWork.GetQuestionRepository.FindOneAsync(s => s.Id == id);

            if (question is not null)
            {
                question = request.Adapt<Question>();
                question.LastUpdatedBy = userId;
                question.LastUpdatedTime = DateTime.Now;

                await _unitOfWork.GetQuestionRepository.UpdateAsync(question);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Not found");
            }
        }

        public async Task<bool> DeleteQuestion(string id, ClaimsPrincipal claim)
        {
            var userId = claim.FindFirst("aid")?.Value;

            var account = await _unitOfWork.GetAccountRepository.GetCustomerDetailsInfo(userId);

            if (account is null)
            {
                throw new BadRequestException("Account not found");
            }
            var question = await _unitOfWork.GetQuestionRepository.GetByIdAsync(id);
            if (question == null)
            {
                throw new BadRequestException("Not found question");
            }
            question.DeletedBy = userId;
            question.DeletedTime = DateTime.Now;
            await _unitOfWork.GetQuestionRepository.UpdateAsync(question);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuestionResponse>> GetAll()
        {
            var questions = (await _unitOfWork.GetQuestionRepository.GetAll());
            if (questions != null)
            {
                return questions.Adapt<List<QuestionResponse>>();


            }
            throw new NotFoundException("No category was found");
        }
    }
}
