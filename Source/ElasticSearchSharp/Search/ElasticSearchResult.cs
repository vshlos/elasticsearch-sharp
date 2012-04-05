using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp.Search
{
    public abstract class ElasticSearchResult<T> : ElasticSearchResult
    {
        [JsonProperty(PropertyName = "hits")]
        public ElasticSearchResultHits<T> Hits { get; set; }
    }

   
    
}
