using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ElasticSearchSharp.Query.Converters;

namespace ElasticSearchSharp.Query
{
    [JsonConverter(typeof(EnumPropertyConverter))]
    public enum BooleanOperator
    {
        [JsonProperty("or")]
        Or,
        [JsonProperty("and")]
        And
    }

}
