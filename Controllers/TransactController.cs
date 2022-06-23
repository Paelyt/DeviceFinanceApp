using DeviceFinanceApp.Classes;
using DeviceFinanceApp.DataManager;
using DeviceFinanceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DeviceFinanceApp.Classes.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeviceFinanceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactController : ControllerBase
    {
        private MyDataManager myDMgr;
        private ResponseModel genericResp;
        private Helper helper;
        private readonly ErrorLogger logger;
        private readonly DeviceFinanceContext _dbcontext;
        private readonly IOptions<AppSettingParams> _appsetting;

        public TransactController(IOptions<AppSettingParams> appsetting,DeviceFinanceContext dbcontext)
        {
            logger = new ErrorLogger(_Destination: @"ErrorLog");
            genericResp = new ResponseModel();
            myDMgr = new MyDataManager(dbcontext);
            helper = new Helper(appsetting,dbcontext);
        }
        //  private readonly ErrorLogger logger;
        // GET: api/<TransactController>

        [HttpPost]
        [Route("checkEligibilty")] //PARTNERS
        public async Task<IActionResult> checkEligibilty(eligibilityPayload payload)
        {
            try
            {
               // var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);
                Request.Headers.TryGetValue("signature", out var signature);
                genericResp.IsSuccess = false;
                genericResp.Data = null;
                CryptographyManager cryp = new CryptographyManager();
                var plainText = (partnerId + partnerKey + payload.refNumber);

                if (!cryp.VerifyHash(plainText, signature.ToString(), CryptographyManager.HashName.SHA512))
                {
                    genericResp.ResponseDescription = "Invalid Data: Hash value cannot be verified.";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_hash));
                    return StatusCode(401, genericResp);
                }

                var checkPartner = await myDMgr.ValidatePartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.checkEligibility(payload, partnerId);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }

        //public IHttpActionResult
        [HttpGet]
        [Route("getProducts")] //PARTNERS
        public async Task<IActionResult> getProducts(string categoryId)
        {
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);
                Request.Headers.TryGetValue("signature", out var signature);
                genericResp.IsSuccess = false;
                genericResp.Data = null;

                var partnerObj = await myDMgr.ValidatePartner(partnerId, partnerKey);

                if (!partnerObj)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }


                string respDescription = "";
                var checkPartner = myDMgr.CheckSignature(partnerId, partnerKey, signature, out respDescription);
                if (!checkPartner)
                {
                    genericResp.ResponseDescription = respDescription;
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }
                int deviceMake = 0;
                int.TryParse(categoryId, out deviceMake);
                var response = helper.getProducts(deviceMake);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }
        [HttpGet]
        [Route("getStores")] //PARTNERS
        public async Task<IActionResult> getStores(string categoryID)
        {
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);
                Request.Headers.TryGetValue("signature", out var signature);
                genericResp.IsSuccess = false;
                genericResp.Data = null;

                var partnerObj = await myDMgr.ValidatePartner(partnerId, partnerKey);

                if (!partnerObj)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }


                string respDescription = "";
                var checkPartner = myDMgr.CheckSignature(partnerId, partnerKey, signature, out respDescription);
                if (!checkPartner)
                {
                    genericResp.ResponseDescription = respDescription;
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }
                int deviceMake = 0;
                int.TryParse(categoryID, out deviceMake);
                var response = helper.getStores();
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }
        [HttpPost]
        [Route("onboardCustomer")] //PARTNERS
        public async Task<IActionResult> onboardCustomer(Onboard payload)
        {
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);
                Request.Headers.TryGetValue("signature", out var signature);
                genericResp.IsSuccess = false;
                genericResp.Data = null;
                CryptographyManager cryp = new CryptographyManager();
                var plainText = (partnerId + partnerKey + payload.transactionReference);

                if (!cryp.VerifyHash(plainText, signature.ToString(), CryptographyManager.HashName.SHA512))
                {
                    // logger.WriteActivity("hashvalue was NOT verified:");
                    // WebLog.Log("Plain text: " + plainText);

                    genericResp.ResponseDescription = "Invalid Data: Hash value cannot be verified.";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_hash));
                    return StatusCode(401, genericResp);
                }

                var checkPartner = await myDMgr.ValidatePartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.OnboardCustomer(payload, partnerId);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }


        [HttpPost]
        [Route("customerRepayment")] //PARTNERS
        public async Task<IActionResult> customerRepayment(customerRepayment payload)
        {
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);
                Request.Headers.TryGetValue("signature", out var signature);
                genericResp.IsSuccess = false;
                genericResp.Data = null;
                CryptographyManager cryp = new CryptographyManager();
                var plainText = (partnerId + partnerKey + payload.transactionReference);

                if (!cryp.VerifyHash(plainText, signature.ToString(), CryptographyManager.HashName.SHA512))
                {
                    // logger.WriteActivity("hashvalue was NOT verified:");
                    // WebLog.Log("Plain text: " + plainText);

                    genericResp.ResponseDescription = "Invalid Data: Hash value cannot be verified.";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_hash));
                    return StatusCode(401, genericResp);
                }

                var checkPartner = await myDMgr.ValidatePartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.CustomerRepayment(payload, partnerId);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }

        [HttpGet]
        [Route("validateCustomerOrder")]//OEM
        public async Task<IActionResult> validateCustomerOrder(string deliveryCode)
        {
            // 
            try
            {
                var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);

                genericResp = new ResponseModel();



                genericResp.IsSuccess = false;
                genericResp.Data = null;
                var checkPartner = await myDMgr.ValidateOEMPartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.validateOrderfulfillment(deliveryCode);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }

        [HttpPost]
        [Route("fulFillOrder")]//OEM
        public async Task<IActionResult> fulFillOrder(fulFillOrder payload)
        {
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);


                genericResp = new ResponseModel();



                genericResp.IsSuccess = false;
                genericResp.Data = null;
                var checkPartner = await myDMgr.ValidateOEMPartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.managefulfilledOrder(payload);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }


        [HttpPost]
        [Route("verifyAddress")]//OEM
        public async Task<IActionResult> verifyAddress(addressVerifyObj payload)
        {
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);


                genericResp = new ResponseModel();



                genericResp.IsSuccess = false;
                genericResp.Data = null;
                var checkPartner = await myDMgr.ValidateOEMPartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }
                var dvObj = myDMgr.GetCustomerOderbyRefenceID(payload.transactionReference);
                var response = helper.verifyCustomerAddress(dvObj);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }
        [HttpPost]
        [Route("verifyAddressResponse")]//OEM
        public async Task<IActionResult> verifyAddressResponse(addressVerifyResponse payload)
        {
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);


                genericResp = new ResponseModel();



                genericResp.IsSuccess = false;
                genericResp.Data = null;
                var checkPartner = await myDMgr.ValidateOEMPartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }
                
                var response = helper.verifyCustAddressResponse(payload);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }
        [HttpPost]
        [Route("imeiChange")]//OEM
        public async Task<IActionResult> imeiChange(deviceUpdate payload)
        {
            // 
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);

                genericResp = new ResponseModel();



                genericResp.IsSuccess = false;
                genericResp.Data = null;
                var checkPartner = await myDMgr.ValidateOEMPartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.IMEIChange(payload, partnerId);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }
        [HttpPost]
        [Route("repairDeviceLog")]//OEM
        public async Task<IActionResult> repairDeviceLog(deviceUpdate payload)
        {
            // 
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);

                genericResp = new ResponseModel();



                genericResp.IsSuccess = false;
                genericResp.Data = null;
                var checkPartner = await myDMgr.ValidateOEMPartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.manageDevice(payload, partnerId, (int)deviceStatus.Repair_Log);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }
        [HttpPost]
        [Route("repairDevice")]//OEM
        public async Task<IActionResult> repairDevice(deviceUpdate payload)
        {
            // 
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);

                genericResp = new ResponseModel();



                genericResp.IsSuccess = false;
                genericResp.Data = null;
                var checkPartner = await myDMgr.ValidateOEMPartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.manageDevice(payload, partnerId, (int)deviceStatus.Refurbished);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }

        [HttpPost]
        [Route("recoverDevice")]//OEM
        public async Task<IActionResult> recoverDevice(deviceUpdate payload)
        {
            // 
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);

                genericResp = new ResponseModel();



                genericResp.IsSuccess = false;
                genericResp.Data = null;
                var checkPartner = await myDMgr.ValidateOEMPartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.manageDevice(payload, partnerId, (int)deviceStatus.Recovered);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }

        [HttpPost]
        [Route("sendFulfilOrder")]//PARTNER
        public async Task<IActionResult> sendFulfilOrder(ResponseModel payload)
        {
            // 
            try
            {

                genericResp = new ResponseModel();

                genericResp.IsSuccess = false;
                genericResp.Data = null;
                var checkPartner = await myDMgr.ValidateOEMPartner("", "");

                //if (!checkPartner)
                //{
                //    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                //    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                //    return StatusCode(401, genericResp);
                //}
                //  var json = JsonConvert.SerializeObject(payload);
                if (payload.ResponseCode == "00")
                {
                    return Ok(payload);
                }
                return BadRequest(payload);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }

        [HttpPost]
        [Route("activateBundle")] //OEM
        public async Task<IActionResult> activateBundle(bundle payload)
        {
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);

                genericResp.IsSuccess = false;
                genericResp.Data = null;


                var checkPartner = await myDMgr.ValidatePartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.activateBundle(payload, partnerId);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }
        [HttpPost]
        [Route("lockDevice")] //OEM
        public async Task<IActionResult> lockDevice(deviceUpdate payload)
        {
            try
            {
                Request.Headers.TryGetValue("partnerId", out var partnerId);
                Request.Headers.TryGetValue("partnerKey", out var partnerKey);

                genericResp.IsSuccess = false;
                genericResp.Data = null;


                var checkPartner = await myDMgr.ValidateOEMPartner(partnerId, partnerKey);

                if (!checkPartner)
                {
                    genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                    genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                    return StatusCode(401, genericResp);
                }

                var response = helper.manageDevice(payload, partnerId, (int)deviceStatus.Locked);
                if (response.ResponseCode == "00")
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.WriteLog(ex);
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Transaction_error));
                genericResp.ResponseDescription = "transaction error";
                return StatusCode(401, genericResp);
            }
        }
    }
}
