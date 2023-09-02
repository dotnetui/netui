using Net.Essentials.Vimeo.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.JsonConverters
{
    internal class ConnectionMetadataJsonConverter : JsonConverter<ConnectionMetadata>
    {
        public override ConnectionMetadata ReadJson(JsonReader reader, Type objectType, ConnectionMetadata existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return new ConnectionMetadata((string)reader.Value);
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                var token = JToken.Load(reader);
                return token.ToObject<ConnectionMetadata>();
            }
            else
            {
                throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing ConnectionMetadata");
            }
        }

        public override void WriteJson(JsonWriter writer, ConnectionMetadata value, JsonSerializer serializer)
        {
            string json = JsonConvert.SerializeObject(value);
            writer.WriteRawValue(json);
        }
    }
}
