using System.Text.Json.Serialization;

namespace AspNetCoreStarter.Models.LinkList;

public class LinkListItem 
{
    [JsonPropertyName("field")]
    public LinkListItemField? Field { get; set; }
}