using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ElasticSearchSharp.Query.Converters;

namespace ElasticSearchSharp.Query.Queries
{

    [JsonConverter(typeof(EnumPropertyConverter))]
    public enum TextQueryType
    {
        [JsonProperty("phrase")]
        Phrase,
        [JsonProperty("prefix_phrase")]
        Prefix_Phrase
    }
}
