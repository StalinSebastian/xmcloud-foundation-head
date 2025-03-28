using System.Text.Json.Serialization;

namespace AspNetCoreStarter.Models.Title;

public class TitleLocation
{
    public TitleUrl? Url { get; set; }
    public TitleField? Field { get; set; }
}