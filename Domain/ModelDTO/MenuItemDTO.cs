using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ModelDTO
{
    public class MenuItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string MealType { get; set; }
        public double? AvgRating { get; set; }   
        public double? SentimentScore { get; set;}
        public double? CommonScore { get; set; }
        public bool? isItemUnderDiscardList { get; set; }
        public string? dietType { get; set; }
        public string? SpiceLevel { get; set; }
        public string? regionalMealPreference { get; set; }
        public string? isSweetTooth { get; set; }
    }
}

