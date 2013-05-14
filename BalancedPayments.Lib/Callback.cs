using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;

namespace BalancedPayments.Lib
{
    public class Callback : Resource
    {
        public string url { get; set; }

        public Callback(Settings settings) : base(settings)
        {
        }

        public Callback(Settings settings, Dictionary<string, object> payload) : base(settings, payload)
        {
        }

        public Callback(Settings settings, string uri) : base(settings, uri) 
        {
        }

        public override Dictionary<string, object> serialize()
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload["url"] = url;
            return payload;
        }

        public override void deserialize(Dictionary<string, object> payload)
        {
            base.deserialize(payload);
            url = (String)payload["url"];
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
