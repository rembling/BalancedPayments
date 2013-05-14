using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using Newtonsoft.Json;

namespace BalancedPayments.Lib
{
    public class Card : Resource
    {
        public DateTime created_at { get; set; }
        public string street_address { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string name { get; set; }
        public int expiration_month { get; set; }
        public int expiration_year { get; set; }
        public string card_number { get; set; }
        public string last_four { get; set; }
        public string brand { get; set; }
        public bool is_valid { get; set; }
        public string fingerprint { get; set; }
        public Dictionary<string, string> meta { get; set; }

        public Card() : base()
        {
        }

        public Card(Settings settings)
            : base(settings)
        {
        }

        public Card(Settings settings, Dictionary<string, object> payload)
            : base(settings, payload)
        {
        }

        public Card(Settings settings, string uri) : base(settings, uri)
        {
        }

        public override Dictionary<string, object> serialize()
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("meta", meta);
            payload.Add("card_number", card_number);
            payload.Add("street_address", street_address);
            payload.Add("postal_code", postal_code);
            payload.Add("country_code", country_code);
            payload.Add("name", name);
            payload.Add("expiration_month", expiration_month);
            payload.Add("expiration_year", expiration_year);
            payload.Add("is_valid", is_valid);
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload)
        {
            base.deserialize(payload);
            created_at = Convert.ToDateTime(payload["created_at"]);
            meta = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload["meta"].ToString());
            street_address = payload.ContainsKey("street_address") ? (string)payload["street_address"] : "";
            postal_code = payload.ContainsKey("postal_code") ? (string)payload["postal_code"] : "";
            country_code = payload.ContainsKey("country_code") ? (string)payload["country_code"] : "";
            name = payload.ContainsKey("name") ? (string)payload["name"] : "";
            expiration_month = Convert.ToInt32(payload["expiration_month"].ToString());
            expiration_year = Convert.ToInt32(payload["expiration_year"].ToString());
            card_number = (string)payload["last_four"];
            last_four = (string)payload["last_four"];
            brand = (string)payload["brand"];
            is_valid = (bool)payload["is_valid"];
            fingerprint = (string)payload["hash"];
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
