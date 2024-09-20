using Mapster;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response;
using Meowgic.Shares.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class QuestionService(IUnitOfWork unitOfWork) : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PagedResultResponse<Question>> GetPagedQuestion(QueryPagedQuestion request)
        {
            return (await _unitOfWork.GetQuestionRepository().GetPagedQuestion(request)).Adapt<PagedResultResponse<Question>>();
        }

        public async Task<Question> CreateQuestion(QuestionRequest request)
        {
            var question = request.Adapt<Question>();

            await _unitOfWork.GetQuestionRepository().AddAsync(question);
            await _unitOfWork.SaveChangesAsync();

            return question.Adapt<Question>();
        }

        public async Task UpdateQuestion(string id, QuestionRequest request)
        {
            var question = await _unitOfWork.GetQuestionRepository().FindOneAsync(s => s.Id == id);

            if (question is not null)
            {
                question = request.Adapt<Question>();

                await _unitOfWork.GetQuestionRepository().UpdateAsync(question);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("Not found");
            }
        }

        public async Task<bool> DeleteQuestion(string id)
        {
            var question = await _unitOfWork.GetQuestionRepository().GetByIdAsync(id);
            if (question == null)
            {
                return false;
            }
            await _unitOfWork.GetQuestionRepository().DeleteAsync(question);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuestionResponse>> GetAll()
        {
            var questions = (await _unitOfWork.GetQuestionRepository().GetAll());
            if (questions != null)
            {
                return questions.Adapt<List<QuestionResponse>>();


            }
            throw new NotFoundException("No category was found");
        }
    }
}
