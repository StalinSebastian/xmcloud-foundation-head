using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreStarter.Models
{
    public class LinkItemTemplate
    {
        [SitecoreComponentField(Name = "textField")]
        public TextField TextField { get; set; } = default!;
    }
}
