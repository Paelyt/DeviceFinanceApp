using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class AppSettingParams
    {
        public string INT_PREQUALIFY_INFO_URL { get; set; }
        public string INT_API_KEY { get; set; }
        public string INT_API_SECRET { get; set; }
        public string SMS_SENDER { get; set; }
        public string SMS_USERNAME { get; set; }
        public string SMS_PASSWORD { get; set; }
    }
}
