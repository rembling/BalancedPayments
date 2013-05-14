using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedPayments.Lib.core
{
    public class ResourcePagination<T> : IEnumerable<T>
    {
        public class ResourceIterator : IEnumerable<T>
        {
            public String uri;
            public ResourcePage<T> page;
            public int index;

            public ResourceIterator(String uri, ResourcePage<T> page)
            {
                this.uri = uri;
                this.page = page;
                this.index = 0;
            }

            public bool hasNext()
            {
                return (index < page.getSize() || this.page.getNextUri() != null);
            }

            public T next()
            {
                if (index >= page.getSize())
                {
                    page = page.getNext();
                    index = 0;
                }
                T t = page.getItems()[index];
                index += 1;
                return t;
            }

            public void remove()
            {
                //throw new UnsupportedOperationException();
            }

            public IEnumerator<T> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        protected Client client = new Client(null, null);
        protected T cls;
        protected UriBuilder uri_builder;

        public ResourcePagination(T cls, String uri)
        {
            this.cls = cls;
            this.uri_builder = new UriBuilder(uri);
        }

        public T create()
        {
            Dictionary<string, object> payload = new Dictionary<string, object>();
            return create(payload);
        }

        public T create(Dictionary<string, object> payload)
        {
            T t;
            t = (T)Activator.CreateInstance(typeof(T), cls);
            var o = (object)t;
            ((Resource)o).deserialize(client.post(getURI(), payload));
            return t;
        }

        public int total()
        {
            int limit = getLimit();
            setLimit(1);
            String uri = getURI();
            ResourcePage<T> page = new ResourcePage<T>(cls, uri);
            setLimit(limit);
            return page.getTotal();
        }

        protected int getLimit()
        {
            return 0;
            //foreach (var kv in uri_builder.)
            //{
            //    if (kv.getName().equals("limit"))
            //    {
            //        return new Convert.ToInt32(kv.getValue());
            //    }
            //}
            //return null;
        }

        protected void setLimit(int limit)
        {
            //if (limit == null)
            //{
            //    List<NameValuePair> prams = uri_builder.getQueryParams();
            //    NameValuePair current = null;
            //    Iterator<NameValuePair> iter = prams.iterator();
            //    while (iter.hasNext())
            //    {
            //        current = iter.next();
            //        if (current.getName().equals("limit"))
            //        {
            //            iter.remove();
            //            String qs = URLEncodedUtils.format(prams, "UTF-8");
            //            uri_builder.setQuery(qs);
            //            break;
            //        }
            //    }
            //}
            //else
            //    uri_builder.setParameter("limit", limit.ToString());
        }

        protected string getURI()
        {
            return uri_builder.ToString();
        }

        public IEnumerable<T> iterator()
        {
            String uri = getURI();
            ResourcePage<T> page = new ResourcePage<T>(cls, uri);
            return new ResourceIterator(uri, page);
        }

        public List<T> all()
        {
            String uri = getURI();
            ResourcePage<T> page = new ResourcePage<T>(cls, uri);
            List<T> items = new List<T>(page.getTotal());
            var iterator = new ResourceIterator(uri, page);
            while (iterator.hasNext())
            {
                T obj = iterator.next();
                items.Add(obj);
            }
            return items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
