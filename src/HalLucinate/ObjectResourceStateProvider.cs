using System.Text.Json;

namespace Inomatic.HalLucinate;

public class ObjectResourceStateProvider : IResourceStateProvider
{
    private readonly object state;

    public ObjectResourceStateProvider(object state)
    {
        this.state = state;
    }

    public JsonElement GetState(JsonSerializerOptions? options = null)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(state, state.GetType(), options);
        using var doc = JsonDocument.Parse(bytes);
        return doc.RootElement.Clone();
    }
}
