using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading;
using System.Net;
using System.IO;
using ElasticSearchSharp.Search;

namespace ElasticSearchSharp
{
    public class ElasticSearchConnection 
    {
        public ElasticSearchConnection(string index, string [] hosts, TimeSpan? timeout = null)
        {
            connection = new ConnectionStringParser { Index = index, Hosts = hosts };
            this.Timeout = timeout ?? TimeSpan.FromSeconds(5);
        }


        public ElasticSearchConnection(string connectionString = "ElasticSearch", TimeSpan? timeout = null)
        {
            connection = ConnectionStringParser.GetSettings(connectionString);
            this.Timeout = timeout ?? TimeSpan.FromSeconds(5);
        }

      

        internal string CreateSearchRequestUrl(string type, string index = null, string method = "_search", string parameters = null)
        {

            if (index == null)
            {
                index = connection.Index;
            }

            //TODO: figure out a fallback, for now just use the first connection string

            var baseUrl = CreateUri(index, type, method);
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                baseUrl += "?" + parameters;
            }

            return baseUrl;

        }

        internal string CreateUri(params string[] parts)
        {
            return string.Join("/", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
        }


        internal T UseConnection<T>(string url, string method, Func<Stream, T> responseHandler, Action<Stream> requestHandler = null )
        {
            
            

            int index;
            var hostIndexStart = index = hostIndex;
            bool worked = false;
            T obj = default(T);
            
            do
            {

                var host = connection.Hosts[index];
                try
                {

                    var fullUrl = CreateUri(host, url);
                    var request = HttpWebRequest.Create(fullUrl) as HttpWebRequest;
                    request.Timeout = (int)this.Timeout.TotalMilliseconds;
                    request.Method = method;
                    request.ContentType = "application/json; charset=UTF-8";
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;


                    if (requestHandler != null)
                    {
                        using (var requestStream = request.GetRequestStream())
                        {
                            requestHandler(requestStream);
                        }
                    }

                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                        obj = responseHandler(response.GetResponseStream());
                        worked = true;
                    }

                }
                catch (WebException ex)
                {
                    //well thats why we fail over.
                    if (ex.Status != WebExceptionStatus.ConnectFailure && ex.Status != WebExceptionStatus.ConnectionClosed && ex.Status != WebExceptionStatus.Timeout)
                        throw;

                }
              

                index = (++index) % connection.Hosts.Length;
            } while (!worked && index != hostIndexStart);

            //update the index to keep it going in a circle.
            Interlocked.CompareExchange(ref hostIndex, index, hostIndexStart);


            if (!worked)
                throw new WebException("Could not find a server to connect to.");

            return obj;
        }

        public string Index
        {
            get
            {
                return connection.Index;
            }
        }

        private ConnectionStringParser connection;
  


        private int hostIndex = 0;
        public TimeSpan Timeout { get; set; }

      



    }

   
   
}
