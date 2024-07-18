using Data.ModelDTO;
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

        public async Task<List<FeedbackDTO>> GetFeedbacksByFeedbackIdAsync(int feedbackId)
        {
            var feedback = await _feedbackRepository.GetFeedbacksByFeedbackIdAsync(feedbackId);
            return feedback.Select(model => MapFeedbackModelToDto(model)).ToList();
        }

        public async Task<List<FeedbackDTO>> ViewAllFeedbacksAsync()
        {
            var feedback = await _feedbackRepository.ViewAllFeedbacks();
            return feedback.Select(model => MapFeedbackModelToDto(model)).ToList();
        }
        private FeedbackDTO MapFeedbackModelToDto(Feedback model)
        {
            return new FeedbackDTO
            {
                MenuItemId = model.MenuItemId,
                UserId = model.UserId,
                Comment = model.Comment,
                Rating = model.Rating,
                Date = model.Date,
                SentimentScore = model.SentimentScore,
            };
        }
    }

}
