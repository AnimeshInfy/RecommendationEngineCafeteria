using Core.Utilities;
using Domain.Services.IServices;
using Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class RecommendationEngineService : IRecommendationEngineService
    {
        private readonly IRatingServce _ratingService;
        private readonly ISentimentsAnalysisService _sentimentService;
        public RecommendationEngineService(IRatingServce ratingServce, ISentimentsAnalysisService sentimentService)
        {
            _ratingService = ratingServce;
            _sentimentService = sentimentService;
        }

        public Task CalculateSentimentScore()
        {
            throw new NotImplementedException();
        }
    }
}
