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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }
        [Required]
        public string TargetedUserIds { get; set; }

        [NotMapped]
        public int[] TargetedUserIdsInt
        {
            get => TargetedUserIds?.Split(',').Select(int.Parse).ToArray();
            set => TargetedUserIds = string.Join(",", value);
        }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime NotificationDate { get; set; }
    }
}
