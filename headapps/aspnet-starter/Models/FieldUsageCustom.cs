using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model;

namespace AspNetCoreStarter.Models
{
    public class FieldUsageCustom : HeadingAndDescription
    {
        public Field<int> CustomIntField { get; set; } = default!;
    }
}
