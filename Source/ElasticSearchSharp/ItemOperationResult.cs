using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp
{
    public class ItemOperationResult
    {
        [JsonProperty(PropertyName = "_index")]
        public string Index { get; set; }
        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
    }
}
