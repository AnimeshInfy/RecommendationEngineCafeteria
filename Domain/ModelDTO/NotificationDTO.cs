using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ModelDTO
{
    public class NotificationDTO
    {
        public string Message { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
