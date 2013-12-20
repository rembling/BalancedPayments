using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using Newtonsoft.Json;

namespace BalancedPayments.Lib
{
    public class BankAccount : Resource
    {
        public DateTime created_at { get; set; }
        public String name { get; set; }
        public String account_number { get; set; }
        public String routing_number { get; set; }
        public String type { get; set; }
        public String fingerprint { get; set; }
        public String bank_name { get; set; }
        public String verifications_uri { get; set; }
        public List<BankAccountVerification> verifications { get; set; }
        public String verification_uri { get; set; }
        public Dictionary<string, string> meta { get; set; }

        public BankAccount() : base()
        {
        }

        public BankAccount(Settings settings) : base(settings)
        {
        }

        public BankAccount(Settings settings, string uri) : base(settings, uri)
        {
        }
    
        public BankAccount(Settings settings, Dictionary<string, object> payload) : base(settings, payload)
        {
        }

        public override Dictionary<string, object> serialize() {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("name", name);
            payload.Add("account_number", account_number);
            payload.Add("routing_number", routing_number);
            payload.Add("type", type);
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload)
        {
            base.deserialize(payload);
            created_at = Convert.ToDateTime(payload["created_at"].ToString());
            name = (string) payload["name"];
            account_number = (string) payload["account_number"];
            routing_number = (string) payload["routing_number"];
            type = (string) payload["type"];
            fingerprint = (string) payload["fingerprint"];
            bank_name = (string) payload["bank_name"];
            meta = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload["meta"].ToString());
            verifications_uri = (string) payload["verifications_uri"];
            verifications = Collections.BankAccountVerifications(this.Settings, verifications_uri);
            verification_uri = (string) payload["verification_uri"];
        }

        public override void save() {
            if (id == null && uri == null) {
                uri = "/v" + Settings.VERSION + "/bank_accounts";
            }
            base.save();
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
