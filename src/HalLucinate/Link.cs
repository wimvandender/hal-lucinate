namespace Inomatic.HalLucinate;

public class Link
{
    public Link(string href)
    {
        Href = href;
    }

    public string? Deprication { get; set; }

    public string Href { get; set; }

    public string? Hreflang { get; set; }

    public string? Name { get; set; }

    public string? Profile { get; set; }

    public bool Templated { get; set; }

    public string? Title { get; set; }

    public string? Type { get; set; }

    public static implicit operator Link(string href)
    {
        return new Link(href);
    }

    public Uri AsUri(UriKind uriKind = UriKind.Relative)
    {
        return new Uri(Href, uriKind);
    }
}
