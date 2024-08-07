﻿using Domain.ModelDTO;
using Domain.Models;
using Domain.Services;
using Domain.Services.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.RequestHandler
{
    public class FeedbackRequestHandler
    {
        private readonly IFeedbackService _feedbackService;
        private readonly INotificationService _notificationService;
        private readonly IMenuService _menuService;

        public FeedbackRequestHandler(IFeedbackService feedbackService,
            INotificationService notificationService,
            IMenuService menuService)
        {
            _feedbackService = feedbackService;
            _notificationService = notificationService;
            _menuService = menuService;
        }

        public async Task<string> HandleRequestAsync(string request)
        {
            if (request.Contains("ProvideFeedback"))
            {
                var feedback = request.Split('_');
                var jsonDeserialized = JsonConvert.DeserializeObject<Feedback>(feedback[1]);
                await _feedbackService.AddFeedbackAsync(jsonDeserialized);
                return "Feeback Provided!";
            }
            if (request.Contains("ViewFeedbacks"))
            {
                var feedbacks = await _feedbackService.ViewAllFeedbacksAsync();
                return JsonConvert.SerializeObject(feedbacks);
            }
            if (request.Contains("ViewFeedbacksById"))
            {
                var feedbackInfo = request.Split('_').Select(int.Parse).ToArray();
                await _feedbackService.GetFeedbacksByFeedbackIdAsync(feedbackInfo[1]);
            }
            if (request.Contains("GetDetailedfeedbackonfoodItem"))
            {
                var detailedFeedbackInfo = request.Split('_');
                int foodId = Convert.ToInt32(detailedFeedbackInfo[1]);
                await _notificationService.GetDetailedFeedbackOnDiscardedItems(foodId);
            }
            if (request.Contains("GiveDetailedFeedbackOnDiscardedItems"))
            {
                var detailedFeedbackInfo = request.Split('_');
                await _notificationService.GiveDetailedFeedbackOnDiscardedItems(detailedFeedbackInfo[1]);
            }
            return "Invalid Request";
        }

    }
}
