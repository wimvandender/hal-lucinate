using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inomatic.HalLucinate.Serialization;

public class EmbeddedResourceCollectionJsonConverter : JsonConverter<EmbeddedResourceCollection>
{
    public override EmbeddedResourceCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var embeddedResourceCollection = new EmbeddedResourceCollection();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return embeddedResourceCollection;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            var relationType = reader.GetString();
            reader.Read();

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var resource = JsonSerializer.Deserialize<Resource>(ref reader, options);
                if (relationType != null && resource != null)
                {
                    embeddedResourceCollection.Add(relationType, resource);
                }
            }
            else if (reader.TokenType == JsonTokenType.StartArray)
            {
                var resourceArray = JsonSerializer.Deserialize<Resource[]>(ref reader, options);
                if (relationType != null && resourceArray != null)
                {
                    embeddedResourceCollection.Add(relationType, resourceArray);
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, EmbeddedResourceCollection value, JsonSerializerOptions options)
    {
        var resourceDictionary = value.ToDictionary();
        writer.WriteStartObject();
        foreach (var keyValue in resourceDictionary)
        {
            writer.WritePropertyName(keyValue.Key);
            if (value.ShouldSerializeAsCollection(keyValue.Key))
            {
                var valueToSerialize = Enumerable.ToArray(keyValue.Value);
                JsonSerializer.Serialize(writer, valueToSerialize, valueToSerialize.GetType(), options);
            }
            else
            {
                var valueToSerialize = keyValue.Value[0];
                JsonSerializer.Serialize(writer, valueToSerialize, valueToSerialize.GetType(), options);
            }
        }
        writer.WriteEndObject();
    }
}