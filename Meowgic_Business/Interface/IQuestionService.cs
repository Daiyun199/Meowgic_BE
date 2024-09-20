using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IQuestionService
    {
        Task<PagedResultResponse<Question>> GetPagedQuestion(QueryPagedQuestion request);

        Task<Question> CreateQuestion(QuestionRequest request);

        Task UpdateQuestion(string id, QuestionRequest request);

        Task<bool> DeleteQuestion(string id);
        Task<List<QuestionResponse>> GetAll();
    }
}
