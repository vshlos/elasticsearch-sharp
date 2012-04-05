using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp
{
    public class SaveItemResult : ItemOperationResult
    {
        [JsonProperty(PropertyName = "ok")]
        public bool Success { get; set; }
      
        [JsonProperty(PropertyName = "_version")]
        public int Version { get; set; }
    }
}
