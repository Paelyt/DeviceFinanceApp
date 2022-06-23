using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class CustomerOrder
    {
        [Key]
        public int ID { get; set; }
        public int StoreID { get; set; }
        public string ProductCode { get; set; }
        public string ReferenceId { get; set; }
        public string TransactionReference { get; set; }
        public string CustomerActivatedPhoneNo { get; set; }
        public int Tenure { get; set; }
        public string DeliveryCode { get; set; }
        public string DeviceName { get; set; }
        public string IMEI { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNo { get; set; }
        public string BVN { get; set; }
        public string BVNPhoto { get; set; }
        public string NIN { get; set; }
        public string StateOfResidence { get; set; }
        public string LGA { get; set; }        
        public string TransactionDate { get; set; }
        public string NextOfKinFullName { get; set; }
        public string NextofkinPhoneNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DateMOdified { get; set; }
        public string ValueDate { get; set; }
        public string ValueTime { get; set; }
        public int IsVisible { get; set; }
        public int OrderStatus_FK { get; set; }
        public string CallBackUrl { get; set; }
        public int AddressVerified { get; set; }
        public string AddressVerificationRemark { get; set; }
    }
}
