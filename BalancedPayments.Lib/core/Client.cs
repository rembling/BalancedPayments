using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace BalancedPayments.Lib.core
{
    public class Client
    {
        private const String AGENT = "balanced-c#";
        private const int CONNECTION_TIMEOUT = 60 * 1000;
    
        private String root;
        private String key;

        public Client(Settings settings)
        {
            this.root = settings.location;
            this.key = settings.key;
        }

        public Client(string root, string key)
        {
            this.root = root;
            this.key = key;
        }

        /// <summary>
        /// Used for retreiving content
        /// </summary>
        /// <param name="address"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public Dictionary<string, object> get(string address, string body)
        {
            var request = (HttpWebRequest)WebRequest.Create(this.root + address);
            request.Method = "GET";
            request.Credentials = new NetworkCredential(this.key, "");
            request.PreAuthenticate = true;

            if (request.Method == "GET")
            {
                if (!string.IsNullOrEmpty(body))
                {
                    var requestBody = Encoding.UTF8.GetBytes(body);
                    request.ContentLength = requestBody.Length;
                    request.ContentType = "application/json";
                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(requestBody, 0, requestBody.Length);
                    }
                }
                else
                {
                    request.ContentLength = 0;
                }
            }

            request.Timeout = 15000;
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

            string output = string.Empty;
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252)))
                    {
                        output = stream.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    using (var stream = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        output = stream.ReadToEnd();
                    }
                }
                else if (ex.Status == WebExceptionStatus.Timeout)
                {
                    output = "Request timeout is expired.";
                }
            }
            //for debuggin
            //Console.WriteLine(output);
            return (Dictionary<string, object>)JsonConvert.DeserializeObject(output.ToString(), typeof(Dictionary<string, object>));
        }

        /// <summary>
        /// Used for updating content
        /// </summary>
        /// <param name="address"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public Dictionary<string, object> put(string address, object body)
        {
            string output = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(this.root + address);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.Credentials = new NetworkCredential(this.key, "");
            request.PreAuthenticate = true;
            request.Timeout = 15000;
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                byte[] postBytes = Encoding.UTF8.GetBytes(json);
                //request.ContentLength = postBytes.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(postBytes, 0, postBytes.Length);
                dataStream.Close();
                
                using (var response = request.GetResponse())
                {
                    using (var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252)))
                    {
                        output = stream.ReadToEnd();
                    }
                }
            }
            else
            {
                
                using (var response = request.GetResponse())
                {
                    using (var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252)))
                    {
                        output = stream.ReadToEnd();
                    }
                }
            }
            
            //for debuggin
            //Console.WriteLine(output);
            return (Dictionary<string, object>)JsonConvert.DeserializeObject(output.ToString(), typeof(Dictionary<string, object>));
        }

        /// <summary>
        /// Used for creating content
        /// </summary>
        /// <param name="address"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public Dictionary<string, object> post(string address, object body)
         {
             var request = (HttpWebRequest)WebRequest.Create(this.root + address);
             request.Method = "POST";
             request.ContentType = "application/json";
             request.Credentials = new NetworkCredential(this.key, "");
             request.PreAuthenticate = true;
             request.Timeout = 15000;
             request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

             if (body != null)
             {
                 var json = JsonConvert.SerializeObject(body);
                 byte[] postBytes = Encoding.UTF8.GetBytes(json);
                 request.ContentLength = postBytes.Length;
                 Stream dataStream = request.GetRequestStream();
                 dataStream.Write(postBytes, 0, postBytes.Length);
                 dataStream.Close();
             }

             string output = string.Empty;
             using (var response = request.GetResponse())
             {
                 using (var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252)))
                 {
                     output = stream.ReadToEnd();
                 }
             }
             //for debuggin
             //Console.WriteLine(output);
             return (Dictionary<string, object>)JsonConvert.DeserializeObject(output.ToString(), typeof(Dictionary<string, object>));
        }

        public void delete(string address, object body)
        {
            var request = (HttpWebRequest)WebRequest.Create(this.root + address);
            request.Method = "DELETE";
            request.ContentType = "application/json";
            request.Credentials = new NetworkCredential(this.key, "");
            request.PreAuthenticate = true;
            request.Timeout = 15000;
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

            if (body != null)
            {
                request.Method = "PUT";
                var json = JsonConvert.SerializeObject(body);
                byte[] postBytes = Encoding.UTF8.GetBytes(json);
                request.ContentLength = postBytes.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(postBytes, 0, postBytes.Length);
                dataStream.Close();
            }

            string output = string.Empty;
            using (var response = request.GetResponse())
            {
                using (var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252)))
                {
                    output = stream.ReadToEnd();
                }
            }
            //for debugging
            //Console.WriteLine(output);
        }
    
        public void delete(string address) {
            delete(address, null);
        }

        /// <summary>
        /// When the DELETE verb cannot be used (i.e. when the Resource has child records, or is associated to an account or another Resource,
        /// use this method; it PUTS a meta tag of "is_valid" = "false" and basically accomplishes the same thing as a DELETE. Note, invalid or
        /// deleted Resources may still show up in lists on the BalancedPayments dashboard website
        /// </summary>
        /// <param name="address"></param>
        /// <param name="body"></param>
        public void invalidate(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(this.root + uri);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.Credentials = new NetworkCredential(this.key, "");
            request.PreAuthenticate = true;
            request.Timeout = 15000;
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

            var body = new Dictionary<string, string>();
            body.Add("is_valid", "false");

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                byte[] postBytes = Encoding.UTF8.GetBytes(json);
                request.ContentLength = postBytes.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(postBytes, 0, postBytes.Length);
                dataStream.Close();
            }

            string output = string.Empty;
            using (var response = request.GetResponse())
            {
                using (var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252)))
                {
                    output = stream.ReadToEnd();
                }
            }
            //for debugging
            //Console.WriteLine(output);
        }

        private Uri buildUri(String path, Dictionary<string, string> prams) 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(root);
            sb.Append(path);
            if (prams != null && prams.Count() > 0) 
            {
                sb.Append("?");
                sb.Append(buildQueryString(prams));
            }
            return new Uri(sb.ToString());
        }
        private string buildQueryString(Dictionary<string, string> prams) 
        {
            StringBuilder queryString = new StringBuilder();
            foreach(var s in prams)
            {
                queryString.Append(string.Format("{0}={1}&", s.Key, s.Value));
            }
            return queryString.ToString(); 
        }
    }
}
