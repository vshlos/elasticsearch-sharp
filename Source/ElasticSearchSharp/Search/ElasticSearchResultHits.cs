using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp.Search
{
    public class ElasticSearchResultHits<T>
    {
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
        [JsonProperty(PropertyName = "max_score")]
        public double? MaxScore { get; set; }
        [JsonProperty(PropertyName = "hits")]
        public List<T> Hits { get; set; }
    }

}
