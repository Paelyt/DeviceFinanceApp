using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class Subscription
    {
        [Key]
        public int ID { get; set; }
        public int PricingModel_FK { get; set; }
        public int MonthVal { get; set; }
        public double PrincipalAmount { get; set; }
        public double InterestAmount { get; set; }
        public double TotalAmount { get; set; }
        public double MonthlyRepayAmount { get; set; }
        public int IsVisible { get; set; }
    }
}
