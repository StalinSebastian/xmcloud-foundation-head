using AspNetCoreStarter.Configuration;
using AspNetCoreStarter.Extensions;
using AspNetCoreStarter.Models;
using Microsoft.AspNetCore.Localization;
using Sitecore.AspNetCore.SDK.ExperienceEditor.Extensions;
using Sitecore.AspNetCore.SDK.GraphQL.Extensions;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var handler = builder.Configuration.GetSection(DefaultHandlerOptions.Key).Get<DefaultHandlerOptions>();
var experienceEditor = builder.Configuration.GetSection(ExperienceEditorConfiguration.Key).Get<ExperienceEditorConfiguration>();
var jssEditingSecret = builder.Configuration.GetValue<string>(ExperienceEditorConfiguration.JssEditingSecretKey);

SitecoreSettings? sitecoreSettings = builder.Configuration.GetSection(SitecoreSettings.Key).Get<SitecoreSettings>();
ArgumentNullException.ThrowIfNull(sitecoreSettings);

builder.Services.AddRouting()
                .AddLocalization()
                .AddMvc();

builder.Services.AddGraphQlClient(configuration =>
                {
                    configuration.ContextId = sitecoreSettings.EdgeContextId;
                })
                .AddMultisite();

if (sitecoreSettings.EnableLocalContainer)
{
    // Register the GraphQL version of the Sitecore Layout Service Client for use against local container endpoint
    builder.Services.AddSitecoreLayoutService()
                    .AddGraphQlHandler("default", sitecoreSettings.DefaultSiteName!, sitecoreSettings.EdgeContextId!, sitecoreSettings.LocalContainerLayoutUri!)
                    .AsDefaultHandler();
}
else
{
    // Register the GraphQL version of the Sitecore Layout Service Client for use against experience edge
    builder.Services.AddSitecoreLayoutService()
                    .AddGraphQlWithContextHandler("default", sitecoreSettings.EdgeContextId!, siteName: sitecoreSettings.DefaultSiteName!)
                    .AsDefaultHandler();
}
builder.Services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

// builder.Services.AddSitecoreLayoutService()
//                 .AddHttpHandler(handler.Name, handler.Uri)
//                 .WithRequestOptions(request =>
//                 {
//                     foreach (var entry in handler.RequestDefaults)
//                         request.Add(entry.Key, entry.Value);
//                 })
//                 .AsDefaultHandler();

builder.Services.AddSitecoreRenderingEngine(options =>
    {
        options.AddModelBoundView<ContentBlock>("ContentBlock")
            .AddStyleguideViews()
            .AddStarterKitViews()
            .AddDefaultPartialView("_ComponentNotFound");
    })
    .WithExperienceEditor(options =>
        {
            options.Endpoint = experienceEditor.Endpoint;
            options.JssEditingSecret = jssEditingSecret;
            //This is an example to show how we can target custom routes for the Experience Editor by adding custom mapping handlers.
            options.MapToRequest((sitecoreResponse, scPath, httpRequest) =>
                httpRequest.Path = scPath + sitecoreResponse?.Sitecore?.Route?.DatabaseName);
        });
//.WithTracking();

//builder.Services.AddSitecoreVisitorIdentification(options =>
//{
//    // Usually SitecoreInstanceHostName is same as Layout service but can be any Sitecore CD/CM instance which shares same AspNet session with Layout Service.
//    // This Sitecore instance will be used for Visitor identification.
//    var uriSetting = builder.Configuration.GetSection("Analytics:SitecoreInstanceUri").Get<Uri>();
//    options.SitecoreInstanceUri = uriSetting ?? new Uri("https://SitecoreInstanceHostName");
//});

//This configuration necessary for proper resolving of IP address and Scheme of original request in case reverse proxies sends XForwarded sets of headers. See Tracking documentation for details.
// Uncomment if you expect to resolve x-forwarded headers.
//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
//});

// Add services to the container.
//builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

if (sitecoreSettings.EnableEditingMode)
{
    app.UseSitecoreExperienceEditor();
}

app.UseRouting();
app.UseMultisite();
app.UseStaticFiles();

// Make sure to resolve IP address before Rendering engine functionality. It will allow xDb to record real client IP address.
// Uncomment if you expect to resolve x-forwarded headers.
//app.UseForwardedHeaders();
//app.UseSitecoreVisitorIdentification();

//Adds localization functionality
//Calling UseSitecoreRequestLocalization() on the localization  allows culture to be resolved from both the sc_lang query string and the culture token from route data.
app.UseRequestLocalization(options =>
{
    var supportedCultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("da"), new CultureInfo("da-DK") };
    options.DefaultRequestCulture = new RequestCulture(culture: "da", uiCulture: "da");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.UseSitecoreRequestLocalization();
});

app.UseSitecoreRenderingEngine();

app.MapControllerRoute(
    "error",
    "error",
    new { controller = "Default", action = "Error" }
);

//app.MapSitecoreLocalizedRoute("Localized", "Index", "Sitecore");
app.MapSitecoreLocalizedRoute("sitecore", "Index", "Default");
//app.MapFallbackToController("Index", "Sitecore");
app.MapFallbackToController("Index", "Default");

app.Run();
