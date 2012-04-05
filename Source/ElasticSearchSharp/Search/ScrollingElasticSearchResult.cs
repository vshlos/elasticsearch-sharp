using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp.Search
{
    public abstract class ScrollingElasticSearchResult<T> : ElasticSearchResult<T>
    {
        [JsonProperty(PropertyName = "_scroll_id")]
        public string ScrollId { get; set; }
    }
}
