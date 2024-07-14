using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiClientServerConnection
{
    public class MenuItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int AvailabilityStatus { get; set; }
    }

    public class MenuResponse
    {
        public string Status { get; set; }
        public List<MenuItem> Menu { get; set; }
    }

}



