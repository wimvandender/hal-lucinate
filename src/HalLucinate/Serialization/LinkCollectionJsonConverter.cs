using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inomatic.HalLucinate.Serialization;

public class LinkCollectionJsonConverter : JsonConverter<LinkCollection>
{
    public override LinkCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var linkCollection = new LinkCollection();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return linkCollection;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            var relationType = reader.GetString();
            reader.Read();

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var link = JsonSerializer.Deserialize<Link>(ref reader, options);
                if (relationType != null && link != null)
                {
                    linkCollection.Add(relationType, link);
                }
            }
            else if (reader.TokenType == JsonTokenType.StartArray)
            {
                var linkArray = JsonSerializer.Deserialize<Link[]>(ref reader, options);
                if (relationType != null && linkArray != null)
                {
                    linkCollection.Add(relationType, linkArray);
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, LinkCollection value, JsonSerializerOptions options)
    {
        var linkDictionary = value.ToDictionary();
        writer.WriteStartObject();
        foreach (var keyValue in linkDictionary)
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