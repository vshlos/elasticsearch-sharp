using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp.Search
{
    public abstract class ElasticSearchResultHit<T> : ItemOperationResult
    {
        [JsonProperty(PropertyName = "_score")]
        public double? Score { get; set; }

    }

    
}
