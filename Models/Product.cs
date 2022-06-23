using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public int Device_FK { get; set; }
        public int PricingModel_FK { get; set; }
        public string ProductCode { get; set; }
        public double BundlePrice { get; set; }
        public double InterestValue { get; set; }
        public double MonthlyRepayment { get; set; }
        public double PrincipalAmount { get; set; }
        public double InterestAmount { get; set; }
        public int ActiveFlag { get; set; }
        public int IsVisible { get; set; }
    }
}
