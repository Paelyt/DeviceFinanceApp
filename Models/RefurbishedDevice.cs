using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class RefurbishedDevice
    {
        [Key]
        public int ID { get; set; }
        public int CustomerOrder_FK { get; set; }
        public string RepairCenterID { get; set; }
        public string PhoneNumber { get; set; }
        public string ExpectedCollectionDate { get; set; }
        public string SubmissionDate { get; set; }
        public string CollectionDate { get; set; }
        public string Remarks { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int ActiveFlag { get; set; }
        public int IsVisible { get; set; }        
    }
}
