using Domain.Models;
using Domain.Repositories;
using Domain.Repositories.IRepositories;
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
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        [Authorize(Roles = "Employee")]
        public async Task AddFeedbackAsync(Feedback feedback)
        {
            await _feedbackRepository.AddFeedbackAsync(feedback);
        }

        public async Task<List<Feedback>> GetFeedbacksByFeedbackIdAsync(int feedbackId)
        {
            return await _feedbackRepository.GetFeedbacksByFeedbackIdAsync(feedbackId);
        }

        public async Task<List<Feedback>> ViewAllFeedbacksAsync()
        {
            return await _feedbackRepository.ViewAllFeedbacks();
        }
    }

}
