using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace AspNetCoreStarter.Models
{
    public class FieldUsageContentList : HeadingAndDescription
    {
        [SitecoreComponentField(Name = "localContentList")]
        public ContentListField<LinkItemTemplate> LocalContentList { get; set; } = default!;

        [SitecoreComponentField(Name = "sharedContentList")]
        public ContentListField<LinkItemTemplate> SharedContentList { get; set; } = default!;
    }
}
