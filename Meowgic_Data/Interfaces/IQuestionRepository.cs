using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Card;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<PagedResultResponse<Question>> GetPagedQuestion(QueryPagedQuestion request);
        void Update(Question question);
        Task<List<Question>> GetAll();
    }
}
