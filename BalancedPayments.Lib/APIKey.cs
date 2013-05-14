using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;

namespace BalancedPayments.Lib
{
    public class APIKey : Resource
    {
        private const String root = "api_keys";

        public string secret { get; set; }
        public DateTime created_at { get; set; }

        public APIKey(Settings settings) : base(settings)
        {
        }

        public override void save() {
            if (id == null && uri == null)
                uri = String.Format("/v{0}/{1}", Settings.VERSION, root);
            base.save();
        }
    
        public override Dictionary<string, object> serialize() {
            Dictionary<string, object> payload = new Dictionary<string,object>();
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload) {
            base.deserialize(payload);
            secret = payload.ContainsKey("secret") ? (string) payload["secret"] : null;
            created_at = Convert.ToDateTime(payload["created_at"].ToString());
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
