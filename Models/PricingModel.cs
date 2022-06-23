using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class PricingModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Tenure { get; set; }        
        public DateTime DateCreated { get; set; }
        public DateTime DateMOdified { get; set; }
        public string Description { get; set; }
        public double UpfrontPayment { get; set; }
        public int InterestType { get; set; }
        public int OEM_FK { get; set; }
        public int IsVisible { get; set; }
    }
   
}
