using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserNotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [ForeignKey("Notification")]
        public int NotificationId { get; set; }

        public bool IsRead { get; set; }

        public Users Users { get; set; }
        public Notification Notification { get; set; }
    }
}
