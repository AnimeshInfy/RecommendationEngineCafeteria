using Domain.DataAccess;
using Domain.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class SentimentsAnalysisRepository : ISentimentsAnalysisRepository
    {
        private readonly CafeteriaDbContext _context;
        public SentimentsAnalysisRepository(CafeteriaDbContext context)
        {
            _context = context;
        }

        public async Task CalculateIndividualSentimentScore(int feedbackId)
        {
            var feedback = _context.Feedbacks.FirstOrDefault(x => x.Id == feedbackId);

            var sentimentScore = 0;

            var goodSentiments = _context.Sentiments.Where(x => x.Mood == "Positive").
                                 Select(x => x.Sentiments).ToListAsync();

            var badSentiments = _context.Sentiments.Where(x => x.Mood == "Negative").
                                 Select(x => x.Sentiments).ToListAsync();

            List<string> negativeWords = new List<string> {"Not","Too","More","Could be","Should be" };

            List<string> feedbackList = feedback.Comment.Split(' ').ToList();

            foreach ( var goodSentiment in await goodSentiments)
            {
                bool hasCommonItems = feedbackList.Any(item => negativeWords.Contains(item));

                if (feedbackList.Contains(goodSentiment))
                {
                    if (hasCommonItems)
                    {
                        feedback.SentimentScore -= 5;
                    }
                    feedback.SentimentScore += 5;
                }
            }
            foreach (var badSentiment in await badSentiments)
            {
                bool hasCommonItems = feedbackList.Any(item => negativeWords.Contains(item));

                if (feedbackList.Contains(badSentiment))
                {
                    if (hasCommonItems)
                    {
                        feedback.SentimentScore += 5;
                    }
                    feedback.SentimentScore -= 5;
                }
            }
            _context.SaveChangesAsync();
        }

        public async Task CalculateSentimentsScore()
        {
            var feedbackGroupByItemId = await _context.Feedbacks
                                      .GroupBy(u => u.MenuItemId)
                                      .Select(x => new
                                      {
                                          MenuItemId = x.Key,
                                          SentimentScoreAvg = x.Average(f => f.SentimentScore)
                                      })
                                      .ToListAsync();

            foreach (var group in feedbackGroupByItemId)
            {
                var menuItem = await _context.MenuItems.FindAsync(group.MenuItemId);
                if (menuItem != null)
                {
                    menuItem.SentimentScore  = group.SentimentScoreAvg;
                }
            }
        }
    }
}
