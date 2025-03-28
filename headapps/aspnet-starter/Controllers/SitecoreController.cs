using Microsoft.AspNetCore.Mvc;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Interfaces;

namespace AspNetCoreStarter.Controllers
{
    public class SitecoreController : Controller
    {
        public SitecoreController()
        {
                
        }

        public IActionResult Index([SitecoreRouteField] Field<string> pageTitle)
        {
            ISitecoreRenderingContext? request = HttpContext.GetSitecoreRenderingContext();
            ViewBag.Title = pageTitle.Value;

            return View();
        }

    }
}
