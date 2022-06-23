using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class OEM_Store
    {
        [Key]
        public int ID { get; set; }
        public int OEM_LGA_FK { get; set; }
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string LandMark { get; set; }
        public int IsVisible { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
