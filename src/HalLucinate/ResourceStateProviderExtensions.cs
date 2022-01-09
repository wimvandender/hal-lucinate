using System.Buffers;
using System.Text.Json;

namespace Inomatic.HalLucinate;

public static class ResourceStateProviderExtensions
{
    public static object? ToObject(this IResourceStateProvider resourceStateProvider, Type returnType, JsonSerializerOptions? options = null)
    {
        var element = resourceStateProvider.GetState(options);
        var json = GetJsonReadonlySpanForElement(element);
        return JsonSerializer.Deserialize(json, returnType, options);
    }

    public static T? ToObject<T>(this IResourceStateProvider resourceStateProvider, JsonSerializerOptions? options = null)
    {
        var element = resourceStateProvider.GetState(options);
        var json = GetJsonReadonlySpanForElement(element);
        return JsonSerializer.Deserialize<T>(json, options);
    }

    private static ReadOnlySpan<byte> GetJsonReadonlySpanForElement(JsonElement jsonElement)
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(bufferWriter);
        jsonElement.WriteTo(writer);
        writer.Flush();
        return bufferWriter.WrittenSpan;
    }
}
