using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using Newtonsoft.Json;

namespace BalancedPayments.Lib
{
    public class Merchant : Resource
    {
        public const String PERSON_TYPE = "person";
        public const String BUSINESS_TYPE = "business";
    
        public DateTime created_at { get; set; }
        public String type { get; set; }
        public String name { get; set; }
        public String email_address { get; set; }
        public String phone_number { get; set; }
        public String accounts_uri { get; set; }
        public List<Account> accounts { get; set; }
        public String api_keys_uri { get; set; }
        public List<APIKey> api_keys { get; set; }
        public String street_address { get; set; }
        public String city { get; set; }
        public String postal_code { get; set; }
        public String country_code { get; set; }
        public Dictionary<string, string> meta;

        public Merchant(Settings settings) : base(settings)
        {
        }

        public override Dictionary<string, object> serialize() {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("type", type);
            payload.Add("name", name);
            payload.Add("email_address", email_address);
            payload.Add("phone_number", phone_number);
            payload.Add("accounts_uri", accounts_uri);
            payload.Add("street_address", street_address);
            payload.Add("city", city);
            payload.Add("postal_code", postal_code);
            payload.Add("country_code", country_code);
            if (meta != null)
                payload.Add("meta", meta);
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload) {
            base.deserialize(payload);
            created_at = Convert.ToDateTime((String) payload["created_at"]);
            meta = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload["meta"].ToString());
            type = (String) payload["type"];
            name = (String) payload["name"];
            email_address = (String) payload["email_address"];
            phone_number = (String) payload["phone_number"];
            street_address = (String) payload["street_address"];
            city = (String) payload["city"];
            postal_code = (String) payload["postal_code"];
            country_code = (String) payload["country_code"];        
            accounts_uri = (String) payload["accounts_uri"];
            accounts = Collections.Accounts(this.Settings);
            api_keys_uri = (String) payload["api_keys_uri"];
            //api_keys = Collections.APIs();
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
