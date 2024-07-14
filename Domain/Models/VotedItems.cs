using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class VotedItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FoodName { get; set; }
        public string MealTypes { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public Users User { get; set; }    
        [Required]
        public DateTime Date { get; set; }

    }
}
