using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class ParticpatingStore
    {
        [Key]
        public int ID { get; set; }
        public int NigerianState_FK { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public string StoreAddress { get; set; }
        public string LandMark { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactEmailAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateMOdified { get; set; }
        public int IsVisible { get; set; }
    }
}
