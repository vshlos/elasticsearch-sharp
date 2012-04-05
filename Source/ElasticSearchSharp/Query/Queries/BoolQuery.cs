
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Collections;
using Newtonsoft.Json.Serialization;
using ElasticSearchSharp.Query.Attributes;
using ElasticSearchSharp.Query.Converters;

namespace ElasticSearchSharp.Query.Queries
{

    
    [JsonWrapPropertyAttribute("bool")]
    public class BoolQuery : QueryBase
    {

        [JsonProperty("must", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ObjectWrapConverter))]
        public QueryBase Must { get; set; }


        [JsonProperty("must_not", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ObjectWrapConverter))]
        public QueryBase MustNot { get; set; }


        [JsonProperty("should", NullValueHandling=NullValueHandling.Ignore)]
        [JsonConverter(typeof(ListObjectWrapConverter))]
        public List<QueryBase> Should { get; set; }

        [JsonProperty("minimum_number_should_match", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinimumNumberShouldMatch { get; set; }

        [JsonProperty("disable_coord", NullValueHandling = NullValueHandling.Ignore)]
        public bool DisableCoord { get; set; }

        [JsonProperty("boost", NullValueHandling = NullValueHandling.Ignore)]
        public double? Boost { get; set; }

    }




 
    
}
