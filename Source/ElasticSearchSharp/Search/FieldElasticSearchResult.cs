using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp.Search
{
    public class FieldElasticSearchResult<T> : ElasticSearchResultHit<T>
    {
        [JsonProperty(PropertyName = "fields")]
        public T Fields { get; set; }
    }

}
