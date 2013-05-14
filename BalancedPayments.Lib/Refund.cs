using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using Newtonsoft.Json;

namespace BalancedPayments.Lib
{
    public class Refund : Resource
    {
        public DateTime created_at { get; set; }
        public int amount { get; set; }
        public String description { get; set; }
        public Account account { get; set; }
        public String appears_on_statement_as { get; set; }
        public String transaction_number { get; set; }
        public Debit debit { get; set; }
        public Dictionary<string, string> meta { get; set; }

        public Refund() : base()
        {
        }

        public Refund(Settings settings) : base(settings) {
        }

        public Refund(Settings settings, Dictionary<string, object> payload)
            : base(settings, payload)
        {
        }
    
        public override Dictionary<string, object> serialize() {
            Dictionary<string, object> payload = new Dictionary<string,object>();
            payload.Add("amount", amount);
            payload.Add("description", description);
            payload.Add("appears_on_statement_as", appears_on_statement_as);
            payload.Add("meta", meta);
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload) {
            base.deserialize(payload);
            created_at = Convert.ToDateTime(payload["created_at"].ToString());
            meta = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload["meta"].ToString());
            amount = Convert.ToInt32(payload["amount"].ToString());
            description = (string) payload["description"];
            appears_on_statement_as = (string) payload["appears_on_statement_as"];
            var acct = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["account"].ToString());
            account = new Account(this.Settings, acct);
            var dbt = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["debit"].ToString());
            debit = new Debit(this.Settings, dbt);
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
