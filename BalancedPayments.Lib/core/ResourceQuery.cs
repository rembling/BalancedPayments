using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedPayments.Lib.core
{
    public class ResourceQuery<T> : ResourcePagination<T>
    {
        protected const string dateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'";

        public ResourceQuery(T cls, String uri) : base(cls, uri)
        {
        }

        public T first() {
            int limit = getLimit();
            setLimit(1);
            List<T> items = all();
            setLimit(limit);
            if (items.Count() == 0)
                throw new Exception("No results found");
            return items[0];
        }
    
        public T one() {
            int limit = getLimit();
            setLimit(2);
            List<T> items = all();
            setLimit(limit);
            if (items.Count() == 0)
                throw new Exception("No results found");
            if (items.Count() > 1)
                throw new Exception("Multiple results found");
            return items[0];
        }
    
        // filtering
    
        public ResourceQuery<T> filter(String field, String op, String value) {
            String name = String.Format("%s[%s]", field, op);
            //uri_builder.addParameter(name, value);
            return this;
        }
    
        public ResourceQuery<T> filter(String field, String op, String[] values) {
            String name = String.Format("%s[%s]", field, op);
            String value = String.Join(",", values);
            //uri_builder.addParameter(name, value);
            return this;
        }
    
        public ResourceQuery<T> filter(String field, String value) {
            //uri_builder.addParameter(field, value);
            return this;
        }
    
        public ResourceQuery<T> filter(String field, String[] values) {
            String value = string.Join(",", values);
            //uri_builder.addParameter(field, value);
            return this;
        }
    
        public ResourceQuery<T> filter(String field, String op, DateTime value) {
            return this.filter(field, op, value.ToString(dateTimeFormat));
        }

        public ResourceQuery<T> filter(String field, String op, DateTime[] values)
        {
            String[] transformed = new String[values.Length];
            for (int i = 0; i != values.Length; i++)
                transformed[i] = values[i].ToString(dateTimeFormat);
            return this.filter(field, op, transformed);
        }

        public ResourceQuery<T> filter(String field, DateTime value)
        {
            return this.filter(field, value.ToString(dateTimeFormat));
        }

        public ResourceQuery<T> filter(String field, DateTime[] values)
        {
            String[] transformed = new String[values.Length];
            for (int i = 0; i != values.Length; i++)
                transformed[i] = values[i].ToString(dateTimeFormat);
            return this.filter(field, transformed);
        }
    
        public ResourceQuery<T> filter(string field, string op, object value) {
            return this.filter(field, op, value.ToString());
        }

        public ResourceQuery<T> filter(string field, string op, object[] values) {
            String[] transformed = new String[values.Length];
            for (int i = 0; i != values.Length; i++)
                transformed[i] = values[i].ToString();
            return this.filter(field, op, transformed);
        }
    
        public ResourceQuery<T> filter(string field, object value) {
            return this.filter(field, value.ToString());
        }
    
        public ResourceQuery<T> filter(string field, object[] values) {
            String[] transformed = new string[values.Length];
            for (int i = 0; i != values.Length; i++)
                transformed[i] = values[i].ToString();
            return this.filter(field, transformed);
        }
    
        // sorting
    
        public ResourceQuery<T> order_by(String field) {
            //uri_builder.addParameter("sort", field);
            return this;
        }
    
        public ResourceQuery<T> order_by(String field, bool ascending) {
            String value = String.Format("%s,%s", field, ascending ? "asc" : "desc");
            //uri_builder.addParameter("sort", value);
            return this;
        }
    }
}
