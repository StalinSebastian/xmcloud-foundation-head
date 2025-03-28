using System.Text.Json.Serialization;

namespace AspNetCoreStarter.Models.Title;

public class TitleData
{
    public TitleLocation? DataSource { get; set; }
    public TitleLocation? ContextItem { get; set; }
}