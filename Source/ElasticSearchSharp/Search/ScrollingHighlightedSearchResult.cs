using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearchSharp.Search
{
    public class ScrollingHighlightedSearchResult<TEntity, THighlight> : ScrollingElasticSearchResult<HighlightPartialElasticSearch<TEntity, THighlight>>
    {

    }

}
