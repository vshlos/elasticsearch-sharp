using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using ElasticSearchSharp.Search;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Collections;
using System.Linq.Expressions;
using ElasticSearchSharp.Query;

namespace ElasticSearchSharp
{
    public class ElasticSearchCollection<T> : IQueryable, IQueryable<T>, IOrderedQueryable, IOrderedQueryable<T>
    {
        public ElasticSearchCollection(string collectionName, ElasticSearchConnection connection)
        {
            CollectionName = collectionName;
            Connection = connection;
        }

        public ElasticSearchCollection(ElasticSearchConnection connection)
        {
            CollectionName = typeof(T).Name;
            Connection = connection;
        }


        public string CollectionName
        {
            get;
            private set;
        }

        public ElasticSearchConnection Connection
        {
            get;
            private set;
        }


        public SaveItemResult Save(T item)
        {
            return Save(null, item);
        }


        public SaveItemResult Save(string id, T item)
        {
            var url = Connection.CreateSearchRequestUrl(CollectionName, method: null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                url = string.Format("{0}/{1}", url, id);
            }


            return Connection.UseConnection(url, "PUT",
                result => SerializationHelper.Deserialize<SaveItemResult>(result),
                request => SerializationHelper.Serialize(request, item, new IsoDateTimeConverter()));
        }


        public SaveItemResult Update(string id, string script)
        {
            var url = Connection.CreateSearchRequestUrl(CollectionName, method: null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                url = string.Format("{0}/{1}/_update", url, id);
            }


            return Connection.UseConnection(url, "POST",
                 result => SerializationHelper.Deserialize<SaveItemResult>(result),
                 request => SerializationHelper.Serialize(request, new { script = script }, new IsoDateTimeConverter()));
        }



        public DeleteItemResult Remove(string id)
        {
            var url = Connection.CreateUri(Connection.CreateSearchRequestUrl(this.CollectionName, method: null), id);
            try
            {

                return Connection.UseConnection(url, "DELETE",
                    result => SerializationHelper.Deserialize<DeleteItemResult>(result));

            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response == null || response.StatusCode != HttpStatusCode.NotFound)
                    throw;
            }

            return new DeleteItemResult() { Found = false, Success = false, Type = CollectionName, Index = Connection.Index };

        }

        public IEnumerable<HighlightPartialElasticSearch<T, THighlight>> FindScrollingAndHighlight<THighlight>(object query, TimeSpan scrollTime, ref string scrollId)
        {

            string url;
            if (string.IsNullOrWhiteSpace(scrollId))
            {
                url = Connection.CreateSearchRequestUrl(CollectionName, parameters: "scroll=" + TimespanToTimeString(scrollTime));
            }
            else
            {
                url = Connection.CreateSearchRequestUrl("scroll", index: "_search", parameters: "scroll=" + TimespanToTimeString(scrollTime) + "&scroll_id=" + scrollId, method: null);
            }
            var results = FindAs<ScrollingHighlightedSearchResult<T, THighlight>>(query, url);
            if (results == null)
                return null;
            scrollId = results.ScrollId;
            return results.Hits.Hits;
        }




        public IEnumerable<T> FindScrolling(object query, TimeSpan scrollTime, ref string scrollId)
        {

            string url;
            if (string.IsNullOrWhiteSpace(scrollId))
            {
                url = Connection.CreateSearchRequestUrl(CollectionName, parameters: "scroll=" + TimespanToTimeString(scrollTime));
            }
            else
            {
                url = Connection.CreateSearchRequestUrl("scroll", index: "_search", parameters: "scroll=" + TimespanToTimeString(scrollTime) + "&scroll_id=" + scrollId, method: null);
            }
            var results = FindAs<ScrollingElasticSearchResult<T>>(query, url);
            if (results == null)
                return null;
            scrollId = results.ScrollId;
            return results.Hits.Hits;
        }

        public IEnumerable<HighlightPartialElasticSearch<T, THighlight>> FindAndHighlight<THighlight>(object query)
        {
            var results = FindAs<HighlightedSearchResult<T, THighlight>>(query);
            if (results == null)
                return null;
            return results.Hits.Hits;
        }

        public IEnumerable<T> FindFields(object query)
        {
            var results = FindAs<FieldSearchResults<T>>(query);
            if (results == null)
                return null;
            return results.Hits.Hits.Select(k => k.Fields);
        }

        public IEnumerable<T> Find(ElasticSearchQuery query)
        {
            if (query.Fields != null)
                return FindFields(query);
            else
                return Find((object)query);

        }

        public IEnumerable<T> Find(object query)
        {
            var results = FindAs<SimpleElasticSearchResult<T>>(query);
            if (results == null)
                return null;
            return results.Hits.Hits.Select(k => k.Source);
        }

        public T FindOne(object query)
        {
            return FindAs<SimpleElasticSearchResultHit<T>>(query).Source;
        }


        public TS FindAs<TS>(object searchQuery)
        {
            return FindAs<TS>(searchQuery, url: null);
        }

        private TS FindAs<TS>(object searchQuery, string url = null)
        {
            try
            {



                url = url ?? Connection.CreateSearchRequestUrl(CollectionName);

                return Connection.UseConnection(url, "POST",
                      result => SerializationHelper.Deserialize<TS>(result),
                      request => SerializationHelper.Serialize(request, searchQuery, new IsoDateTimeConverter()));

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var exception = SerializationHelper.Deserialize<ElasticSearchExceptionObject>(ex.Response.GetResponseStream());
                    throw new ElasticSearchException(exception, ex);
                }
                throw;
            }
        }


        public T GetById(string id)
        {
            var url = Connection.CreateUri(Connection.CreateSearchRequestUrl(this.CollectionName, method: null), id);
            try
            {

                return Connection.UseConnection(url, "GET",
                   result =>
                   {
                       var dict = SerializationHelper.Deserialize<SimpleElasticSearchResultHit<T>>(result, new IsoDateTimeConverter());
                       return dict.Source;
                   });

                
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response == null || response.StatusCode != HttpStatusCode.NotFound)
                    throw;
            }

            return default(T);
        }


        private string TimespanToTimeString(TimeSpan scrollTime)
        {
            return string.Format("{0}m", scrollTime.TotalMinutes);
        }




        #region "IQueryable"
        public Type ElementType
        {
            get { return typeof(T); }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return expression; }
        }

        public IQueryProvider Provider
        {
            get { return provider; }
        }


        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)this.provider.Execute(this.expression)).GetEnumerator();
        }


        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)this.provider.Execute(this.expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        Expression IQueryable.Expression
        {
            get { return expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return provider; }
        }

        private LinqQueryProvider provider;
        private Expression expression;

        #endregion
    }
}
