using System.Text.Json;

namespace Inomatic.HalLucinate;

public class JsonElementStateProvider : IResourceStateProvider
{
    private readonly JsonElement jsonElement;

    public JsonElementStateProvider(JsonElement jsonElement)
    {
        this.jsonElement = jsonElement;
    }

    public JsonElement GetState(JsonSerializerOptions? options = null)
    {
        return jsonElement;
    }
}
