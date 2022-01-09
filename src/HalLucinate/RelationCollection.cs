using System.Collections;

namespace Inomatic.HalLucinate;

public class RelationCollection<T> : IEnumerable
{
    private readonly HashSet<string> forcedCollectionRelations = new();

    private readonly List<(string Rel, T Value)> items = new();

    public void Add(string rel, T value)
    {
        items.Add((rel, value));
    }

    public void Add(string rel, IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            items.Add((rel, value));
        }
        forcedCollectionRelations.UnionWith(new[] { rel });
    }

    public IEnumerator GetEnumerator()
    {
        return ToDictionary().GetEnumerator();
    }

    public bool ShouldSerializeAsCollection(string rel)
    {
        return items.Count(x => x.Rel == rel) > 1 || forcedCollectionRelations.Contains(rel);
    }

    public IDictionary<string, IReadOnlyList<T>> ToDictionary()
    {
        var lookup = items.ToLookup(x => x.Rel, x => x.Value);
        return lookup.ToDictionary(x => x.Key, x => (IReadOnlyList<T>)x.ToList().AsReadOnly());
    }
}