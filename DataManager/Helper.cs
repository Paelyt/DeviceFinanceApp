using DeviceFinanceApp.Classes;
using DeviceFinanceApp.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DeviceFinanceApp.Classes.Utility;

namespace DeviceFinanceApp.DataManager
{
    public class Helper
    {
        private readonly MyHelper myHelper;
        private readonly ResponseModel _response;
        private readonly ErrorLogger logger;
        private readonly MyDataManager dMgr;
        private readonly IOptions<AppSettingParams> _appsetting;
        public Helper(IOptions<AppSettingParams> appsetting, DeviceFinanceContext _dbcontext)
        {
            logger = new ErrorLogger(_Destination: @"ErrorLog");
            _response = new ResponseModel();
            dMgr = new MyDataManager(_dbcontext);
            _appsetting = appsetting;
            myHelper = new MyHelper(appsetting);
        }
        public ResponseModel getStores()
        {
            try
            {
                var checkRef = dMgr.getStores();

                if (checkRef != null)
                {
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Request successful";
                    _response.Data = checkRef;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "No record found";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Not_found));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public ResponseModel getLoanDueAmount(string loanReference)
        {
            try
            {
                DateTime _today = getCurrentLocalDateTime();
                var chk = dMgr.getLoanDueAmountByLoanRef(loanReference, _today);

                if (chk.amountDue>=0)
                {
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Request successful";
                    _response.Data = chk;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "No record found";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Failed));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public ResponseModel SendSMS(sms payload)
        {
            try
            {
                int i = myHelper.SendSMS(payload.message, payload.phoneNumber);

                if (i>0)
                {
                    smsResponseDetails obj = new smsResponseDetails();
                    obj.sucess = true;
                    obj.status = "sms sent";

                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Request successful";
                    _response.Data = obj;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "No record found";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Failed));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
         public ResponseModel getProducts(int categoryID)
        {
            try
            {
                List<Products> checkRef = categoryID > 0 ? dMgr.getProductsByCategory(categoryID) : dMgr.getProducts();

                if (checkRef != null)
                {
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Transaction recieved successfully";
                    _response.Data = checkRef;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "No record found for the category";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Not_found));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
   
        public ResponseModel checkEligibility(eligibilityPayload Payload, string partnerId)
        {
            try
            {
                //Call Intelligra here
                eligibility eObj = new eligibility();
                dynamic obj = new JObject();
                obj.msisdn = Payload.refNumber;
                obj.telco = "MTN";
                var json = obj.ToString();
                string posturl = _appsetting.Value.INT_PREQUALIFY_INFO_URL;
                var data = myHelper.DoPost(json, posturl);

                if (data == null)
                {
                    eObj = null;
                }
                else
                { 
                    dynamic myResp = JObject.Parse(data);

                    if (myResp.score == null)
                    {
                        eObj= null;
                    }
                    eligibility eObj1 = new eligibility
                    {
                        creditLimit= myResp.creditLimit==null?0: myResp.creditLimit,
                        riskClass= myResp.riskClass == null ? 0 : myResp.riskClass,
                        msisdn = myResp.msisdn == null ? 0 : myResp.msisdn,
                        score = myResp.score == null ? 0 : myResp.score,
                        telco = myResp.telco == null ? 0 : myResp.telco,
                        status = myResp.status == null ? 0 : myResp.status,
                    };

                    eObj = eObj1;

                }
                if (eObj!=null)
                {
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Request successful";
                    _response.Data = eObj;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Transaction log error";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public ResponseModel OnboardCustomer(Onboard Payload, string partnerId)
        {
            try
            {
                var checkRef = dMgr.CheckIfReferenceExist(Payload.transactionReference);
                if (checkRef)
                {

                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Reference number already exist";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Ref_num_exist));

                    return _response;
                }

                var dvObj = dMgr.SaveDeviceApplication(Payload, partnerId);
                if (dvObj.ID > 0)
                {
                    DeliveryObj deliveryObj = new DeliveryObj();
                    BookCustomerOrder(Payload, out deliveryObj);

                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Request successful";
                    _response.Data = deliveryObj;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Transaction log error";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public ResponseModel activateBundle(bundle Payload, string partnerId)
        {
            try
            {

                // CALL Intelligra subscriptipion API
                //var dvObj = dMgr.SaveCustomerRepayment(Payload, partnerId);

                int myID = 0;
                if (myID > 0)
                {

                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Request successful";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Transaction log error";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public ResponseModel manageDevice(deviceUpdate payload, string partnerId, int deviceStatus)
        {
            try
            {
                var cObj = dMgr.GetCustomerOrderWithPhoneNumber(payload.servicePhoneNumber);
                if (cObj == null)
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Service number does not exist";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Reference_does_not_exist));
                }
                else
                {
                    dMgr.SaveDeviceManager(payload, partnerId, deviceStatus, cObj.ReferenceId);
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Request Successful";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                return _response;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
       
        public ResponseModel CustomerRepayment(customerRepayment Payload, string partnerId)
        {
            try
            {
                //var checkRef = dMgr.CheckIfReferenceExist(Payload.transactionReference);
                //if (checkRef)
                //{

                //    _response.IsSuccess = false;
                //    _response.ResponseDescription = "Reference number already exist";
                //    _response.Data = null;
                //    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Ref_num_exist));

                //    return _response;
                //}

                var dvObj = dMgr.SaveCustomerRepayment(Payload, partnerId);
                if (dvObj.ID > 0)
                {
                    
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Request successful";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Transaction log error";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }

        public CustomerOrder BookCustomerOrder(Onboard payload, out DeliveryObj deliveryObj)
        {
            CustomerOrder dObj = new CustomerOrder();
            deliveryObj = new DeliveryObj();
            try
            {
                dObj.ID = 0;

                MyDevice device = dMgr.GetLoanTenureWithProductCode(payload.productCode);
                if (device == null)
                    return dObj;

                int tenure = device.tenure;
                int storeId = 0;
                int.TryParse(payload.storeId, out storeId);

                DateTime today = Utility.getCurrentLocalDateTime();
                CustomerOrder myObj = new CustomerOrder
                {
                    AccountNumber = payload.accountNumber,
                    BankCode = payload.bankCode,
                    BVN = payload.bvn,
                    CallBackUrl = payload.callbackUrl,
                    CustomerActivatedPhoneNo = "",
                    CreatedDate = today,
                    CustomerName = payload.lastname + ' ' + payload.firstname + ' ' + payload.othernames,
                    CustomerPhoneNo = payload.phoneNumber,
                    DeliveryCode = Utility.GenerateDeliveryRef(),
                    DeviceName = device.deviceName,
                    DateMOdified = today,
                    CustomerAddress = payload.homeAddress,
                    IMEI = "",
                    IsVisible = 1,
                    OrderStatus_FK = (int)Utility.AppStatus.customerOrder,
                    StoreID = storeId,
                    ProductCode = payload.productCode,
                    ReferenceId = payload.transactionReference,
                    Tenure = tenure,
                    TransactionReference = payload.transactionReference,
                    TransactionDate = today.ToString("yyyy/MM/dd"),
                    ValueTime = today.ToString("H:mm:ss"),
                    ValueDate = today.ToString("yyyy/MM/dd"),
                    AddressVerified = 0
                    
                };

                dObj = dMgr.BookCustomerOrder(myObj);

                deliveryObj = dMgr.getDeliveryResponse(dObj,device);

            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                dObj.ID = 0;
            }
            return dObj;
        }
        public ResponseModel managefulfilledOrder(fulFillOrder payload)
        {
            try
            {
                var custOrder = dMgr.GetCustomerOrderWithdeliveryCode(payload.deliveryCode);
                if (custOrder == null)
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Delivery code doest not exist";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Reference_does_not_exist));
                    return _response;
                }

                var dvObj = dMgr.UpdateFulfilledCustomerOrder(custOrder, payload);
                if (dvObj.ID > 0)
                {
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Transaction updated successfully";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                    OrderFulfilmentNotification(custOrder.DeliveryCode);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Transaction log error";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public ResponseModel manageCustomerOrder(customerOrder payload)
        {
            try
            {
                var checkRef = dMgr.CheckIfCustomerOderRefExist(payload.ReferenceId);
                if (checkRef)
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Reference number already exist";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Ref_num_exist));
                    return _response;
                }

                var dvObj = dMgr.SaveCustomerOrder(payload);
                if (dvObj.ID > 0)
                {
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Transaction recieved successfully";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Transaction log error";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }

        public ResponseModel verifyCustAddressResponse(addressVerifyResponse payload)
        {
            try
            {

                var dvObj = dMgr.GetCustomerOderbyRefenceID(payload.transactionReference);
                if (dvObj != null)
                {
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Transaction updated successfully";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                    dMgr.UpdateAddressVerification(dvObj, payload);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Transaction log error";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }

        public ResponseModel verifyCustomerAddress(CustomerOrder cObj)
        {
            try
            {
              
                if (cObj != null)
                {
                    addressVerifyObj payload = new addressVerifyObj
                    {
                        transactionReference = cObj.TransactionReference,
                        AddressLGA = cObj.LGA,
                        AddressState = cObj.StateOfResidence,
                        customerAddress = cObj.CustomerAddress,
                        customerName = cObj.CustomerName,
                        customerPhone = cObj.CustomerPhoneNo
                    };

                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Request successful";
                    _response.Data = cObj;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                   
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Transaction log error";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                }


            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public ResponseModel validateOrderfulfillment(string deliveryCode)
        {
            try
            {
                var cObj = dMgr.GetCustomerOrderDetails(deliveryCode);
                if (cObj == null)
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Delivery Code does not exist";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Reference_does_not_exist));
                }
                else
                {
                    _response.IsSuccess = cObj.orderStatusFK != (int)AppStatus.Fulfilled ? true : false;
                    _response.ResponseDescription = cObj.orderStatusFK != (int)AppStatus.Fulfilled ? "Request successful" : "Delivery code has been used.";
                    _response.ResponseCode = cObj.orderStatusFK != (int)AppStatus.Fulfilled ? string.Format("{0:00}", ((int)responsecode.Success)) : string.Format("{0:00}", ((int)responsecode.Delivery_code_already_used_error));
                    _response.Data = cObj.orderStatusFK != (int)AppStatus.Fulfilled ? cObj : null;
                }
                return _response;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public ResponseModel IMEIChange(deviceUpdate payload, string partnerId)
        {
            try
            {
                // var cObj = dMgr.GetCustomerOrderWithdeliveryCode(payload.deliveryCode);
                var cObj = dMgr.GetCustomerOrderWithPhoneNumber(payload.servicePhoneNumber);
                if (cObj == null)
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Service Phone Number does not exist";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Reference_does_not_exist));
                }
                else
                {
                    UpdateIMEI(cObj, payload);
                    manageDevice( payload, partnerId,(int)deviceStatus.Changed);
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "IMEI successfully updated.";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                return _response;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public int UpdateIMEI(CustomerOrder cObj, deviceUpdate payload)
        {
            int update = 0;
            try
            {
                dMgr.UpdateCustomerOrder(cObj, payload.imei);

                DateTime today = Utility.getCurrentLocalDateTime();
                DeviceChange devObj = new DeviceChange();
                devObj.IMEI = payload.imei;
                devObj.CustomerOrder_FK = cObj.ID;
                devObj.TransDate = today.ToString("yyyy/MM/dd");
                devObj.CreatedDate = today;
                devObj.IsVisible = 1;
                devObj.Remarks = "IMEI Changed";
                dMgr.SaveIMEI(devObj);
                update = 1;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
            }
            return update;
        }
        public ResponseModel OrderFulfilmentNotification(string deliveryCode)
        {
            try
            {
                // var cObj = dMgr.GetCustomerOrderWithReferenceId(referenceId);
                var cObj = dMgr.GetCustomerOrderWithdeliveryCode(deliveryCode);
                if (cObj == null)
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Delivery Code does not exist";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Reference_does_not_exist));
                }
                else
                {
                    var fObj = PartnerCallBack(cObj);
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "Request Successful";
                    _response.Data = fObj;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                    var json = JsonConvert.SerializeObject(_response);

                    myhttpManager.DoPost(json, fObj.callbackUrl);
                }
                return _response;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public ResponseModel RefurbishedDevice(deviceUpdate payload,string partnerId)
        {
            try
            {
                var cObj = dMgr.GetCustomerOrderWithPhoneNumber(payload.servicePhoneNumber);
                if (cObj == null)
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Delivery Code does not exist";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Reference_does_not_exist));
                }
                else
                {
                    UpdateRefurbishedDevice(cObj, payload);
                    manageDevice(payload, partnerId,(int) deviceStatus.Refurbished);
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "IMEI successfuly updated.";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                return _response;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public ResponseModel SaveDevice4Repairs(deviceUpdate payload, string partnerId)
        {
            try
            {
                var cObj = dMgr.GetCustomerOrderWithPhoneNumber(payload.servicePhoneNumber);
                if (cObj == null)
                {
                    _response.IsSuccess = false;
                    _response.ResponseDescription = "Delivery Code does not exist";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Reference_does_not_exist));
                }
                else
                {
                    //SaveDevice4Repair(cObj, payload);
                    manageDevice( payload,partnerId,(int)deviceStatus.Repair_Log);
                    _response.IsSuccess = true;
                    _response.ResponseDescription = "IMEI successfuly updated.";
                    _response.Data = null;
                    _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.Success));
                }
                return _response;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                _response.IsSuccess = false;
                _response.ResponseDescription = "An exception has occured";
                _response.Data = null;
                _response.ResponseCode = string.Format("{0:00}", ((int)responsecode.General_failure));

            }
            return _response;

        }
        public int SaveDevice4Repair(CustomerOrder cObj, refurbished payload)
        {
            int update = 0;
            try
            {
                DateTime today = Utility.getCurrentLocalDateTime();
                RefurbishedDevice devObj = new RefurbishedDevice();
                devObj.ExpectedCollectionDate = payload.expectedCollectionDate;
                devObj.RepairCenterID = payload.repairCenterID;
                devObj.CustomerOrder_FK = cObj.ID;
                devObj.PhoneNumber = payload.phoneNumber;
                devObj.SubmissionDate = payload.submissionDate;
                devObj.CollectionDate = payload.collectionDate;
                devObj.IsVisible = 1;
                devObj.ActiveFlag = 0;
                devObj.Remarks = payload.remarks;
                devObj.DateCreated = today;
                devObj.DateModified = today;
                dMgr.SaveRefurbishedDevice(devObj);
                update = 1;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
            }
            return update;
        }
        public int UpdateRefurbishedDevice(CustomerOrder cObj, deviceUpdate payload)
        {
            int update = 0;
            try
            {
                DateTime today = Utility.getCurrentLocalDateTime();
                RefurbishedDevice devObj = new RefurbishedDevice();

                devObj.PhoneNumber = payload.servicePhoneNumber;
                devObj.SubmissionDate = payload.submissionDate;
                devObj.CollectionDate = payload.collectionDate;
                devObj.IsVisible = 1;
                devObj.Remarks = payload.remarks;
                devObj.DateModified = today;
                dMgr.UpdateOpenRefurbishedDevice(devObj);
                update = 1;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
            }
            return update;
        }

        public fulfilOrderObj PartnerCallBack(CustomerOrder cObj)
        {

            fulfilOrderObj devObj = new fulfilOrderObj();
            try
            {
                DateTime today = Utility.getCurrentLocalDateTime();


                devObj.activatedPhoneNumber = cObj.CustomerActivatedPhoneNo;
                devObj.imei = cObj.IMEI;
                devObj.referenceId = cObj.ReferenceId;
                devObj.remarks = "";
                devObj.transDate = today.ToString();
                devObj.storedID = cObj.StoreID.ToString();
                devObj.callbackUrl = cObj.CallBackUrl==null?"": cObj.CallBackUrl;
                devObj.deliveryCode = cObj.DeliveryCode;

                return devObj;
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                return null;
            }

        }
    }
}
