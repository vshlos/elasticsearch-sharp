using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp.Search
{
    public class HighlightPartialElasticSearch<TEntity, THighlight> : FieldElasticSearchResult<TEntity>
    {
        [JsonProperty(PropertyName = "highlight")]
        public THighlight Hightlight { get; set; }
    }
}
