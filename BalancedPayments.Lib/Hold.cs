using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using Newtonsoft.Json;

namespace BalancedPayments.Lib
{
    public class Hold : Resource
    {
        public DateTime created_at { get; set; }
        public int amount { get; set; }
        public DateTime expires_at { get; set; }
        public String description { get; set; }
        public Debit debit { get; set; }
        public String transaction_number { get; set; }
        public Boolean is_void { get; set; }
        public String account_uri { get; set; }
        public Account account { get; set; }
        public String card_uri { get; set; }
        public Card card { get; set; }
        public Dictionary<string, object> meta { get; set; }

        public static Hold get(Settings settings, String uri)
        {
            return new Hold(settings, (new Client(settings)).get(uri, ""));
        }

        public Hold() : base()
        {
        }

        public Hold(Settings settings)
            : base(settings)
        {
        }

        public Hold(Settings settings, String uri)
            : base(settings, uri)
        {
        }

        public Hold(Settings settings, Dictionary<string, object> payload)
            : base(settings, payload)
        {
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

        public void void_()
        {
            is_void = true;
            save();
        }

        public Debit capture(int amount)
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("hold_uri", uri);
            payload.Add("amount", amount);
            debit = new Debit(this.Settings, payload);
            return debit;
        }

        public Debit capture()
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("hold_uri", uri);
            debit = new Debit(this.Settings, payload);
            return debit;
        }

        public override Dictionary<string, object> serialize()
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("amount", amount);
            payload.Add("description", description);
            payload.Add("is_void", is_void);
            payload.Add("meta", meta);
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload)
        {
            base.deserialize(payload);
            created_at = Convert.ToDateTime(payload["created_at"].ToString());
            meta = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["meta"].ToString());
            amount = Convert.ToInt32(payload["amount"].ToString());
            expires_at = Convert.ToDateTime(payload["expires_at"].ToString());
            description = (string)payload["description"];
            is_void = Convert.ToBoolean(payload["is_void"].ToString());
            if (payload.ContainsKey("account_uri"))
            {
                account = null;
                account_uri = (String)payload["account_uri"];
            }
            else
            {
                var a = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["account"].ToString());
                account = new Account(this.Settings, a);
                account_uri = account.uri;
            }
            if (payload.ContainsKey("debit") && payload["debit"] != null)
            {
                var d = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload["debit"].ToString());
                debit = new Debit(this.Settings, d);
            }
            else
                debit = null;
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
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
