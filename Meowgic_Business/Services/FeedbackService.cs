using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.Feedback;
using Meowgic.Shares.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedbackService(
            IFeedbackRepository feedbackRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _feedbackRepository = feedbackRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            try
            {
                return await _feedbackRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving feedbacks.", ex);
            }
        }

        public async Task<Feedback?> GetFeedbackByIdAsync(string id)
        {
            try
            {
                return await _feedbackRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving feedback with ID {id}.", ex);
            }
        }

        public async Task<List<Feedback>> GetAllFeedbacksByServiceIdAsync(string serviceId)
        {
            try
            {
                return await _feedbackRepository.GetAllByServiceId(serviceId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving feedbacks.", ex);
            }
        }

        public async Task<Feedback> CreateFeedbackAsync(FeedbackRequestDTO feedbackRequest)
        {
            try
            {
                // Lấy AccountId từ HttpContext
                var accountId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (accountId == null)
                {
                    throw new UnauthorizedException("User is not authenticated.");
                }

                var feedback = _mapper.Map<Feedback>(feedbackRequest);
                feedback.AccountId = accountId;

                await _feedbackRepository.AddAsync(feedback);

                return feedback;
            }
            catch (UnauthorizedException ex)
            {
                throw ex; // Giữ nguyên lỗi UnauthorizedException
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the feedback.", ex);
            }
        }

        public async Task<Feedback> UpdateFeedbackAsync(string id, FeedbackRequestDTO feedbackRequest)
        {
            try
            {
                var existingFeedback = await _feedbackRepository.GetByIdAsync(id);
                if (existingFeedback == null)
                {
                    throw new NotFoundException("Feedback not found.");
                }

                // Cập nhật thông tin từ DTO
                _mapper.Map(feedbackRequest, existingFeedback);
                _feedbackRepository.UpdateAsync(existingFeedback);

                return existingFeedback;
            }
            catch (NotFoundException ex)
            {
                throw ex; // Giữ nguyên lỗi NotFoundException
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the feedback with ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteFeedbackAsync(string id)
        {
            try
            {
                var result = await _feedbackRepository.DeleteAsync(id);
                if (result == false)
                {
                    throw new Exception("Delete failed");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the feedback with ID {id}.", ex);
            }
        }
    }
}
