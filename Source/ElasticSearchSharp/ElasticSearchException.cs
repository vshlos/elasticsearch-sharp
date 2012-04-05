using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp
{
    [Serializable]
    public class ElasticSearchException : Exception
    {
        public ElasticSearchException() { }
        public ElasticSearchException(string message) : base(message) { }
        public ElasticSearchException(ElasticSearchExceptionObject exception, Exception inner)
            : base(exception.Error, inner)
        {
            StatusCode = exception.StatusCode;
        }
        public ElasticSearchException(string message, Exception inner) : base(message, inner) { }
        protected ElasticSearchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public int StatusCode
        {
            get;
            set;
        }
    }

    public class ElasticSearchExceptionObject
    {

        [JsonProperty("status")]
        public int StatusCode
        {
            get;
            set;
        }

        [JsonProperty("error")]
        public string Error
        {
            get;
            set;
        }
    }

}
