using Microsoft.AspNetCore.Mvc;
using Sitecore.AspNet.RenderingEngine;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.AspNet.RenderingEngine.Filters;
using Sitecore.LayoutService.Client.Response.Model;

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
