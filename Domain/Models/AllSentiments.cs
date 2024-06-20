using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AllSentiments
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Sentiments { get; set; }
        [Required]
        public string Mood {  get; set; }
    }
}
