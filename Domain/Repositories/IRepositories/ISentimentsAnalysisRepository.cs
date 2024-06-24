﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepositories
{
    public interface ISentimentsAnalysisRepository
    {
        public Task CalculateSentimentsScore();
        public Task CalculateIndividualSentimentScore(int feedbackId);
    }
}
