using Sitecore.JavaScriptServices.Configuration;
using Sitecore.JavaScriptServices.ViewEngine.LayoutService.Pipelines.GetLayoutServiceContext;
using Sitecore.LayoutService.ItemRendering.Pipelines.GetLayoutServiceContext;

namespace XmCloudAuthoring.Services.GraphQL.Procesors
{
    public class ExampleContextExtension : JssGetLayoutServiceContextProcessor
    {
        public ExampleContextExtension(IConfigurationResolver configurationResolver) : base(configurationResolver)
        {
        }

        protected override void DoProcess(GetLayoutServiceContextArgs args, AppConfiguration application)
        {
            args.ContextData.Add("securityInfo", new
            {
                isAnonymous = !Sitecore.Context.User.IsAuthenticated
            });
        }
    }
}