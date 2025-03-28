using AspNetCoreStarter.Models;
using Microsoft.AspNetCore.Mvc;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model;
using Sitecore.AspNetCore.SDK.RenderingEngine.Binding.Attributes;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Interfaces;
using System.Runtime;

namespace AspNetCoreStarter.Controllers
{
    public class SitecoreController : Controller
    {
        private readonly ILogger<DefaultController> _logger;
        private readonly IConfiguration _configuration;
        private readonly SitecoreSettings? _settings;

        public SitecoreController(ILogger<DefaultController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            _settings = configuration.GetSection(SitecoreSettings.Key).Get<SitecoreSettings>();
            ArgumentNullException.ThrowIfNull(_settings);
        }

        public IActionResult Index([SitecoreRouteField] Field<string> pageTitle)
        {
            ISitecoreRenderingContext? request = HttpContext.GetSitecoreRenderingContext();
            ViewBag.Title = pageTitle.Value;

            return View();
        }

    }
}
