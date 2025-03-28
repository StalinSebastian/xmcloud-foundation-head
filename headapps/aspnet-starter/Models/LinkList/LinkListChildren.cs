using System.Text.Json.Serialization;

namespace AspNetCoreStarter.Models.LinkList;

public class LinkListChildren
{
    [JsonPropertyName("results")]
    public List<LinkListItem>? Results { get; set; }
}