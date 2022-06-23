using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class DevicePlan
    {
        [Key]
        public int ID { get; set; }
        public int PricingModel_FK { get; set; }
        public int Device_FK { get; set; }
        public string RiskClasses { get; set; }
        public int DeviceDataID { get; set; }
        public string LoanID { get; set; }
        public string DeviceplanName { get; set; }
        public string BundleName { get; set; }
        public double DeviceCost { get; set; }
        public double SellingPrice { get; set; }
        public double OldSellingPrice { get; set; }
        public double TotalRepayment { get; set; }
        public double MonthlyPayment { get; set; }
        public double UpfrontPayment { get; set; }
        public double FinancedAmount { get; set; }
        public double DataQty { get; set; }
        public double SmsQty { get; set; }
        public double VoiceMinutes { get; set; }
        public double MinDurationMonths { get; set; }
        public double MaxDurationMonths { get; set; }
        public int DeviceOptionID { get; set; }
        public int DeviceTypeID { get; set; }
        public double BrokersFee { get; set; }       
        public double IntelligraCompensation { get; set; }
        public double LockTechnologyFee { get; set; }   
        public double InsuranceFee { get; set; }

    }
}
