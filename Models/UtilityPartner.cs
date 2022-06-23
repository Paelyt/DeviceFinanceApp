using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class UtilityPartner
    {
        [Key]
        public int ID { get; set; }
        public string PartnerName { get; set; }
        public string BearerToken { get; set; }
        public int IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
