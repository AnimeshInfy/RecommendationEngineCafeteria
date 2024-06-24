using Domain.ModelDTO;
using Domain.Models;
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

        public FeedbackRequestHandler(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public async Task<string> HandleRequestAsync(string request)
        {
            if (request.Contains("ProvideFeedback"))
            {
                var a = request.Split('_');
                var jsonDeserialized = JsonConvert.DeserializeObject<Feedback>(a[1]);
                await _feedbackService.AddFeedbackAsync(jsonDeserialized);
                return "Feeback Provided!";
            }
            return "Invalid Request";
        }
    }
}
