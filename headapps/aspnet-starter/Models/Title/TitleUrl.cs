using System.Text.Json.Serialization;

namespace AspNetCoreStarter.Models.Title;

public class TitleUrl
{
    public string? Path{ get; set; }
    public string? SiteName { get; set; }
}