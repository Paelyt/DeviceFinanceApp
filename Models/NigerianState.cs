using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class NigerianState
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Audit { get; set; }
        public int IsVisible { get; set; }
        public int Country_FK { get; set; }
        public int OrderID { get; set; }
    }
}
