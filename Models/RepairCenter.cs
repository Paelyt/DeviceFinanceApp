using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class RepairCenter
    {
        [Key]
        public int ID { get; set; }
        public int RepairCenterID { get; set; }
        public string RepairCenterName { get; set; }
        public string RepairCenterAddress { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public int IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
