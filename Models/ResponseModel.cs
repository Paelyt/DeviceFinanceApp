using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Models
{
    public class ResponseModel
    {
        public bool IsSuccess{ get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public object Data { get; set; }
    }

    public class TransferResponse
    {
        public string Status { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public string Reference_number { get; set; }
        public DateTime Trans_date { get; set; }
    }
}
