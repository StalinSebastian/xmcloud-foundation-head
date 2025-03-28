using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace AspNetCoreStarter.Models
{
    public class Tab
    {
        public TextField Title { get; set; } = default!;

        public RichTextField Content { get; set; } = default!;
    }
}
