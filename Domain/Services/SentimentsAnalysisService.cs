using Domain.Repositories.IRepositories;
using Domain.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class SentimentsAnalysisService : ISentimentsAnalysisService
    {
        private readonly ISentimentsAnalysisRepository _sentiments;
        public SentimentsAnalysisService(ISentimentsAnalysisRepository sentiments) 
        {
            _sentiments = sentiments;
        }
        public async Task CalculateSentimentsScore()
        {
             await _sentiments.CalculateSentimentScores();
        }
    }
}
