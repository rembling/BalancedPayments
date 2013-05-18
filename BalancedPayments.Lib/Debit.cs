using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using Newtonsoft.Json;

namespace BalancedPayments.Lib
{
    public class Debit : Resource
    {
        public DateTime created_at { get; set; }
        public int amount { get; set; }
        public String description { get; set; }
        public String transaction_number { get; set; }
        public Card card { get; set; }
        public String card_uri { get; set; }
        public String account_uri { get; set; }
        public Account account { get; set; }
        public String hold_uri { get; set; }
        public Hold hold { get; set; }
        public String refunds_uri { get; set; }
        public List<Refund> refunds { get; set; }
        public Dictionary<string, string> meta { get; set; }

        public Debit() : base()
        {
        }

        public Debit(Settings settings)
            : base(settings)
        {
        }

        public Debit(Settings settings, Dictionary<string, object> payload)
            : base(settings, payload)
        {
        }

        public Debit(Settings settings, string uri) : base(settings, uri){
        }

        public Refund refund(
                int amount,
                string description,
                Dictionary<string, string> meta)
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("amount", amount);
            if (description != null)
                payload.Add("description", description);
            if (meta != null)
                payload.Add("meta", meta);
            if (uri != null)
                payload.Add("debit_uri", uri);
            var response = this.client.post(account_uri + "/refunds", payload);
            Refund refund = new Refund(this.Settings, response);
            return refund;
        }

        public Refund refund(int amount)
        {
            return refund(amount, null, null);
        }

        public Refund refund()
        {
            return refund(0, "", new Dictionary<string, string>());
        }

        public Account getAccount()
        {
            if (account == null)
                account = new Account(this.Settings, account_uri);
            return account;
        }

        public Card getCard()
        {
            if (card == null)
                card = new Card(this.Settings, card_uri);
            return card;
        }

        public Hold getHold()
        {
            if (hold == null)
                hold = new Hold(this.Settings, hold_uri);
            return hold;
        }

        public override Dictionary<string, object> serialize()
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("meta", meta);
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload)
        {
            base.deserialize(payload);
            created_at = Convert.ToDateTime(payload["created_at"].ToString());
            meta = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload["meta"].ToString());
            amount = Convert.ToInt32(payload["amount"].ToString());
            description = (string)payload["description"];
            transaction_number = (string)payload["transaction_number"];
            if (payload.ContainsKey("account_uri"))
            {
                account = null;
                account_uri = (string)payload["account_uri"];
            }
            else
            {
                var acc = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["account"].ToString());
                account = new Account(this.Settings, acc);
                account_uri = account.uri;
            }
            if (payload.ContainsKey("source_uri"))
            {
                card = null;
                card_uri = (string)payload["source_uri"];
            }
            else
            {
                var c = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["source"].ToString());
                card = new Card(this.Settings, c);
                card_uri = card.uri;
            }
            if (payload.ContainsKey("hold_uri"))
            {
                hold = null;
                hold_uri = (String)payload["hold_uri"];
            }
            else if (payload.ContainsKey("hold"))
            {
                var h = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["hold"].ToString());
                hold = new Hold(this.Settings, h);
                hold_uri = hold.uri;
            }
            else
            {
                hold = null;
                hold_uri = null;
            }
            refunds_uri = (string)payload["refunds_uri"];
            refunds = Collections.Refunds(this.Settings, refunds_uri);

        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
