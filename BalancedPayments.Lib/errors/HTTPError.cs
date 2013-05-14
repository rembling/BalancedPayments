using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BalancedPayments.Lib.errors
{
    public class HTTPError : Exception
    {
        public int status_code;
        public string status;
        public string raw;

        public HTTPError(HttpResponse response, String raw)
        {
            var status_line = response.StatusDescription;
            this.status_code = response.StatusCode;
            this.status = response.Status;
            this.raw = raw;
        }
    }
}
