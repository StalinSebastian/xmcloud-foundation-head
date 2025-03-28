using System.Text.Json.Serialization;

namespace AspNetCoreStarter.Models.LinkList;

public class DatasourceField
{
    [JsonPropertyName("field")]
    public LinkListField? Field { get; set; }

    [JsonPropertyName("children")]
    public LinkListChildren? Children { get; set; }
}