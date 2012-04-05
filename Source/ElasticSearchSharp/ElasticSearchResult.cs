using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp
{
    public abstract class ElasticSearchResult
    {

        [JsonProperty(PropertyName = "took")]
        public int Took { get; set; }
        [JsonProperty(PropertyName = "timed_out")]
        public bool TimedOut { get; set; }
        [JsonProperty(PropertyName = "_shards")]
        public Shards Shards { get; set; }

    }
}
