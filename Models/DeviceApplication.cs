using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class DeviceApplication
    {
        [Key]
        public int ID { get; set; }
        public string PartnerID { get; set; }
        public string TransactionReference { get; set; }
        public string ProductCode { get; set; }
        public string IMEI { get; set; }
        public string StoreID { get; set; }
        public string MeansOfId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Othernames { get; set; }
        public string Gender { get; set; }
        public string HomeAddress { get; set; }
        public string PersonalEmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankCode { get; set; }
        public string BVN { get; set; }
        public string StateOfOrigin { get; set; }
        public string LGA { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int ApplicationStatus_FK { get; set; }
        public int IsVisible { get; set; }
        public string Remarks { get; set; }
    }
}
