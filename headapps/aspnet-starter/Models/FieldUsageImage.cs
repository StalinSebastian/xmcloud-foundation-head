using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace AspNetCoreStarter.Models
{
    public class FieldUsageImage : HeadingAndDescription
    {
        public ImageField Sample1 { get; set; } = default!;

        public ImageField Sample2 { get; set; } = default!;
    }
}
