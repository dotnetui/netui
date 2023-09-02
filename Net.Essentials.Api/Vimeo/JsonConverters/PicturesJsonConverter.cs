using Net.Essentials.Vimeo.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.JsonConverters
{
    internal class PicturesJsonConverter : JsonConverter<Pictures>
    {
        public override Pictures ReadJson(JsonReader reader, Type objectType, Pictures existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                return new Pictures
                {
                    Sizes = serializer.Deserialize<List<Picture>>(reader),
                };
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                var token = JToken.Load(reader);
                return token.ToObject<Pictures>();
            }
            else
            {
                throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing Pictures");
            }
        }

        public override void WriteJson(JsonWriter writer, Pictures value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
