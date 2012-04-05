using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElasticSearchSharp.Query.Attributes;
using Newtonsoft.Json;

namespace ElasticSearchSharp.Query.Queries
{
    [JsonWrapPropertyAttribute("ids")]
    public class IdsQuery : QueryBase
    {
        public IdsQuery()
        {
            Ids = new List<string>();
        }

        [JsonProperty("values")]
        public List<string> Ids { get; set; }
    }
}
