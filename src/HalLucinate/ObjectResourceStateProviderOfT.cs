using System.Text.Json;

namespace Inomatic.HalLucinate;

public class ObjectResourceStateProvider<TState> : IResourceStateProvider
{
    private readonly TState state;

    public ObjectResourceStateProvider(TState state)
    {
        this.state = state;
    }

    public JsonElement GetState(JsonSerializerOptions? options = null)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(state, options);
        using var doc = JsonDocument.Parse(bytes);
        return doc.RootElement.Clone();
    }
}
