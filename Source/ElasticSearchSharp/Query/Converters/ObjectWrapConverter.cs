using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ElasticSearchSharp.Query.Queries;
using ElasticSearchSharp.Query.Attributes;

namespace ElasticSearchSharp.Query.Converters
{
    internal class ObjectWrapConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            writer.WriteStartObject();
            if (value != null)
            {
                var type = value.GetType();
                var name = type.Name;
                var attribute = type.GetCustomAttributes(typeof(JsonWrapPropertyAttribute), true).FirstOrDefault() as JsonWrapPropertyAttribute;
                if (attribute != null)
                {
                    name = attribute.WrapName;
                }

                writer.WritePropertyName(name);
                serializer.Serialize(writer, value);

            }
            writer.WriteEndObject();
        }
    }

}
