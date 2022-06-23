using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Classes
{
    public class Utility
    {
        public enum deviceStatus
        {
            Locked = 1,
            Changed = 2,
            Repair_Log = 3,
            Refurbished = 4,
            Recovered = 5
        }
        public enum responsecode
        {
            Success = 00,
            Failed = 99,
            Invalid_Partner = 98,
            Null_Header = 90,
            Null_Partner_Response = 89,
            General_failure = 88,
            Reference_does_not_exist = 30,
            Null_callback = 40,
            Failed_txn_update = 22,
            Not_found = 15,
            Ref_num_exist = 20,
            Transaction_error = 24,
            Delivery_code_already_used_error = 28,
            Invalid_hash = 21
        }
        public class customerValdate
        {
            public string referenceId { get; set; }
            public string deliveryCode { get; set; }
        }
        public class imeiChange
        {
            public string OldIMEI { get; set; }
            public string NewIMEI { get; set; }
            public string deliveryCode { get; set; }
            public string transDate { get; set; }
        }
        public class deviceUpdate
        {
            public string imei { get; set; }
            public string OldImei { get; set; }
            public string servicePhoneNumber { get; set; }
            public string transactionReference { get; set; }
            public string remarks { get; set; }
            public int lockId { get; set; }
            public string submissionDate { get; set; }
            public string expectedCollectionDate { get; set; }
            public string collectionDate { get; set; }
        }
        public class fulfilOrderObj
        {
            public string referenceId { get; set; }
            public string deliveryCode { get; set; }
            public string activatedPhoneNumber { get; set; }
            public string imei { get; set; }
            public string storedID { get; set; }
            public string transDate { get; set; }
            public string remarks { get; set; }
            public string callbackUrl { get; set; }
        }
        public class refurbished
        {
            public string phoneNumber { get; set; }
            public string repairCenterID { get; set; }
            public string submissionDate { get; set; }
            public string expectedCollectionDate { get; set; }
            public string collectionDate { get; set; }
            public string remarks { get; set; }
        }
        public class repairDevice
        {
            public string IMEI { get; set; }
            public string remarks { get; set; }
            public string transDate { get; set; }
            public string expectedCollectionDate { get; set; }
            public string storeId { get; set; }
            public string phoneNumber { get; set; }
        }
        public class DeliveryObj
        {
            public string deliveryCode { get; set; }
            public string deviceName { get; set; }
            public string monthlyRepayment { get; set; }
            public string upfrontRepayment { get; set; }
            public int tenure { get; set; }
            public string customerName { get; set; }
            public string phoneNumber { get; set; }
            public string transactionReference { get; set; }
            public int storeId { get; set; }
            public string storeName { get; set; }
            public string storeAddress { get; set; }
            public string storeState { get; set; }
            public string storeLGA { get; set; }
        }
        public class MyDevice
        {
            public string IMEI { get; set; }
            public string deviceName { get; set; }
            public string monthlyRepayment { get; set; }
            public string upfrontPayment { get; set; }
            public int tenure { get; set; }
        }
        public class loanDueAmountResponseDetails
        {
            public string loanReference { get; set; }
            public double amountDue { get; set; }
        }
        public class smsResponseDetails
        {
            public bool sucess { get; set; }
            public string status { get; set; }
        }
        public class sms
        {
            public string phoneNumber { get; set; }
            public string message { get; set; }
        }
        public class customerRepayment
        {
            public string accountNumber { get; set; }
            public string accountName { get; set; }
            public string amount { get; set; }
            public string transactionReference { get; set; }
            public string customerReference { get; set; }
        }
        public class eligibilityPayload
        {
            public string msisdn { get; set; }
            public string telco { get; set; }
            public string refNumber { get; set; }
        }
        public class eligibility
        {
            public double score { get; set; }
            public double creditLimit { get; set; }
            public string msisdn { get; set; }
            public string telco { get; set; }
            public string riskClass { get; set; }
            public bool status { get; set; }
        }
        public class bundle
        {
            public string servicePhoneNumber { get; set; }
            public string customerName { get; set; }
            public string amount { get; set; }
            public string transactionReference { get; set; }
            public string transDate { get; set; }
        }
        public class Onboard
        {
            public string transactionReference { get; set; }
            public string productCode { get; set; }
            public string lastname { get; set; }
            public string firstname { get; set; }
            public string othernames { get; set; }
            public string gender { get; set; }
            public string homeAddress { get; set; }
            public string phoneNumber { get; set; }
            public string alternatePhoneNumber { get; set; }
            public string personalEmailAddress { get; set; }
            public string dateOfBirth { get; set; }
            public string accountNumber { get; set; }
            public string accountName { get; set; }
            public string bankCode { get; set; }
            public string bvn { get; set; }
            public string storeId { get; set; }
            public string meansOfId { get; set; }
            public string stateOfOrigin { get; set; }
            public string lga { get; set; }
            public string landmark { get; set; }
            public string callbackUrl { get; set; }
        }
        public class customerOrder
        {
            public string StoreID { get; set; }
            public string ProductCode { get; set; }
            public string ReferenceId { get; set; }
            public int Tenure { get; set; }
            public string DeliveryCode { get; set; }
            public string CustomerName { get; set; }
            public string CustomerAddress { get; set; }
            public string CustomerPhoneNo { get; set; }
            public string BVN { get; set; }
            public string AccountNumber { get; set; }
            public string AccountName { get; set; }
            public string BankCode { get; set; }
        }
        public class addressVerifyResponse
        {
            public string transactionReference { get; set; }
            public string customerName { get; set; }
            public string customerPhone { get; set; }
            public int verified { get; set; }
            public string remark { get; set; }
        }
        public class addressVerifyObj
        {
            public string transactionReference { get; set; }
            public string customerName { get; set; }
            public string customerPhone { get; set; }
            public string customerAddress { get; set; }
            public string AddressState { get; set; }
            public string AddressLGA { get; set; }
        }
        public class fulFillOrder
        {
            public string storeId { get; set; }
            public string loanId { get; set; }
            public string transactionReference { get; set; }
            public int tenure { get; set; }
            public string deliveryCode { get; set; }
            public string imei { get; set; }
            public string servicePhoneNo { get; set; }
            public string transDate { get; set; }
        }
        public class Stores
        {
            public int StoreId { get; set; }
            public string StoreState { get; set; }
            public string StoreLGA { get; set; }
            public string StoreName { get; set; }
            public string StoreAddress { get; set; }
            public string ContactName { get; set; }
            public string ContactPhone { get; set; }
            public string ContactEmail { get; set; }
        }
        public class Products
        {
            public int Id { get; set; }
            public string deviceCategory { get; set; }
            public string deviceBrand { get; set; }
            public string deviceName { get; set; }
            public string loanCode { get; set; }
            public double oldPrice { get; set; }
            public double sellingPrice { get; set; }
            public double upfrontPayment { get; set; }
            public double monthlyRepayment { get; set; }
            public int loanTenure { get; set; }
            public string deviceSpecfications { get; set; }
            public string deviceScreensize { get; set; }
            public string deviceProcessor { get; set; }
            public string deviceRam { get; set; }
            public string deviceRom { get; set; }
            public string deviceImage1 { get; set; }
            public string deviceImage2 { get; set; }
            public string deviceImage3 { get; set; }
            public string deviceImage4 { get; set; }
        }
        public class validateOrderObj
        {
            public int Id { get; set; }
            public string device { get; set; }
            public string deviceBrand { get; set; }
            public string deviceplanName { get; set; }
            public string customerName { get; set; }
            public string customerAddress { get; set; }
            public string customerPhoneNo { get; set; }
            public string loanCode { get; set; }
            public string bvn_photo { get; set; }
            public string nextOfKinFullName { get; set; }
            public string nextofkinPhoneNumber { get; set; }
            public string bvn { get; set; }
            public string nin { get; set; }
            public string state { get; set; }
            public string lga { get; set; }
            public string deliveryCode { get; set; }
            public string referenceId { get; set; }
            public string loanTenure { get; set; }
            public string accountNumber { get; set; }
            public string bankCode { get; set; }
            public int storeID { get; set; }
            public string applicationDate { get; set; }
            public int orderStatusFK { get; set; }
            public string orderStatus { get; set; }
        }


        public static DateTime getCurrentLocalDateTime()
        {
            // gives you current Time in server timeZone
            var serverTime = DateTime.Now;
            // convert it to Utc using timezone setting of server computer
            var utcTime = serverTime.ToUniversalTime();
            var tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            // convert from utc to local
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
            return localTime;
        }

        public static string GenerateRefNo()
        {

            return UnixTimeStampUtc().ToString() + Create7DigitString(4);
            //  return InstantTimeTicks + InstantTimeSeconds;
        }
        public static string GenerateDeliveryRef()
        {
            return "DV" + Create7DigitString(2) + UnixTimeStampUtc().ToString();
        }
        public static int UnixTimeStampUtc()
        {
            var currentTime = DateTime.Now;
            var zuluTime = currentTime.ToUniversalTime();
            var unixEpoch = new DateTime(1970, 1, 1);
            var unixTimeStamp = (Int32)(zuluTime.Subtract(unixEpoch)).TotalSeconds;
            return unixTimeStamp;
        }
        public static string Create7DigitString(int xter)
        {
            var rng = new Random();
            var builder = new StringBuilder();
            while (builder.Length < xter)
            {
                builder.Append(rng.Next(10).ToString());
            }
            var refNumber = builder.ToString();
            return refNumber;
        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        public enum AppStatus
        {
            customerOrder = 0,
            Pending = 1,
            Shipped = 2,
            Fulfilled = 3,
            Repaying = 4,
            Completed = 5,
            Recovered = 6
        }

    }
}
