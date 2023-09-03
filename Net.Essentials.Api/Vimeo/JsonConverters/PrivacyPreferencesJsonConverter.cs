using Net.Essentials.Vimeo.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.JsonConverters
{
    internal class PrivacyPreferencesJsonConverter : JsonConverter<Privacy>
    {
        public override Privacy ReadJson(JsonReader reader, Type objectType, Privacy existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return new Privacy
                {
                    RawComments = (string)reader.Value,
                    RawView = (string)reader.Value,
                };
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                var token = JToken.Load(reader);
                return token.ToObject<Privacy>();
            }
            else
            {
                throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing PrivacyPreferences");
            }
        }

        public override void WriteJson(JsonWriter writer, Privacy value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
