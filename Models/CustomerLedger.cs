using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class CustomerLedger
    {
        [Key]
        public int ID { get; set; }
        public int OEM_PartnerID { get; set; }
        public int DevicePlanID { get; set; }
        public string LoanReference { get; set; }
        public DateTime TransDate { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public int RepaymentFlag { get; set; }
        public int IsVisible { get; set; }
    }
}
