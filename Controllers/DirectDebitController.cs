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
    public class DirectDebitController : ControllerBase
    {
        private MyDataManager myDMgr;
        private ResponseModel genericResp;
        private Helper helper;
        private readonly ErrorLogger logger;
        private readonly DeviceFinanceContext _dbcontext;
        private readonly IOptions<AppSettingParams> _appsetting;

        public DirectDebitController(IOptions<AppSettingParams> appsetting, DeviceFinanceContext dbcontext)
        {
            logger = new ErrorLogger(_Destination: @"ErrorLog");
            genericResp = new ResponseModel();
            myDMgr = new MyDataManager(dbcontext);
            helper = new Helper(appsetting, dbcontext);
            _appsetting = appsetting;
        }

        [HttpPost]
        [Route("SmsAlert")] //PARTNERS
        public async Task<IActionResult> SmsAlert(sms payload)
        {
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var checkPartner = await myDMgr.ValidateUtilityPartner(_bearer_token);

            if (!checkPartner)
            {
                genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                return StatusCode(401, genericResp);
            }
                      
            var response = helper.SendSMS(payload);
            if (response.ResponseCode == "00")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        

        [HttpGet]
        [Route("LoanDueAmount")] //PARTNERS
        public async Task<IActionResult> LoanDueAmount(string loanReference)
        {
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var checkPartner = await myDMgr.ValidateUtilityPartner(_bearer_token);

            if (!checkPartner)
            {
                genericResp.ResponseDescription = "You are not Authorized. Wrong PartnerId or ParterKey";
                genericResp.ResponseCode = string.Format("{0:00}", ((int)responsecode.Invalid_Partner));
                return StatusCode(401, genericResp);
            }

            var response = helper.getLoanDueAmount(loanReference);
            if (response.ResponseCode == "00")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


    }
}
