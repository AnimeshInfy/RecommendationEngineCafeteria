using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime NotificationDate { get; set; }
        [ForeignKey("MenuItems")]
        public int MenuItemId { get; set; }
        public Users User { get; set; }
        public MenuItems MenuItems { get; set; }    
    }
}
