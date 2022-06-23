using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Classes
{
    public class myhttpManager
    {
        public static string DoGet(string url)
        {

            string resp;
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    resp = client.DownloadString($"{url}");
                   

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
        public static string DoPost(string json, string url)
        {

            string resp;
            try
            {

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    //if (!string.IsNullOrWhiteSpace(plainText))
                    //{
                    //    //client.Headers.Add("principal", principal);
                    //    //client.Headers.Add("credentials", credentials);
                    //    // client.Headers.Add("hash", hash);
                    //}
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    resp = client.UploadString(url, "POST", json);
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
    }
}
