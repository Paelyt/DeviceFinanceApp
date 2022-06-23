using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class DeviceManager
    {
        [Key]
        public int ID { get; set; }
        public string PhoneNumber { get; set; }
        public string IMEI { get; set; }
        public string ReferenceId { get; set; }
        public string TransactionReference { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int DeviceStatus_FK { get; set; }
        public string Remarks { get; set; }
        public string PartnerID { get; set; }
        public string SubmissionDate { get; set; }
        public string ExpectedCollectionDate { get; set; }
        public string CollectionDate { get; set; }
        public int IsVisible { get; set; }
    }
}
