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

            var request = Connection.CreateRequest(url, "PUT");
            using (var requestStream = request.GetRequestStream())
            {
                //serialize request
                SerializationHelper.Serialize(requestStream, item, new IsoDateTimeConverter());

                //get response
                var response = request.GetResponse();

                var result = SerializationHelper.Deserialize<SaveItemResult>(response.GetResponseStream());

                return result;


            }
        }


        public SaveItemResult Update(string id, string script)
        {
            var url = Connection.CreateSearchRequestUrl(CollectionName, method: null);
            if (!string.IsNullOrWhiteSpace(id))
            {
                url = string.Format("{0}/{1}/_update", url, id);
            }

            var request = Connection.CreateRequest(url, "POST");
            using (var requestStream = request.GetRequestStream())
            {
                //serialize request
                SerializationHelper.Serialize(requestStream, new { script = script }, new IsoDateTimeConverter());

                //get response
                var response = request.GetResponse();

                var result = SerializationHelper.Deserialize<SaveItemResult>(response.GetResponseStream());

                return result;


            }
        }



        public DeleteItemResult Remove(string id)
        {
            var url = Connection.CreateUri(Connection.CreateSearchRequestUrl(this.CollectionName, method: null), id);
            try
            {
                var request = Connection.CreateRequest(url, "DELETE");

                var response = request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    return SerializationHelper.Deserialize<DeleteItemResult>(stream, new IsoDateTimeConverter());

                }
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
                var request = !string.IsNullOrWhiteSpace(url) ? Connection.CreateRequest(url, "POST") : Connection.CreateRequest("POST", type: CollectionName);
                using (var requestStream = request.GetRequestStream())
                {
                    SerializationHelper.Serialize(requestStream, searchQuery, new IsoDateTimeConverter());
                }

                var response = request.GetResponse();

                using (var responseStream = response.GetResponseStream())
                {
                    return SerializationHelper.Deserialize<TS>(responseStream);
                }
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
                var request = Connection.CreateRequest(url, "GET");

                var response = request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    var dict = SerializationHelper.Deserialize<SimpleElasticSearchResultHit<T>>(stream, new IsoDateTimeConverter());
                    return dict.Source;
                }
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
