using System.Text.Json;

namespace Inomatic.HalLucinate;

public interface IResourceStateProvider
{
    JsonElement GetState(JsonSerializerOptions? options = null);
}
