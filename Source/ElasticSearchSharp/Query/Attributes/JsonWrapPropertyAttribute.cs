
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearchSharp.Query.Attributes
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class JsonWrapPropertyAttribute : Attribute
    {

        readonly string wrapName;

        public JsonWrapPropertyAttribute(string wrapName)
        {
            this.wrapName = wrapName;

        }

        public string WrapName
        {
            get
            {
                return wrapName;
            }
        }


    }
}
