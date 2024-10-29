using AutoMapper;
using Meowgic.Business.Interface;
using Meowgic.Data.Entities;
using Meowgic.Data.Interfaces;
using Meowgic.Data.Models.Request.ScheduleReader;
using Meowgic.Data.Repositories;
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
    public class ScheduleReaderService : IScheduleReaderService
    {
        private readonly IScheduleReaderRepository _scheduleReaderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ScheduleReaderService(
            IScheduleReaderRepository scheduleReaderRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _scheduleReaderRepository = scheduleReaderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ScheduleReader>> GetAllSchedulesAsync()
        {
            try
            {
                return await _scheduleReaderRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving schedules.", ex);
            }
        }

        public async Task<ScheduleReader?> GetScheduleByIdAsync(string id)
        {
            try
            {
                return await _scheduleReaderRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the schedule with ID {id}.", ex);
            }
        }
        public async Task<IEnumerable<ScheduleReader>> GetSchedulesByDateRangeAndAccountIdAsync(DateOnly startDate, DateOnly endDate)
        {
            try
            {
                var accountId = _httpContextAccessor.HttpContext?.User?.FindFirst("aid")?.Value;
                if (accountId == null)
                {
                    throw new UnauthorizedException("User is not authenticated.");
                }
                return await _scheduleReaderRepository.GetSchedulesByDateRangeAndAccountIdAsync(startDate, endDate, accountId);
            }catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving schedules.", ex);
            }
        }
        public async Task<IEnumerable<ScheduleReader>> GetSchedulesByDateRangeAndAccountIdAndIsBookedAsync(DateOnly startDate, DateOnly endDate, bool isBooked)
        {
            try
            {
                var accountId = _httpContextAccessor.HttpContext?.User?.FindFirst("aid")?.Value;
                if (accountId == null)
                {
                    throw new UnauthorizedException("User is not authenticated.");
                }
                return await _scheduleReaderRepository.GetSchedulesByDateRangeAccountIdAndStatusAsync(startDate, endDate, accountId,isBooked);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving schedules.", ex);
            }
        }
        public async Task<IEnumerable<ScheduleReader>> GetSchedulesByDateRangeAndReaderIdAndIsBookedAsync(string readerID,DateOnly startDate, DateOnly endDate, bool isBooked)
        {
            try
            { 
                return await _scheduleReaderRepository.GetSchedulesByDateRangeAccountIdAndStatusAsync(startDate, endDate, readerID, isBooked);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving schedules.", ex);
            }
        }



        public async Task<ScheduleReader> CreateScheduleAsync(ScheduleRequestDTO2 scheduleRequest)
        {
            try
            {
            

                // Lấy AccountId từ HttpContext
                var accountId = _httpContextAccessor.HttpContext?.User?.FindFirst("aid")?.Value;
                if (accountId == null)
                {
                    throw new UnauthorizedException("User is not authenticated.");
                }

                var schedule = _mapper.Map<ScheduleReader>(scheduleRequest);
                schedule.AccountId = accountId;
                
                await _scheduleReaderRepository.AddAsync(schedule);
             

                return schedule;
            }
            catch (UnauthorizedException ex)
            {
                throw ex; // Giữ nguyên lỗi UnauthorizedException
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the schedule.", ex);
            }
        }

        public async Task<ScheduleReader> UpdateScheduleAsync(string id, ScheduleRequestDTO2 scheduleRequest)
        {
            try
            {
                var existingSchedule = await _scheduleReaderRepository.GetByIdAsync(id);
                if (existingSchedule == null)
                {
                    throw new NotFoundException("Schedule not found.");
                }

                // Cập nhật thông tin từ DTO
                _mapper.Map(scheduleRequest, existingSchedule);
                await _scheduleReaderRepository.UpdateAsync(existingSchedule);


                return existingSchedule;
            }
            catch (NotFoundException ex)
            {
                throw ex; // Giữ nguyên lỗi NotFoundException
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the schedule with ID {id}.", ex);
            }
        }

        public async Task<bool> DeleteScheduleAsync(string id)
        {
            try
            {
                var existingSchedule = await _scheduleReaderRepository.GetByIdAsync(id);
                var accountId = _httpContextAccessor.HttpContext?.User?.FindFirst("aid")?.Value;
                if (accountId != existingSchedule.AccountId)
                {
                    throw new UnauthorizedException("You dont have any permission to delete this schedule");
                }
                var result = await _scheduleReaderRepository.DeleteAsync(id);
                if (!result)
                {
                    throw new NotFoundException("Schedule not found or could not be deleted.");
                }
               

                return result;
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the schedule with ID {id}.", ex);
            }
        }
    }
}
