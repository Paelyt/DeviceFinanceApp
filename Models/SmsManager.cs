using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class SmsManager
    {
        [Key]
        public int ID { get; set; }
        public int UtilityPartner_FK { get; set; }
        public string MSIDN { get; set; }
        public string SmsMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int IsVisible { get; set; }
    }
}
