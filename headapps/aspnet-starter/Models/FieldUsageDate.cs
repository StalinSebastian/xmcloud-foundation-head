using Sitecore.LayoutService.Client.Response.Model.Fields;

namespace AspNetCoreStarter.Models
{
    public class FieldUsageDate : HeadingAndDescription
    {
        public DateField Date { get; set; } = default!;

        public DateField DateTime { get; set; } = default!;
    }
}
