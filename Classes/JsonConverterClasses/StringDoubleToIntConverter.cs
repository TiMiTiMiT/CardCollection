using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace CardCollection.Classes.JsonConverterClasses
{
    public class StringDoubleToIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Handle JSON string like "1.0"
            if (reader.TokenType == JsonTokenType.String)
            {
                string raw = reader.GetString();
                if (double.TryParse(raw, out double value))
                    return (int)value;
            }

            // Handle JSON number like 1.0
            if (reader.TokenType == JsonTokenType.Number)
            {
                return (int)reader.GetDouble();
            }

            throw new JsonException("Expected number or numeric string.");
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
