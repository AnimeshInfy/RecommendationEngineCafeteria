using Domain.DataAccess;
using Domain.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;
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

        public async Task CalculateSentimentScores()
        {
            var feedbacks = await _context.Feedbacks.ToListAsync();
            var goodSentiments = await _context.Sentiments
                                               .Where(x => x.Mood == "Positive")
                                               .Select(x => x.Sentiments)
                                               .ToListAsync();
            var badSentiments = await _context.Sentiments
                                              .Where(x => x.Mood == "Negative")
                                              .Select(x => x.Sentiments)
                                              .ToListAsync();

            List<string> negativeWords = new List<string> { "Not", "Too", "More", "Could be", "Should be" };

            foreach (var feedback in feedbacks)
            {
                List<string> feedbackWords = feedback.Comment.Split(' ').ToList();
                bool hasNegativeWord = feedbackWords.Any(word => negativeWords.Contains(word));

                feedback.SentimentScore = 0;

                foreach (var goodSentiment in goodSentiments)
                {
                    if (feedbackWords.Contains(goodSentiment))
                    {
                        feedback.SentimentScore += hasNegativeWord ? -5 : 5;
                        await _context.SaveChangesAsync();

                    }
                }

                foreach (var badSentiment in badSentiments)
                {
                    if (feedbackWords.Contains(badSentiment))
                    {
                        feedback.SentimentScore += hasNegativeWord ? 5 : -5;
                        await _context.SaveChangesAsync();

                    }
                }
            }

            await _context.SaveChangesAsync();

            var feedbackGroupByItemId = feedbacks
                                      .GroupBy(f => f.MenuItemId)
                                      .Select(g => new
                                      {
                                          MenuItemId = g.Key,
                                          SentimentScoreAvg = g.Average(f => f.SentimentScore ?? 0)
                                      })
                                      .ToList();

            var menuItemIds = feedbackGroupByItemId.Select(f => f.MenuItemId).ToList();
            var menuItems = await _context.MenuItems
                                         .Where(mi => menuItemIds.Contains(mi.Id))
                                         .ToListAsync();

            foreach (var menuItem in menuItems)
            {
                var sentimentScoreAvg = feedbackGroupByItemId
                                        .FirstOrDefault(f => f.MenuItemId == menuItem.Id)?.SentimentScoreAvg;

                if (sentimentScoreAvg.HasValue)
                {
                    menuItem.SentimentScore = sentimentScoreAvg.Value;
                    await _context.SaveChangesAsync();

                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
