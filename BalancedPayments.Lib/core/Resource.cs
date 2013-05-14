using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace BalancedPayments.Lib.core
{
    public abstract class Resource
    {
        protected const string dateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'";
        protected Client client { get; set; }
        public Settings Settings { get; set; }

        public String uri;
        public String id;

        public Resource()
        {
        }

        public Resource(Settings settings)
        {
            this.Settings = settings;
            this.client = new Client(Settings.location, Settings.key);
        }

        public Resource(Settings settings, Dictionary<String, Object> payload) {
            this.Settings = settings;
            this.client = new Client(Settings.location, Settings.key);
            this.deserialize(payload);
        }
    
        public Resource(Settings settings, string uri) {
            this.Settings = settings;
            this.client = new Client(Settings.location, Settings.key);
            Dictionary<string, Object> payload = client.get(uri, "");
            this.deserialize(payload);
        }

        public abstract Dictionary<string, object> serialize();

        public abstract void AttachSettings(Settings settings);

        public virtual void save()
        {
            Dictionary<string, object> request = serialize();
            Dictionary<string, object> response = null;
            if (id == null)
            {
                if (uri == null)
                {
                    throw new Exception(this.ToString());
                }
                response = client.post(uri, request);
            }
            else
                response = client.put(uri, request);
            deserialize(response);
        }

        public virtual void delete()
        {
            if (id != null)
            {
                try
                {
                    client.delete(uri);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Could not DELETE, invalidate instead: {0}", ex.Message));
                    this.invalidate();
                    //client.invalidate(uri);
                }
                
            }
        }

        public virtual void invalidate()
        {
            this.client.invalidate(uri);
        }

        public virtual void deserialize(Dictionary<string, object> payload)
        {
            uri = (string)payload["uri"];
            id = (string)payload["id"];
        }
        protected DateTime deserializeDate(string raw)
        {
            return Convert.ToDateTime(raw);
        }
    }
}
