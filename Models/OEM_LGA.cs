using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class OEM_LGA
    {
        [Key]
        public int ID { get; set; }
        public int OEM_State_FK { get; set; }
        public int LGAId { get; set; }
        public string Name { get; set; }
        public int IsVisible { get; set; }
    }
}
