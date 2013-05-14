using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedPayments.Lib.core
{
    public class ResourcePage<T>
    {
        protected const string api_secret_key = "4407245e885711e2a103026ba7cac9da"; // -u
        protected const string dateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'";
        protected Client client = new Client(null, api_secret_key);
    
        public String uri;
    
        private T cls;
        private List<T> items;
        private int total;
        private String first_uri;
        private String previous_uri;
        private String next_uri;
        private String last_uri;
    
        public ResourcePage(
                T cls,
                String uri,
                List<T> items,
                int total,
                string first_uri,
                string previous_uri,
                string next_uri,
                string last_uri)
        {
            this.cls = cls;
            this.uri = uri;
            this.items = items;
            this.total = total;        
            this.first_uri = first_uri;
            this.previous_uri = previous_uri;
            this.next_uri = next_uri;
            this.last_uri = last_uri;
        
        }
    
        public ResourcePage(
                T cls,
                String uri)
        {
            this.cls = cls;
            this.uri = uri;
        }
    
        public int getSize() {
            return getItems().Count();
        }
    
        public List<T> getItems() {
            if (items == null) 
                load();
            return items;
        }
    
        public String getFirstUri() {
            if (items == null) 
                load();
            return first_uri;
        }
    
        public ResourcePage<T> getFirst() {
            return new ResourcePage<T>(cls, getFirstUri());
        }
    
        public String getPreviousUri() {
            if (items == null) 
                load();
            return previous_uri;
        }
    
        public ResourcePage<T> getPrevious() {
            return new ResourcePage<T>(cls, getPreviousUri());
        }
    
        public String getNextUri() {
            if (items == null) 
                load();
            return next_uri;
        }
    
        public ResourcePage<T> getNext() {
            return new ResourcePage<T>(cls, getNextUri());
        }
    
        public String getLastUri() {
            return last_uri;
        }
    
        public ResourcePage<T> getLast() {
            return new ResourcePage<T>(cls, getLastUri());
        }

    
        public int getTotal() {
            if (items == null) 
                load();
            return total;
        }
    
        private void load() {
            Dictionary<string, object> page = this.client.get(uri, "");
        
            uri = (string) page["uri"];
            total= Convert.ToInt32(page["total"].ToString());
            first_uri = (string) page["first_uri"];
            previous_uri = (string) page["previous_uri"];
            next_uri = (string) page["next_uri"];
            last_uri = (string) page["last_uri"];

            List<Dictionary<string, object>> objs = (List<Dictionary<string, object>>) page["items"];
            items = new List<T>(objs.Count());
            foreach (var obj in objs)
            {
                T t = (T)Activator.CreateInstance(typeof(T), obj);
                var o = (object)t;
                ((Resource)o).deserialize(obj);
                items.Add(t);
            }
        }
    }
}
