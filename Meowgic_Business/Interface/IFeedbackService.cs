using Meowgic.Data.Entities;
using Meowgic.Data.Models.Request.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback?> GetFeedbackByIdAsync(string id);
        Task<List<Feedback>> GetAllFeedbacksByServiceIdAsync(string serviceId);
        Task<Feedback> CreateFeedbackAsync(FeedbackRequestDTO feedbackRequest);
        Task<Feedback> UpdateFeedbackAsync(string id, FeedbackRequestDTO feedbackRequest);
        Task<bool> DeleteFeedbackAsync(string id);
    }

}
