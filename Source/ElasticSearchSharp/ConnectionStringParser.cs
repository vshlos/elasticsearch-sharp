using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;

namespace ElasticSearchSharp
{
    internal class ConnectionStringParser
    {
        public static ConnectionStringParser GetSettings(string connectionStringName = "ElasticSearch")
        {
            string connString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            return JsonConvert.DeserializeObject<ConnectionStringParser>(connString);
        }

        [JsonProperty("index")]
        public string Index
        {
            get;
            set;
        }

        [JsonProperty("hosts")]
        public string[] Hosts
        {
            get;
            set;
        }
    }
}
