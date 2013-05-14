using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using Newtonsoft.Json;

namespace BalancedPayments.Lib
{
    public class Marketplace : Resource
    {
        public String name { get; set; }
        public String support_email_address { get; set; }
        public String support_phone_number { get; set; }
        public String domain_url { get; set; }
        public int in_escrow { get; set; }
        public String bank_accounts_uri { get; set; }
        public List<BankAccount> bank_accounts { get; set; }
        public String cards_uri { get; set; }
        public List<Card> cards { get; set; }
        public String accounts_uri { get; set; }
        public List<Account> accounts { get; set; }
        public String debits_uri { get; set; }
        public List<Debit> debits { get; set; }
        public String credits_uri { get; set; }
        public List<Credit> credits { get; set; }
        public String holds_uri { get; set; }
        public List<Hold> holds { get; set; }
        public String refunds_uri { get; set; }
        public List<Refund> refunds { get; set; }
        public String events_uri { get; set; }
        public List<Event> events { get; set; }
        public String callbacks_uri { get; set; }
        public List<Callback> callbacks { get; set; }
        public Dictionary<object, string> meta { get; set; }

        public Marketplace(Settings settings, string marketplace_url)
            : base(settings, marketplace_url)
        {
        }

        public Account CreateAccount(string name, string email_address, Dictionary<string, string> meta) {
            
            var retVal = new Account(this.Settings);
            try
            {
                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload.Add("name", name);
                payload.Add("email_address", email_address);
                if (meta != null)
                {
                    payload.Add("meta", meta);
                }
                var response = this.client.post(accounts_uri, payload);
                retVal = new Account(this.Settings, response);
            }
            catch (Exception ex)
            {
                // could not create account
            }
            return retVal;
        }

        public Account GetAccount(string uri)
        {
            return new Account(this.Settings, uri);
        }

        public BankAccount GetBankAccount(string uri)
        {
            return new BankAccount(this.Settings, uri);
        }

        public Card GetCard(string uri)
        {
            return new Card(this.Settings, uri);
        }

        public BankAccount TokenizeBankAccount(string name, string account_number, string routing_number)
        {
            Dictionary<string, object> payload = new Dictionary<string,object>();
            payload.Add("name", name);
            payload.Add("account_number", account_number);
            payload.Add("routing_number", routing_number);
            var response = client.post(bank_accounts_uri, payload);
            return new BankAccount(this.Settings, response);
        }
    
        public BankAccount TokenizeBankAccount(string name, string account_number, string routing_number, string type)
        {
            Dictionary<string, object> payload = new Dictionary<string,object>();
            payload.Add("name", name);
            payload.Add("account_number", account_number);
            payload.Add("routing_number", routing_number);
            payload.Add("type", type);
            var response = client.post(bank_accounts_uri, payload);
            return new BankAccount(this.Settings, payload);
        }
    
        public Credit CreditBankAccount(int amount, string description, string account_number, string name, string routing_number, string type)
        {
            Dictionary<string, object> payload = new Dictionary<string,object>();
            payload.Add("amount", amount);
            if (description != null)
                payload.Add("description", description);
            Dictionary<string, object> bank_account = new Dictionary<string,object>();
            bank_account.Add("account_number", account_number);
            bank_account.Add("name", name);
            bank_account.Add("routing_number", routing_number);
            if (type != null)
                bank_account.Add("type", type);
            payload.Add("bank_account", bank_account);
            var repsonse = this.client.post(credits_uri, payload);
            return new Credit(this.Settings, payload);
        }
    
        public Card TokenizeCard(
            string name,
            string card_number,
            string security_code,
            int expiration_month,
            int expiration_year)
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("name", name);
            payload.Add("card_number", card_number);
            payload.Add("security_code", security_code);
            payload.Add("expiration_month", expiration_month);
            payload.Add("expiration_year", expiration_year);
            var response = this.client.post(cards_uri, payload);
            return new Card(this.Settings, response);
        }

        public override Dictionary<string, object> serialize() {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload["name"] = name;
            payload["support_email_address"] = support_email_address;
            payload["support_phone_number"] = support_phone_number;
            payload["domain_url"] = domain_url;
            payload["meta"] = meta;
            return payload;
        }

        public override void deserialize(Dictionary<String, Object> payload)
        {
            base.deserialize(payload);
            name = (String)payload["name"];
            support_email_address = (String)payload["support_email_address"];
            support_phone_number = (String)payload["support_phone_number"];
            domain_url = (String)payload["domain_url"];
            in_escrow = Convert.ToInt32(payload["in_escrow"].ToString());
            bank_accounts_uri = (String)payload["bank_accounts_uri"];
            // lazy load (i.e. do not load collections)
            //bank_accounts = Collections.BankAccounts(this.Settings, bank_accounts_uri);
            cards_uri = (String)payload["cards_uri"];
            //cards = Collections.Cards(this.Settings, cards_uri);
            accounts_uri = (String)payload["accounts_uri"];
            //accounts = Collections.Accounts(this.Settings);
            credits_uri = (String)payload["credits_uri"];
            //credits = Collections.Credits(this.Settings, credits_uri);
            debits_uri = (String)payload["debits_uri"];
            //debits = Collections.Debits(this.Settings, debits_uri);
            holds_uri = (String)payload["holds_uri"];
            //holds = Collections.Holds(this.Settings, holds_uri);
            refunds_uri = (String)payload["refunds_uri"];
            //refunds = Collections.Refunds(this.Settings, refunds_uri);
            meta = JsonConvert.DeserializeObject<Dictionary<object, string>>(payload["meta"].ToString());
            events_uri = (String)payload["events_uri"];
            //events = Collections.Events(events_uri);
            callbacks_uri = (String)payload["callbacks_uri"];
            //callbacks = Collections.Callbacks(callbacks_uri);
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
