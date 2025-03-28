using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;

namespace AspNetCoreStarter.Models;

public class PartialDesignDynamicPlaceholder : BaseModel
{
    [SitecoreComponentParameter(Name ="sig")]
    public string? Sig { get; set; }
}
