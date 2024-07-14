using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utilities
{
    public class RatingCalculator
    {
        public static double CalculateAverageRating(List<Feedback> feedbacks)
        {
            if (feedbacks.Count == 0) {
                return 0;
            }
            return feedbacks.Average(f => f.Rating);
        }
    }

}
