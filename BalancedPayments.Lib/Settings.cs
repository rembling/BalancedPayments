using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedPayments.Lib
{
    public class Settings
    {
        public string VERSION { get; set; }
        public string location { get; set; }
        public string key { get; set; }
        public string marketplace_url { get; set; }
    

        public Settings(string version, string location, string key, string marketPlaceUrl) {
            this.VERSION = version;
            this.location = location;
            this.key = key;
            this.marketplace_url = marketPlaceUrl;
        }
    }
}
