using System.Buffers;
using System.Text;
using System.Text.Json;

namespace Inomatic.HalLucinate;

public class JsonResourceStateProviderBuilder
{
    private readonly ArrayBufferWriter<byte> bufferWriter = new();

    private readonly List<JsonProperty> jsonProperties = new();

    public JsonResourceStateProviderBuilder()
    {
    }

    public void AddProperty(JsonProperty property)
    {
        jsonProperties.Add(property);
    }

    public IResourceStateProvider Build()
    {
        WritePropertiesToBuffer();
        var jsonElement = JsonElementFromBuffer();
        return new JsonElementStateProvider(jsonElement);
    }

    private JsonElement JsonElementFromBuffer()
    {
        var resultJson = Encoding.UTF8.GetString(bufferWriter.WrittenSpan);
        using var doc = JsonDocument.Parse(resultJson);
        return doc.RootElement.Clone();
    }

    private void WritePropertiesToBuffer()
    {
        using var jsonWriter = new Utf8JsonWriter(bufferWriter);
        jsonWriter.WriteStartObject();
        foreach (var property in jsonProperties)
        {
            jsonWriter.WriteProperty(property);
        }
        jsonWriter.WriteEndObject();
        jsonWriter.Flush();
    }
}