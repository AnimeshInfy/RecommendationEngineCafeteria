using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Profile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string dietType { get; set; }

        public string SpiceLevel { get; set; }

        public string regionalMealPreference { get; set; }

        public string isSweetTooth { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public Users Users { get; set; }
    }
}
