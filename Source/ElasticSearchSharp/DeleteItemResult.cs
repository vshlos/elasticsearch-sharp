using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp
{
    public class DeleteItemResult : SaveItemResult
    {
        [JsonProperty("found")]
        public bool Found { get; set; }
    }
}
