using DeviceFinanceApp.Classes;
using DeviceFinanceApp.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DeviceFinanceApp.Classes.Utility;

namespace DeviceFinanceApp.DataManager
{
    public class MyDataManager
    {
        private readonly ResponseModel resModel;
        private readonly DeviceFinanceContext _dbcontext;
        private readonly ErrorLogger logger;
        public MyDataManager(DeviceFinanceContext dbContext)
        {
            _dbcontext = dbContext;
            logger = new ErrorLogger(_Destination: @"ErrorLog");
            resModel = new ResponseModel();
        }
        public async Task<dynamic> ValidatePartner(string partnerId, string partnerKey)
        {
            try
            {
                var getpartner = await _dbcontext.Partner
                    .Where(x => x.PartnerID == partnerId && x.PartnerKey == partnerKey && x.IsVisible == 1).AnyAsync();


                if (getpartner)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return false;
            }

        }
        public async Task<dynamic> ValidateUtilityPartner(string bearerToken)
        {
            try
            {
                var getpartner = await _dbcontext.UtilityPartner
                    .Where(x => x.BearerToken == bearerToken  && x.IsVisible == 1).AnyAsync();


                if (getpartner)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return false;
            }

        }
        public bool CheckSignature(string partnerId, string partnerKey, string signature, out string respDescription)
        {
            respDescription = "";
            try
            {
                var getpartner = _dbcontext.Partner
                    .Where(x => x.PartnerID == partnerId && x.PartnerKey == partnerKey && x.IsVisible == 1).FirstOrDefault();


                if (getpartner != null)
                {
                    CryptographyManager cryp = new CryptographyManager();
                    var plainText = (partnerId + partnerKey + getpartner.BusinessEmail);

                    if (!cryp.VerifyHash(plainText, signature, CryptographyManager.HashName.SHA512))
                    {
                        respDescription = "Invalid Data: Hash value cannot be verified.";
                        return false;
                    }
                    return true;
                }
                respDescription = "You are not Authorized. Wrong PartnerId or ParterKey ";
                return false;

            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return false;
            }

        }
        public async Task<dynamic> ValidateOEMPartner(string partnerId, string partnerKey)
        {
            try
            {
                var getpartner = await _dbcontext.OEM
                    .Where(x => x.PartnerID == partnerId && x.PartnerKey == partnerKey && x.IsVisible == 1).AnyAsync();


                if (getpartner)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return false;
            }

        }
        public List<Stores> getStores()
        {
            try
            {
                var results = (from store in _dbcontext.OEM_Store
                               join lga in _dbcontext.OEM_LGA on store.OEM_LGA_FK equals lga.LGAId
                               join state in _dbcontext.OEM_State on lga.OEM_State_FK equals state.ID
                               select new Stores
                               {
                                   StoreId = store.StoreID,
                                   StoreAddress = store.StoreAddress,
                                   StoreLGA = lga.Name,
                                   StoreName = store.StoreName,
                                   StoreState = state.Name,
                                   ContactName = store.ContactName,
                                   ContactPhone = store.ContactPhone,
                                   ContactEmail = store.ContactEmail
                               }).ToList();

                return results;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }

        public DeliveryObj getDeliveryResponse(CustomerOrder custObj, MyDevice device)
        {
            try
            {
                var results = (from store in _dbcontext.OEM_Store
                               join lga in _dbcontext.OEM_LGA on store.OEM_LGA_FK equals lga.LGAId
                               join state in _dbcontext.OEM_State on lga.OEM_State_FK equals state.ID
                               where store.StoreID == custObj.StoreID
                               select new DeliveryObj
                               {
                                   storeId = store.StoreID,
                                   storeAddress = store.StoreAddress,
                                   storeLGA = lga.Name,
                                   storeName = store.StoreName,
                                   storeState = state.Name,
                                   customerName = custObj.CustomerName,
                                   deliveryCode = custObj.DeliveryCode,
                                   tenure = custObj.Tenure,
                                   phoneNumber = custObj.CustomerPhoneNo,
                                   transactionReference = custObj.ReferenceId,
                                   deviceName = custObj.DeviceName,
                                   monthlyRepayment = device.monthlyRepayment,
                                   upfrontRepayment = device.upfrontPayment
                               }).FirstOrDefault();

                return results;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }

        public List<Products> getProducts()
        {
            try
            {
                var results = (from dev in _dbcontext.Device
                               join devPlan in _dbcontext.DevicePlan on dev.ID equals devPlan.Device_FK
                               join priceModel in _dbcontext.PricingModel on devPlan.PricingModel_FK equals priceModel.ID
                               join devImage in _dbcontext.DeviceImage on devPlan.Device_FK equals devImage.Device_FK
                               join brand in _dbcontext.DeviceBrand on dev.DeviceBrand_FK equals brand.ID
                               join deviceType in _dbcontext.DeviceType on brand.DeviceType_FK equals deviceType.ID

                               select new Products
                               {
                                   Id = devPlan.ID,
                                   deviceCategory = deviceType.Name,
                                   deviceBrand = brand.Name,
                                   deviceImage1 = devImage.ImageUrl1,
                                   deviceImage2 = devImage.ImageUrl2,
                                   deviceImage3 = devImage.ImageUrl3,
                                   deviceImage4 = devImage.ImageUrl4,
                                   deviceName = devPlan.DeviceplanName,
                                   deviceSpecfications = dev.GeneralDescription,
                                   deviceScreensize = dev.Screensize,
                                   deviceProcessor = dev.Processor,
                                   deviceRam = dev.Ram,
                                   deviceRom = dev.Rom,
                                   loanCode = devPlan.LoanID,
                                   loanTenure = priceModel.Tenure,
                                   oldPrice = devPlan.OldSellingPrice,
                                   monthlyRepayment = devPlan.MonthlyPayment,
                                   sellingPrice = devPlan.SellingPrice,
                                   upfrontPayment = priceModel.UpfrontPayment
                               }).ToList();

                return results;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public List<Products> getProductsByCategory(int categoryID)
        {
            try
            {
                var results = (from dev in _dbcontext.Device
                               join devPlan in _dbcontext.DevicePlan on dev.ID equals devPlan.Device_FK
                               join priceModel in _dbcontext.PricingModel on devPlan.PricingModel_FK equals priceModel.ID
                               join devImage in _dbcontext.DeviceImage on devPlan.Device_FK equals devImage.Device_FK
                               join brand in _dbcontext.DeviceBrand on dev.DeviceBrand_FK equals brand.ID
                               join deviceType in _dbcontext.DeviceType on brand.DeviceType_FK equals deviceType.ID

                               where deviceType.ID == categoryID
                               select new Products
                               {
                                   Id = devPlan.ID,
                                   deviceCategory = deviceType.Name,
                                   deviceBrand = brand.Name,
                                   deviceImage1 = devImage.ImageUrl1,
                                   deviceImage2 = devImage.ImageUrl2,
                                   deviceImage3 = devImage.ImageUrl3,
                                   deviceImage4 = devImage.ImageUrl4,
                                   deviceName = devPlan.DeviceplanName,
                                   deviceSpecfications = dev.GeneralDescription,
                                   deviceScreensize = dev.Screensize,
                                   deviceProcessor = dev.Processor,
                                   deviceRam = dev.Ram,
                                   deviceRom = dev.Rom,
                                   loanCode = devPlan.LoanID,
                                   loanTenure = priceModel.Tenure,
                                   oldPrice = devPlan.OldSellingPrice,
                                   monthlyRepayment = devPlan.MonthlyPayment,
                                   sellingPrice = devPlan.SellingPrice,
                                   upfrontPayment = priceModel.UpfrontPayment
                               }).ToList();

                return results;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public dynamic getLoanDueAmountByLoanRef(string loanReference, DateTime _today)
        {
            try
            {
                loanDueAmountResponseDetails obj = new loanDueAmountResponseDetails();
                obj.loanReference = loanReference;
                obj.amountDue = -1;

                var custLedger = _dbcontext.CustomerLedger.Where(x => x.LoanReference == loanReference && x.TransDate<= _today
                && x.RepaymentFlag!=1 ).ToList();

                if (custLedger.Count > 0)
                {
                    obj.amountDue = 0;
                   var sum = custLedger.Select(c => c.Debit).Sum();
                    obj.amountDue = sum;
                }                
                return obj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return false;
            }
        }
        
        public bool CheckIfReferenceExist(string referenceId)
        {
            try
            {
                var checkReference = _dbcontext.DeviceApplication.Where(x => x.TransactionReference == referenceId).ToList();
                if (checkReference.Count() > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return false;
            }
        }
        public int GetPricingModelById(int pricingModelId)
        {
            try
            {
                var pricingModel = _dbcontext.PricingModel.Where(x => x.ID == pricingModelId).FirstOrDefault();

                if (pricingModel == null)
                    return 0;

                return pricingModel.Tenure;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return 0;
            }
        }
        public MyDevice GetLoanTenureWithProductCode(string productCode)
        {
            MyDevice device = new MyDevice();
            try
            {
                var devicePlan = _dbcontext.DevicePlan.Where(x => x.LoanID == productCode).FirstOrDefault();

                if (devicePlan == null)
                    return null;

                device.deviceName = devicePlan.DeviceplanName;
                device.monthlyRepayment = devicePlan.MonthlyPayment.ToString();
                device.upfrontPayment = devicePlan.UpfrontPayment.ToString();
                device.tenure = GetPricingModelById(devicePlan.PricingModel_FK);
                device.IMEI = "";
                return device;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public DevicePlan GetDevicePlanWithProductCode(string productCode)
        {
            try
            {
                var devicePlan = _dbcontext.DevicePlan.Where(x => x.LoanID == productCode).FirstOrDefault();

                return devicePlan;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public bool CheckIfCustomerOderRefExist(string referenceId)
        {
            try
            {
                var checkReference = _dbcontext.CustomerOrder.Where(x => x.ReferenceId == referenceId).ToList();
                if (checkReference.Count() > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return false;
            }
        }
        public CustomerOrder GetCustomerOderbyRefenceID(string referenceId)
        {
            try
            {
                var custOrder = _dbcontext.CustomerOrder.Where(x => x.ReferenceId == referenceId).FirstOrDefault();


                return custOrder;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public DeviceApplication GetDeviceApplication(string referenceId)
        {
            try
            {
                var devObj = _dbcontext.DeviceApplication.Where(x => x.TransactionReference == referenceId).FirstOrDefault();

                return devObj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public validateOrderObj GetCustomerOrderDetails(string deliveryCode)
        {
            try
            {


                var results = (from custOrder in _dbcontext.CustomerOrder
                               join devPlan in _dbcontext.DevicePlan on custOrder.ProductCode equals devPlan.LoanID
                               join priceModel in _dbcontext.PricingModel on devPlan.PricingModel_FK equals priceModel.ID
                               join dev in _dbcontext.Device on devPlan.Device_FK equals dev.ID
                               join brand in _dbcontext.DeviceBrand on dev.DeviceBrand_FK equals brand.ID
                               join astatus in _dbcontext.ApplicationStatus on custOrder.OrderStatus_FK equals astatus.ID

                               where custOrder.DeliveryCode == deliveryCode
                               select new validateOrderObj
                               {
                                   Id = custOrder.ID,
                                   orderStatusFK = custOrder.OrderStatus_FK,
                                   orderStatus = astatus.Description,
                                   customerName = custOrder.CustomerName,
                                   customerAddress = custOrder.CustomerAddress,
                                   customerPhoneNo = custOrder.CustomerPhoneNo,
                                   device = dev.Devicename,
                                   deviceBrand = brand.Name,
                                   deviceplanName = devPlan.DeviceplanName,
                                   loanCode = custOrder.ProductCode,
                                   deliveryCode = custOrder.DeliveryCode,
                                   referenceId = custOrder.ReferenceId,
                                   loanTenure = priceModel.Tenure.ToString(),
                                   accountNumber = custOrder.AccountNumber,
                                   bankCode = custOrder.BankCode,
                                   storeID = custOrder.StoreID,
                                   applicationDate = custOrder.CreatedDate.ToString("yyyy/MM/dd"),
                                   bvn = custOrder.BVN,
                                   bvn_photo = custOrder.BVNPhoto,
                                   lga = custOrder.LGA,
                                   nextOfKinFullName = custOrder.NextOfKinFullName,
                                   nextofkinPhoneNumber = custOrder.NextofkinPhoneNumber,
                                   nin = custOrder.NIN,
                                   state = custOrder.StateOfResidence
                               }).FirstOrDefault();

                return results; 
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public CustomerOrder GetCustomerOrder(string referenceId)
        {
            try
            {
                var custObj = _dbcontext.CustomerOrder.Where(x => x.ReferenceId == referenceId).FirstOrDefault();

                return custObj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public CustomerOrder GetCustomerOrderWithReferenceId(string referenceId)
        {
            try
            {
                var custObj = _dbcontext.CustomerOrder.Where(x => x.ReferenceId == referenceId).FirstOrDefault();

                return custObj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public CustomerOrder GetCustomerOrderWithPhoneNumber(string phoneNumber)
        {
            try
            {
                var custObj = _dbcontext.CustomerOrder.Where(x => x.CustomerActivatedPhoneNo == phoneNumber).FirstOrDefault();

                return custObj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public int UpdateOpenRefurbishedDevice(RefurbishedDevice refObj)
        {
            int update = 0;
            try
            {
                var custObj = _dbcontext.RefurbishedDevice.Where(x => x.PhoneNumber == refObj.PhoneNumber && x.ActiveFlag == 0).FirstOrDefault();

                custObj.ActiveFlag = 1;
                custObj.Remarks = refObj.Remarks;
                custObj.DateModified = refObj.DateModified;
                custObj.CollectionDate = refObj.CollectionDate;
                _dbcontext.SaveChanges();
                update = 1;
                return update;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return update;
            }
        }
        public CustomerOrder GetCustomerOrderWithdeliveryCode(string deliveryCode)
        {
            try
            {
                var custObj = _dbcontext.CustomerOrder.Where(x => x.DeliveryCode == deliveryCode).FirstOrDefault();

                return custObj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
        }
        public CustomerOrder UpdateCustomerOrder(CustomerOrder cObj, string imei)
        {
            CustomerOrder dObj = new CustomerOrder();
            try
            {
                dObj = _dbcontext.CustomerOrder.Where(x => x.ID == cObj.ID).FirstOrDefault();
                dObj.IMEI = imei;
                dObj.DateMOdified = Utility.getCurrentLocalDateTime();
                _dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }
            return dObj;
        }

        public async Task<dynamic> CheckIfRefExist(string referenceId)
        {
            try
            {
                var checkReference = await _dbcontext.DeviceApplication.Where(x => x.TransactionReference == referenceId).AnyAsync();
                if (checkReference)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return false;
            }
        }
        public CustomerRepayment SaveCustomerRepayment(customerRepayment payload, string partnerID)
        {
            CustomerRepayment dObj = new CustomerRepayment();
            try
            {
                double amount = 0;
                double.TryParse(payload.amount, out amount);
                DateTime today = Utility.getCurrentLocalDateTime();
                CustomerRepayment myObj = new CustomerRepayment
                {
                    AccountNumber = payload.accountNumber,
                    AccountName = payload.accountName,
                    PartnerID = partnerID,
                    Amount = amount,
                    DateCreated = today,
                    DateModified = today,
                    CustomerReference = payload.customerReference,
                    IsVisible = 1
                };
                _dbcontext.CustomerRepayment.Add(myObj);
                _dbcontext.SaveChanges();
                dObj = myObj;
            }
            catch (Exception ex)
            {
                dObj.ID = 0;
                logger.WriteLog(ex);
            }
            return dObj;
        }
        public DeviceManager SaveDeviceManager(deviceUpdate payload, string partnerID, int deviceStatus, string refNumber)
        {
            DeviceManager dObj = new DeviceManager();
            try
            {

                DateTime today = Utility.getCurrentLocalDateTime();
                DeviceManager myObj = new DeviceManager
                {
                    CreatedDate = today,
                    DeviceStatus_FK = deviceStatus,
                    PartnerID = partnerID,
                    IMEI = payload.imei,
                    ModifiedDate = today,
                    PhoneNumber = payload.servicePhoneNumber,
                    TransactionReference = payload.transactionReference,
                    IsVisible = 1,
                    ReferenceId = refNumber,
                    Remarks = payload.remarks,
                    SubmissionDate= payload.submissionDate,
                    ExpectedCollectionDate= payload.expectedCollectionDate,
                    CollectionDate= payload.collectionDate
                };
                _dbcontext.DeviceManager.Add(myObj);
                _dbcontext.SaveChanges();
                dObj = myObj;
            }
            catch (Exception ex)
            {
                dObj.ID = 0;
                logger.WriteLog(ex);
            }
            return dObj;
        }
        public DeviceApplication SaveDeviceApplication(Onboard payload, string partnerID)
        {
            DeviceApplication dObj = new DeviceApplication();
            try
            {
                DateTime today = Utility.getCurrentLocalDateTime();
                DeviceApplication myObj = new DeviceApplication
                {
                    AccountNumber = payload.accountNumber,
                    AccountName = payload.accountName,
                    AlternatePhoneNumber = payload.alternatePhoneNumber,
                    ApplicationStatus_FK = (int)Utility.AppStatus.customerOrder,
                    BankCode = payload.bankCode,
                    StoreID = payload.storeId,
                    BVN = payload.bvn,
                    HomeAddress = payload.homeAddress,
                    PersonalEmailAddress = payload.personalEmailAddress,
                    PhoneNumber = payload.phoneNumber,
                    DateCreated = today,
                    DateModified = today,
                    DateOfBirth = payload.dateOfBirth,
                    Firstname = payload.firstname,
                    Gender = payload.gender,
                    LGA = payload.lga,
                    Lastname = payload.lastname,
                    IMEI = "",
                    IsVisible = 1,
                    Othernames = payload.othernames,
                    StateOfOrigin = payload.stateOfOrigin,
                    ProductCode = payload.productCode,
                    Remarks = "",
                    TransactionReference = payload.transactionReference,
                    MeansOfId = payload.meansOfId,
                    PartnerID = partnerID,
                };
                _dbcontext.DeviceApplication.Add(myObj);
                _dbcontext.SaveChanges();
                dObj = myObj;
            }
            catch (Exception ex)
            {
                dObj.ID = 0;
                logger.WriteLog(ex);
            }
            return dObj;
        }

        public CustomerOrder SaveCustomerOrder(customerOrder payload)
        {
            CustomerOrder dObj = new CustomerOrder();
            try
            {
                var devObj = GetDeviceApplication(payload.ReferenceId);
                int storeId = 0;
                int.TryParse(payload.StoreID, out storeId);
                DateTime today = Utility.getCurrentLocalDateTime();
                CustomerOrder myObj = new CustomerOrder
                {
                    AccountNumber = devObj.AccountNumber,
                    BankCode = devObj.BankCode,
                    BVN = devObj.BVN,
                    CreatedDate = today,
                    CustomerName = payload.CustomerName,
                    CustomerPhoneNo = payload.CustomerPhoneNo,
                    DeliveryCode = payload.ReferenceId,// TO BE UPDATED
                    DateMOdified = today,
                    CustomerAddress = devObj.HomeAddress,
                    IsVisible = 1,
                    OrderStatus_FK = 0,
                    StoreID = storeId,
                    ProductCode = devObj.ProductCode,
                    ReferenceId = payload.ReferenceId,
                    Tenure = payload.Tenure,
                    ValueTime = today.ToString("H:mm:ss"),
                    ValueDate = today.ToString("yyyy/MM/dd"),
                    CustomerActivatedPhoneNo = ""

                };
                _dbcontext.CustomerOrder.Add(myObj);
                _dbcontext.SaveChanges();
                dObj = myObj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                dObj.ID = 0;
            }
            return dObj;
        }
        public CustomerOrder BookCustomerOrder(CustomerOrder custOrderObj)
        {
            CustomerOrder dObj = new CustomerOrder();
            try
            {

                _dbcontext.CustomerOrder.Add(custOrderObj);
                _dbcontext.SaveChanges();
                dObj = custOrderObj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                dObj.ID = 0;
            }
            return dObj;
        }

        public DeviceChange SaveIMEI(DeviceChange devObj)
        {
            DeviceChange dObj = new DeviceChange();
            try
            {

                _dbcontext.DeviceChange.Add(devObj);
                _dbcontext.SaveChanges();
                dObj = devObj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                dObj.ID = 0;
            }
            return dObj;
        }
        public RefurbishedDevice SaveRefurbishedDevice(RefurbishedDevice devObj)
        {
            RefurbishedDevice dObj = new RefurbishedDevice();
            try
            {

                _dbcontext.RefurbishedDevice.Add(devObj);
                _dbcontext.SaveChanges();
                dObj = devObj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                dObj.ID = 0;
            }
            return dObj;
        }
        public CustomerOrder UpdateFulfilledCustomerOrder(CustomerOrder custOrderObj, fulFillOrder fObj)
        {
            CustomerOrder dObj = new CustomerOrder();
            try
            {
                dObj = _dbcontext.CustomerOrder.Where(x => x.ReferenceId == custOrderObj.ReferenceId).FirstOrDefault();
                if (dObj != null)
                {
                    dObj.OrderStatus_FK = (int)AppStatus.Fulfilled;
                    dObj.IMEI = fObj.imei;
                    dObj.TransactionReference = fObj.transactionReference;
                    dObj.CustomerActivatedPhoneNo = fObj.servicePhoneNo;
                    dObj.TransactionDate = fObj.transDate;
                    dObj.DateMOdified = Utility.getCurrentLocalDateTime();
                    _dbcontext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                dObj.ID = 0;
            }
            return dObj;
        }

        public CustomerOrder UpdateAddressVerification(CustomerOrder custOrderObj, addressVerifyResponse fObj)
        {
            CustomerOrder dObj = new CustomerOrder();
            try
            {
                dObj = _dbcontext.CustomerOrder.Where(x => x.TransactionReference == custOrderObj.ReferenceId).FirstOrDefault();
                if (dObj != null)
                {
                    dObj.AddressVerified = fObj.verified;
                    dObj.AddressVerificationRemark = fObj.remark;
                    dObj.DateMOdified = Utility.getCurrentLocalDateTime();
                    _dbcontext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                dObj.ID = 0;
            }
            return dObj;
        }
    }
}
