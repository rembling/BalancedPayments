using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedPayments.Lib.core
{
    public class ResourceCollection<T> : ResourcePagination<T>
    {
        public ResourceCollection(T cls, string uri) : base(cls, uri)
        {
        }

        public ResourceQuery<T> query()
        {
            return new ResourceQuery<T>(cls, uri_builder.ToString());
        }
    }
}
