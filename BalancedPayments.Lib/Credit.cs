using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using Newtonsoft.Json;

namespace BalancedPayments.Lib
{
    public class Credit : Resource
    {
        public DateTime created_at { get; set; }
        public int amount { get; set; }
        public String description { get; set; }
        public String status { get; set; }
        public BankAccount bank_account { get; set; }
        public String account_uri { get; set; }
        public Account account { get; set; }
        public Dictionary<string, string> meta { get; set; }

        public Credit() : base()
        {
        }

        public Credit(Settings settings) : base(settings)
        {
        }

        public Credit(Settings settings, Dictionary<string, object> payload)
            : base(settings, payload)
        {
        }


        public override Dictionary<string, object> serialize() {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("meta", meta);
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload) {
            base.deserialize(payload);
            if (payload.ContainsKey("account_uri")) {
                account = null;
                account_uri = (String) payload["account_uri"];
            }
            else if (payload.ContainsKey("account") && payload["account"] != null) {
                var acct = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["account"].ToString());
                account = new Account(this.Settings, acct);
                account_uri = account.uri;
            }
            else {
                account = null;
                account_uri = null;
            }
            var ba = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["bank_account"].ToString());
            bank_account = new BankAccount(this.Settings, ba);
            created_at = Convert.ToDateTime(payload["created_at"].ToString());
            amount = Convert.ToInt32(payload["amount"].ToString());
            description = (string) payload["description"];
            status = (string) payload["status"];
            meta = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload["meta"].ToString());
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
