using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatingApi.Errors
{
    public class ApiException
    {
        public ApiException(int statusCode, string massage=null, string details=null)
        {
            StatusCode = statusCode;
            Massage = massage;
            Details = details;
        }
        public int StatusCode { get; set; }
        public string Massage { get; set; }
        public string Details { get; set; }
    }
}
