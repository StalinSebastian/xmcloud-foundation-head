using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace AspNetCoreStarter.Models
{
    public class FieldUsageItemLink : HeadingAndDescription
    {
        [SitecoreComponentField(Name = "localItemLink")]
        public ItemLinkField<LinkItemTemplate> LocalItemLink { get; set; } = default!;

        [SitecoreComponentField(Name = "sharedItemLink")]
        public ItemLinkField<LinkItemTemplate> SharedItemLink { get; set; } = default!;
    }
}
