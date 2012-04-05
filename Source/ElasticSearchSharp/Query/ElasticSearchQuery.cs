using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using Newtonsoft.Json.Converters;
using ElasticSearchSharp.Query.Queries;
using ElasticSearchSharp.Query.Converters;

namespace ElasticSearchSharp.Query
{
    public class ElasticSearchQuery
    {
        public ElasticSearchQuery()
        {

        }

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Fields
        {
            get;
            set;
        }

        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public int? Skip
        {
            get;
            set;
        }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public int? Limit
        {
            get;
            set;
        }

        [JsonProperty("query")]
        [JsonConverter(typeof(ObjectWrapConverter))]
        public QueryBase Query
        {
            get;
            set;
        }

        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                SerializationHelper.Serialize(writer, this, new IsoDateTimeConverter());

                return writer.ToString();
            }

        }
    }

   


  

  
}
