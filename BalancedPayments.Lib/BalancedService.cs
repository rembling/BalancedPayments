using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedPayments.Lib
{
    public class BalancedService
    {
        public Settings Settings { get; set; }
        public Marketplace Marketplace { get; set; }

        public BalancedService(string version, string location, string key, string marketPlaceUrl)
        {
            this.Settings = new Settings(version, location, key, marketPlaceUrl);
            this.Marketplace = new Marketplace(this.Settings, this.Settings.marketplace_url);
        }
    }
}
