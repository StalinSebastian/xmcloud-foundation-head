using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace AspNetCoreStarter.Models
{
    public class FieldUsageNumber : HeadingAndDescription
    {
        public NumberField Sample { get; set; } = default!;
    }
}
