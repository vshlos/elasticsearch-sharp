using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace ElasticSearchSharp
{
    internal static class SerializationHelper
    {

        public static T Deserialize<T>(Stream stream, params JsonConverter[] converters)
        {
            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            using (var reader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                foreach (var converter in converters)
                    serializer.Converters.Add(converter);

                return serializer.Deserialize<T>(reader);
            }
        }

        public static void Serialize<T>(Stream stream, T item, params JsonConverter[] converters)
        {
            using (var writer = new StreamWriter(stream))
            {
                Serialize(writer, item, converters);
            }
        }

        public static void Serialize<T>(TextWriter writer, T item, params JsonConverter[] converters)
        {
            JsonSerializer serializer = new JsonSerializer();
            foreach (var converter in converters)
                serializer.Converters.Add(converter);
            serializer.Serialize(writer, item);
        }

    }
}
