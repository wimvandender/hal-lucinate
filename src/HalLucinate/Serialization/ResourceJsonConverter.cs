using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inomatic.HalLucinate.Serialization;

public class ResourceJsonConverter : JsonConverter<Resource>
{
    public override Resource? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        LinkCollection? linkCollection = null;
        EmbeddedResourceCollection? embeddedResourceCollection = null;

        var stateJsonElementBuilder = new JsonResourceStateProviderBuilder();
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
        foreach (var property in jsonElement.EnumerateObject())
        {
            if (property.NameEquals("_links"))
            {
                linkCollection = JsonSerializer.Deserialize<LinkCollection>(property.Value.GetRawText(), options);
            }
            else if (property.NameEquals("_embedded"))
            {
                embeddedResourceCollection = JsonSerializer.Deserialize<EmbeddedResourceCollection>(property.Value.GetRawText(), options);
            }
            else
            {
                stateJsonElementBuilder.AddProperty(property);
            }
        }

        var stateProvider = stateJsonElementBuilder.Build();
        return new Resource(stateProvider)
        {
            Links = linkCollection ?? new LinkCollection(),
            EmbeddedResources = embeddedResourceCollection
        };
    }

    public override void Write(Utf8JsonWriter writer, Resource value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.Links != null)
        {
            writer.WritePropertyName("_links");
            JsonSerializer.Serialize(writer, value.Links, options);
        }
        if (value.EmbeddedResources != null)
        {
            writer.WritePropertyName("_embedded");

            JsonSerializer.Serialize(writer, value.EmbeddedResources, options);
        }

        var stateElement = value.GetState(options);
        foreach (var item in stateElement.EnumerateObject())
        {
            writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(item.Name) ?? item.Name);
            JsonSerializer.Serialize(writer, item.Value, options);
        }

        writer.WriteEndObject();
    }
}