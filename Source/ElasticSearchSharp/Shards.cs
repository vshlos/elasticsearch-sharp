using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp
{
    public class Shards
    {
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
        [JsonProperty(PropertyName = "successful")]
        public int Successful { get; set; }
        [JsonProperty(PropertyName = "failed")]
        public int Failed { get; set; }
    }
  
 

    
  

}
