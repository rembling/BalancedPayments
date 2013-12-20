using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;

namespace BalancedPayments.Lib
{
    public class BankAccountVerification : Resource
    {
        public int attempts { get; set; }
        public int remaining_attempts { get; set; }
        public String state { get; set; }

        public BankAccountVerification() : base()
        {
        }

        public BankAccountVerification(Settings settings) : base(settings) {
        }
    
        public BankAccountVerification(Settings settings, string uri) : base(settings, uri) {
        }
    
        public BankAccountVerification(Settings settings, Dictionary<string, object> payload) : base(settings, payload) {
        }

        public override Dictionary<string, object> serialize() {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("attempts", attempts);
            payload.Add("remaining_attempts", remaining_attempts);
            payload.Add("state", state);
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload)
        {
            base.deserialize(payload);
            attempts = Convert.ToInt32(payload["attempts"].ToString());
            remaining_attempts = Convert.ToInt32(payload["remaining_attempts"].ToString());
            state = (string)payload["state"];
        }

        public BankAccountVerification create(string uri)
        {
            var response = client.post(uri, null);
            BankAccountVerification bankAccountVerification = new BankAccountVerification(this.Settings, response);
            return bankAccountVerification;
        }

        public BankAccountVerification confirm(int amount_1, int amount_2)
        {

            Dictionary<string, object> request = new Dictionary<string, object>();
            request.Add("amount_1", amount_1);
            request.Add("amount_2", amount_2);
            Dictionary<string, object> response = client.put(uri, request);

            BankAccountVerification bankAccountVerification = new BankAccountVerification(this.Settings, response);
            return bankAccountVerification;
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
