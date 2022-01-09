using System.Text.Json;

namespace Inomatic.HalLucinate;

internal static class Utf8JsonWriterExtensions
{
    public static void WriteProperty(this Utf8JsonWriter utf8JsonWriter, JsonProperty property)
    {
        utf8JsonWriter.WritePropertyName(property.Name);
        property.Value.WriteTo(utf8JsonWriter);
    }
}