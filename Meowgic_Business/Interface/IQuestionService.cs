using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response;
using Meowgic.Data.Models.Response.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IQuestionService
    {
        Task<PagedResultResponse<Question>> GetPagedQuestion(QueryPagedQuestion request);

        Task<Question> CreateQuestion(QuestionRequest request, ClaimsPrincipal claim);

        Task UpdateQuestion(string id, QuestionRequest request, ClaimsPrincipal claim);

        Task<bool> DeleteQuestion(string id, ClaimsPrincipal claim);
        Task<List<QuestionResponse>> GetAll();
    }
}
