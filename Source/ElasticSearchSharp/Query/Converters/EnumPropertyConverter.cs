using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearchSharp.Query.Converters
{
    internal class EnumPropertyConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = existingValue.ToString();
            foreach (var member in objectType.GetMembers())
            {
                //check if ignore
                if (!member.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any())
                {

                    //check rest
                    var attribute = member.GetCustomAttributes(typeof(JsonPropertyAttribute), true).FirstOrDefault();
                    if (attribute != null)
                    {
                        var prop = attribute as JsonPropertyAttribute;
                        if (prop != null && prop.PropertyName == value)
                            return Enum.Parse(objectType, member.Name);
                    }
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var propType = value.GetType();
            var name = Enum.GetName(propType, value);
            var writeValue = name;
            var member = propType.GetMember(name).FirstOrDefault();
            if (member != null)
            {
                //check if ignore
                if (member.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any())
                {
                    writeValue = null;
                }
                else
                {

                    //check rest
                    var attribute = member.GetCustomAttributes(typeof(JsonPropertyAttribute), true).FirstOrDefault();
                    if (attribute != null)
                    {
                        var prop = attribute as JsonPropertyAttribute;
                        if (prop != null)
                            writeValue = prop.PropertyName;
                    }
                }
            }
            serializer.Serialize(writer, writeValue);
        }
    }
}
