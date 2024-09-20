using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Meowgic.Data.Extension;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Category;
using Meowgic.Data.Models.Request.Question;
using Meowgic.Data.Models.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        private Expression<Func<Question, object>> GetSortProperty(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "description" => question => question.Description == null ? question.Id : question.Description,
                _ => question => question.Id,
            };
        }

        public async Task<PagedResultResponse<Question>> GetPagedQuestion(QueryPagedQuestion request)
        {
            var query = _context.Questions.AsQueryable();
            query = query.ApplyPagedQuestionFilter(request);
            //Sort
            query = request.OrderByDesc ? query.OrderByDescending(GetSortProperty(request.SortColumn))
                                        : query.OrderBy(GetSortProperty(request.SortColumn));
            //Paging
            return await query.ToPagedResultResponseAsync(request.PageNumber, request.PageSize);
        }

        public async Task<List<Question>> GetAll()
        {
            return await _context.Questions.ToListAsync();
        }
        public void Update(Question question)
        {
            _context.Questions.Update(question);
        }
    }
}
