using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using Newtonsoft.Json;

namespace BalancedPayments.Lib
{
    public class Account : Resource
    {    
        public const string BUYER_ROLE = "buyer";
        public const string MERCHANT_ROLE = "merchant";
    
        public DateTime created_at { get; set; }
        public string name { get; set; }
        public string email_address { get; set; }
        public List<string> roles { get; set; }
        public string bank_accounts_uri { get; set; }
        public List<BankAccount> bank_accounts { get; set; }
        public string cards_uri { get; set; }
        public List<Card> cards { get; set;}
        public string credits_uri { get; set; }
        public List<Credit> credits { get; set; }
        public string debits_uri { get; set; }
        public List<Debit> debits { get; set; }
        public string holds_uri { get; set; }
        public List<Hold> holds { get; set; }
        public Dictionary<string, string> meta { get; set; }

        public Account() : base()
        {
        }

        public Account(Settings settings) : base(settings)
        {
        }
    
        public Account(Settings settings, Dictionary<string, object> payload) : base(settings, payload) 
        {
        }
    
        public Account(Settings settings, string uri) : base(settings, uri)
        {
        }

        public Credit Credit(
                int amount,
                string description,
                string destination_uri,
                string appears_on_statement_as,
                Dictionary<string, string> meta) {
            Dictionary<string, object> payload = new Dictionary<string,object>();
            payload.Add("amount", amount);
            if (description != null)
                payload.Add("description", description);
            if (destination_uri != null)
                payload.Add("destination", destination_uri);
            if (appears_on_statement_as != null)
                payload.Add("appears_on_statement_as", appears_on_statement_as);
            if (meta != null)
                payload.Add("meta", meta);
            var response = this.client.post(credits_uri, payload);
            Credit credit = new Credit(this.Settings, response);
            
            return credit;
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="HTTPError">An http error may be thrown</exception>
        /// <returns></returns>
        public Credit Credit(int amount) {
            return Credit(amount, null, null, null, null);
        }
    
        public Debit Debit(
                int amount,
                string description,
                string source_uri,
                string appears_on_statement_as,
                Dictionary<string, string> meta) {
            Dictionary<string, object> payload = new Dictionary<string,object>();
            payload.Add("amount", amount);
            if (description != null)
                payload.Add("description", description);
            if (source_uri != null)
                payload.Add("source_uri", source_uri);
            if (appears_on_statement_as != null)
                payload.Add("appears_on_statement_as", appears_on_statement_as);
            if (meta != null)
                payload.Add("meta", meta);
            var response = this.client.post(debits_uri, payload);
            Debit debit = new Debit(this.Settings, response);
            
            return debit;
        }
    
        public Debit Debit(int amount) {
            return Debit(amount, null, null, null, null);
        }
    
        public Hold Hold(
                int amount,
                string description,
                string source_uri,
                Dictionary<string, string> meta) {
            Dictionary<string, object> payload = new Dictionary<string,object>();
            payload.Add("amount", amount);
            if (description != null)
                payload.Add("description", description);
            if (source_uri != null)
                payload.Add("source", source_uri);
            if (meta != null)
                payload.Add("meta", meta);
            var response = this.client.post(holds_uri, payload);
            Hold hold = new Hold(this.Settings, response);
            return hold;
        }
    
        public Hold Hold(int amount) {
            return Hold(amount, null, null, null);
        }
    
        public void AssociateBankAccount(String bank_account_uri) {
            Dictionary<string, object> payload = new Dictionary<string,object>();
            payload.Add("bank_account_uri", bank_account_uri);
            Dictionary<string, object> response = client.put(uri, payload);
            deserialize(response);
        }
    
        public void AssociateCard(String card_uri) {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("account_uri", uri);
            Dictionary<string, object> response = client.put(card_uri, payload);
            var account = JsonConvert.DeserializeObject<Account>(response["account"].ToString());
        }

        public Refund Refund(
                int amount,
                string description,
                Dictionary<string, string> meta,
                string debit_uri)
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("amount", amount);
            if (description != null)
                payload.Add("description", description);
            if (meta != null)
                payload.Add("meta", meta);
            if (debit_uri != null)
                payload.Add("debit_uri", debit_uri);
            var response = this.client.post(uri + "/refunds", payload);
            Refund refund = new Refund(this.Settings, response);

            return refund;
        }

        public override Dictionary<string, object> serialize()
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("name", name);
            payload.Add("email_address", email_address);
            // roles are set dynamically by BP
            //payload.Add("roles", roles);
            payload.Add("meta", meta);
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload)
        {
            if (payload.ContainsKey("status"))
            {
                var status = payload["status"].ToString();

                switch(status)
                {
                    case "Not Found":
                        break;
                    case "Forbidden":
                        break;
                    default:
                        break;
                }
                return;
            }
            else
            {
                base.deserialize(payload);
                created_at = Convert.ToDateTime(payload["created_at"]);
                meta = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload["meta"].ToString());
                name = (string)payload["name"];
                email_address = (string)payload["email_address"];
                roles = JsonConvert.DeserializeObject<List<string>>(payload["roles"].ToString());
                bank_accounts_uri = (string)payload["bank_accounts_uri"];
                bank_accounts = Collections.BankAccounts(this.Settings, bank_accounts_uri);
                cards_uri = (string)payload["cards_uri"];
                cards = Collections.Cards(this.Settings, cards_uri);
                credits_uri = (string)payload["credits_uri"];
                credits = Collections.Credits(this.Settings, credits_uri);
                debits_uri = (string)payload["debits_uri"];
                debits = Collections.Debits(this.Settings, debits_uri);
                holds_uri = (string)payload["holds_uri"];
                holds = Collections.Holds(this.Settings, holds_uri);
            }
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
