using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class DeviceType
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }        
        public DateTime DateCreated { get; set; }
        public DateTime DateMOdified { get; set; }
        public int IsVisible { get; set; }
    }
}
