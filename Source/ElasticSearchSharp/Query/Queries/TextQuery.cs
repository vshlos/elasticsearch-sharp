using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ElasticSearchSharp.Query.Converters;
using ElasticSearchSharp.Query.Attributes;

namespace ElasticSearchSharp.Query.Queries
{
    [JsonConverter(typeof(TextQueryConverter))]
    [JsonWrapPropertyAttribute("text")]
    public class TextQuery : QueryBase
    {
        public TextQuery()
        {
            Fields = new Dictionary<string, string>();
        }

        [JsonProperty("text")]
        public Dictionary<string, string> Fields
        {
            get;
            set;
        }


        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public TextQueryType? Type { get; set; }

        [JsonProperty("operator", NullValueHandling = NullValueHandling.Ignore)]
        public BooleanOperator? Operator { get; set; }

        [JsonProperty("analyzer", NullValueHandling = NullValueHandling.Ignore)]
        public string Analyzer
        {
            get;
            set;
        }

        [JsonProperty("max_expansions", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxExpansions
        {
            get;
            set;
        }

        public class TextQueryConverter : JsonConverter
        {

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(TextQuery);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var val = value as TextQuery;
                var count = val.Fields.Count;

                //writer.WriteStartObject();
                //writer.WritePropertyName("text");

                if (count == 0)
                {
                    writer.WriteNull();
                }
                else
                {
                    var first = val.Fields.FirstOrDefault();

                    writer.WriteStartObject();
                    writer.WritePropertyName(first.Key);
                    writer.WriteStartObject();
                    writer.WritePropertyName("query");
                    writer.WriteValue(first.Value);

                    //var enumConverter = new EnumPropertyConverter();

                    if (val.Type.HasValue)
                    {
                        writer.WritePropertyName("type");
                        serializer.Serialize(writer, val.Type);
                        //enumConverter.WriteJson(writer, val.Type, serializer);
                    }

                    if (val.Operator.HasValue)
                    {
                        writer.WritePropertyName("operator");
                        serializer.Serialize(writer, val.Operator);
                        //enumConverter.WriteJson(writer, val.Operator, serializer);
                    }
                    if (val.MaxExpansions.HasValue)
                    {
                        writer.WritePropertyName("max_expansions");
                        writer.WriteValue(val.MaxExpansions.Value);
                    }

                    if (!string.IsNullOrWhiteSpace(val.Analyzer))
                    {
                        writer.WritePropertyName("analyzer");
                        writer.WriteValue(val.Analyzer);
                    }


                    writer.WriteEndObject();


                    foreach (var item in val.Fields.Skip(1))
                    {

                        writer.WritePropertyName(item.Key);
                        writer.WriteValue(first.Value);



                    }
                    writer.WriteEndObject();
                }

                //writer.WriteEndObject();

            }
        }
    }

}
