using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class MenuItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public MealType MealType { get; set; }  
        public string? AvgRating { get; set; }
        public string? SentimentScore { get; set; }
    }
    public enum MealType
    {
        Breakfast,
        Lunch,
        Dinner
    }
}
