using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class CustomerRepayment
    {
        [Key]
        public int ID { get; set; }
        public string PartnerID { get; set; }
        public string CustomerReference { get; set; }
        public string TransactionReference { get; set; }
        public double Amount { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int IsVisible { get; set; }
    }
}
