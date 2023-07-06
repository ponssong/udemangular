using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public string Details { get; }
        
        public ApiException(int statusCode, string messsage = null, string details = null) : base(statusCode, messsage)
        {
            this.Details = details;            
        }
        
    }
}