using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class Partner
    {
        [Key]
        public int ID { get; set; }
        public string PartnerID { get; set; }
        public string PartnerKey { get; set; }
        public string PartnerName { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessEmail { get; set; }
        public string PartnerDescription { get; set; }
        public string Phonenumber { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateMOdified { get; set; }
        public int IsVisible { get; set; }
    }
}
