using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class DeviceChange
    {
        [Key]
        public int ID { get; set; }
        public int CustomerOrder_FK { get; set; }
        public string IMEI { get; set; }
        public string Remarks { get; set; }
        public string TransDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IsVisible { get; set; }
    }
}
