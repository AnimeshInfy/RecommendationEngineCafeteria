using Domain.DataAccess;

namespace Core.Utilities
{
    public class SentimentAnalysis
    {
        private readonly CafeteriaDbContext _dbContext;

        public SentimentAnalysis(CafeteriaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string AnalyzeSentiment(string comment)
        {
            var positiveWords = _dbContext.Sentiments.Where(x => x.Mood.ToLower() == "good")
                .Select(s => s.Sentiments.ToLower()).ToList();

            foreach (var word in positiveWords)
            {
                if (comment.ToLower().Contains(word))
                {
                    return "Positive";
                }
            }

            var negativeWords = _dbContext.Sentiments.Where(x => x.Mood.ToLower() == "bad")
            .Select(s => s.Sentiments.ToLower()).ToList();

            foreach (var word in negativeWords)
            {
                if (comment.ToLower().Contains(word))
                {
                    return "Negative";
                }
            }
            return "Neutral";
        }
    }
}
