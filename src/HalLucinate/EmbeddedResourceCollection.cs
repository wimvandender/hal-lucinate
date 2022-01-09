using Inomatic.HalLucinate.Serialization;
using System.Text.Json.Serialization;

namespace Inomatic.HalLucinate;

[JsonConverter(typeof(EmbeddedResourceCollectionJsonConverter))]
public class EmbeddedResourceCollection : RelationCollection<Resource>
{
}