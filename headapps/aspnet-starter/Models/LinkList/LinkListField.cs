using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using System.Text.Json.Serialization;

namespace AspNetCoreStarter.Models.LinkList;

public class LinkListField
{
    [JsonPropertyName("title")]
    public TextField? Title { get; set; }
}