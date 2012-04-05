using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElasticSearchSharp.Query.Attributes;
using Newtonsoft.Json;

namespace ElasticSearchSharp.Query.Queries
{
    [JsonWrapPropertyAttribute("match_all")]
    public class MatchAllQuery :QueryBase
    {
        [JsonProperty("boost", NullValueHandling = NullValueHandling.Ignore)]
        public double? Boost { get; set; }

        [JsonProperty("norms_field", NullValueHandling = NullValueHandling.Ignore)]
        public string BoostingField { get; set; }
    }
}
