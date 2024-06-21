using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("MenuItems")]
        [Required]
        public int MenuItemId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        [Range(1, 5)] 
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public Users User { get; set; }
        public int SentimentScore { get; set; }
        public MenuItems MenuItems { get; set; }

    }
}
