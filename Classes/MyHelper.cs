using DeviceFinanceApp.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Classes
{
    public class MyHelper
    {
        private readonly IOptions<AppSettingParams> _appsetting;
        private readonly string apiKey;
        private readonly string apiSecret;
        public MyHelper(IOptions<AppSettingParams> appsetting)
        {
            _appsetting = appsetting;
            //apiKey = _appsetting.Value.INT_API_KEY;
            //apiSecret = _appsetting.Value.INT_API_SECRET;
        }
        public static string DoGet(string url)
        {
            string resp;
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "text/plain";
                   
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    resp = client.DownloadString(url);
                }
            }
            catch (WebException wex)
            {
               // WebLog.Log(wex);
                using (var response = (HttpWebResponse)wex.Response)
                {
                    var statusCode = response != null ? (int)response.StatusCode : 500;
                    if (statusCode == 500 && response == null) return null;
                    var dataStream = response?.GetResponseStream();
                    if (dataStream == null) return null;
                    using (var tReader = new StreamReader(dataStream))
                    {
                        resp = tReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
              //  WebLog.Log(ex);
                resp = ex.Message;
            }
            return resp;
        }
        public  string DoPost(string json, string postUrl)
        {

            string resp;
            try
            {
                string apiKey = _appsetting.Value.INT_API_KEY;
                string apiSecret = _appsetting.Value.INT_API_SECRET;
               

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    if (!string.IsNullOrWhiteSpace(apiKey))
                    {
                        client.Headers.Add("api_key", apiKey);
                        client.Headers.Add("api_secret", apiSecret);
                    }
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    resp = client.UploadString(postUrl, "POST", json);
                }
            }
            catch (WebException wex)
            {

                using (var response = (HttpWebResponse)wex.Response)
                {
                    var statusCode = response != null ? (int)response.StatusCode : 500;
                    if (statusCode == 500 && response == null) return null;
                    var dataStream = response?.GetResponseStream();
                    if (dataStream == null) return null;
                    using (var tReader = new StreamReader(dataStream))
                    {
                        resp = tReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }
            return resp;
        }
        public  int SendSMS(string strmessage, string PhoneNumber)
        {
            int Resp = 0;
            string myRessult = string.Empty;
            try
            {
                //"PayOrBoro";
                string smsusername = _appsetting.Value.SMS_USERNAME;
                string smsPwd = _appsetting.Value.SMS_PASSWORD; 
                string sender = _appsetting.Value.SMS_SENDER;
                // string myBaseurl = "http://sms.hollatags.com/api/credit?user=bodesanya&pass=pa33c0de";
                string myBaseurl = "http://sms.hollatags.com/api/send?" + "user=" + smsusername + " &pass=" + smsPwd + "&to=" + PhoneNumber + "&from=" + sender + "&msg=" + strmessage;
                var resp = DoGet(myBaseurl);
                if (resp == "sent")
                    Resp = 1;



                // lblMsg.Text = WebResponse;
            }

            catch (Exception ex)
            {
               // WebLog.Log(ex.Message + "##Helper:SendSMS##" + ex.StackTrace);
                Resp = 0;
            }
            return Resp;
        }
    }
}
