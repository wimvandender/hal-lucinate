using System.Text.Json;
using System.Text.Json.Serialization;
using Inomatic.HalLucinate.Serialization;

namespace Inomatic.HalLucinate;

[JsonConverter(typeof(ResourceJsonConverter))]
public class Resource : IResourceStateProvider
{
    private readonly IResourceStateProvider stateProvider;

    public Resource()
    {
        stateProvider = new ObjectResourceStateProvider(new { });
    }

    public Resource(object state) : this(new ObjectResourceStateProvider(state))
    {
    }

    public Resource(IResourceStateProvider stateProvider)
    {
        this.stateProvider = stateProvider;
    }

    public EmbeddedResourceCollection? EmbeddedResources { get; set; }

    public LinkCollection? Links { get; set; }

    public JsonElement GetState(JsonSerializerOptions? options = null)
    {
        return stateProvider.GetState(options);
    }
}
