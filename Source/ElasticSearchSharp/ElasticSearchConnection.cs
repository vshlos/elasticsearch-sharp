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
        public ElasticSearchConnection(string index, string [] hosts)
        {
            connection = new ConnectionStringParser { Index = index, Hosts = hosts };
        }


        public ElasticSearchConnection(string connectionString = "ElasticSearch")
        {
            connection = ConnectionStringParser.GetSettings(connectionString);
        }

        internal HttpWebRequest CreateRequest(string url, string method)
        {
            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = method;
            request.ContentType = "application/json; charset=UTF-8";
            return request;
        }

        internal HttpWebRequest CreateRequest(string httpMethod, string type, string index = null, string method = "_search", string parameters = null)
        {
            return CreateRequest(CreateSearchRequestUrl(type, index, method, parameters), httpMethod);
        }

        internal string CreateSearchRequestUrl(string type, string index = null, string method = "_search", string parameters = null)
        {

            if (index == null)
            {
                index = connection.Index;
            }

            //TODO: figure out a fallback, for now just use the first connection string

            var baseUrl = CreateUri(connection.Hosts[0], index, type, method);
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

        public string Index
        {
            get
            {
                return connection.Index;
            }
        }

        private ConnectionStringParser connection;
    }


   
   
}
