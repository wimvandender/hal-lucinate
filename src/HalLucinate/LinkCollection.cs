using System.Text.Json.Serialization;
using Inomatic.HalLucinate.Serialization;

namespace Inomatic.HalLucinate;

[JsonConverter(typeof(LinkCollectionJsonConverter))]
public class LinkCollection : RelationCollection<Link>
{
}
