using System.Text.Json.Serialization;

namespace AspNetCoreStarter.Models.LinkList;

public class DataField
{
    [JsonPropertyName("datasource")]
    public DatasourceField? Datasource { get; set; }
}