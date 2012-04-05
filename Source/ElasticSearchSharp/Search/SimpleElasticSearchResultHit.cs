using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp.Search
{
    public class SimpleElasticSearchResultHit<T> : ElasticSearchResultHit<T>
    {
        [JsonProperty(PropertyName = "_source")]
        public T Source { get; set; }
    }
}
