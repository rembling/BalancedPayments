using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;

namespace BalancedPayments.Lib
{
    public class Event : Resource
    {
        public DateTime occurred_at { get; set; }
        public String type { get; set; }
        public Dictionary<string, object> entity;

        public Event(Settings settings) : base(settings) {
        }
    
        public Event(Settings settings, string uri) : base(settings, uri)
        {
        }
    
        public Event(Settings settings, Dictionary<string, object> payload) : base(settings, payload)
        {
        }

        public override void deserialize(Dictionary<string, object> payload)
        {
            base.deserialize(payload);
            occurred_at = Convert.ToDateTime((String)payload["occurred_at"]);
            type = (String)payload["type"];
            entity = (Dictionary<string, object>)payload["entity"];
        }

        public override Dictionary<string, object> serialize()
        {
            throw new Exception("Cannot be created or updated");
        }

        public override void AttachSettings(Settings settings)
        {
            this.Settings = settings;
        }
    }
}
