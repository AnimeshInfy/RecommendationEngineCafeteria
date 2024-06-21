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
        private readonly RatingCalculator _ratingCalculator;
        private readonly SentimentAnalysis _sentiment;
        public RecommendationEngineService(RatingCalculator ratingCalculator, SentimentAnalysis sentiment)
        {
            _ratingCalculator = ratingCalculator;
            _sentiment = sentiment; 
        }
        
    }
}
