using Domain.Models;
using Domain.Repositories;
using Domain.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly FeedbackRepository _feedbackRepository;

        public FeedbackService(FeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        [Authorize(Roles = "Employee")]
        public async Task AddFeedbackAsync(Feedback feedback)
        {
            await _feedbackRepository.AddFeedbackAsync(feedback);
        }

        public Task<List<Feedback>> GetFeedbacksByFeedbackIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Feedback>> GetFeedbacksByIdAsync(int id)
        {
            return await _feedbackRepository.GetFeedbacksByFeedbackIdAsync(id);
        }

        public async Task<List<Feedback>> ViewAllFeedbacksAsync()
        {
            return await _feedbackRepository.ViewAllFeedbacks();
        }
    }

}
