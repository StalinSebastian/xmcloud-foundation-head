using Sitecore.AspNetCore.SDK.RenderingEngine.Configuration;
using AspNetCoreStarter.Models.LinkList;
using AspNetCoreStarter.Models.Navigation;
using AspNetCoreStarter.Models.Title;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using AspNetCoreStarter.Models;

namespace AspNetCoreStarter.Extensions;

public static class ServiceCollectionExtensions
{
    public static RenderingEngineOptions AddStyleguideViews(this RenderingEngineOptions renderingEngineOptions)
    {
        renderingEngineOptions.AddPartialView("Styleguide-Layout", "_StyleguideLayout")
                                .AddPartialView("GraphQL-Layout", "_GraphQLLayout")
                                .AddModelBoundView<HeadingAndDescription>("Styleguide-ComponentParams")
                                .AddModelBoundView<CustomRouteType>("Styleguide-CustomRouteType")
                                .AddModelBoundView<FieldUsageCheckbox>("Styleguide-FieldUsage-Checkbox")
                                .AddModelBoundView<FieldUsageCustom>("Styleguide-FieldUsage-Custom")
                                .AddModelBoundView<FieldUsageDate>("Styleguide-FieldUsage-Date")
                                .AddModelBoundView<FieldUsageFile>("Styleguide-FieldUsage-File")
                                .AddModelBoundView<FieldUsageImage>("Styleguide-FieldUsage-Image")
                                .AddModelBoundView<FieldUsageLink>("Styleguide-FieldUsage-Link")
                                .AddModelBoundView<FieldUsageItemLink>("Styleguide-FieldUsage-ItemLink")
                                .AddModelBoundView<FieldUsageContentList>("Styleguide-FieldUsage-ContentList")
                                .AddModelBoundView<FieldUsageNumber>("Styleguide-FieldUsage-Number")
                                .AddModelBoundView<FieldUsageText>("Styleguide-FieldUsage-Text")
                                .AddModelBoundView<FieldUsageRichText>("Styleguide-FieldUsage-RichText")
                                .AddModelBoundView<HeadingAndDescription>("Styleguide-Layout-Reuse")
                                .AddModelBoundView<HeadingAndDescription>("Styleguide-Layout-Tabs")
                                .AddModelBoundView<HeadingAndDescription>("Styleguide-RouteFields")
                                .AddModelBoundView<HeadingOnly>("Styleguide-Section")
                                .AddModelBoundView<HeadingAndDescription>("Styleguide-SitecoreContext")
                                .AddModelBoundView<HeadingAndDescription>("Styleguide-Multilingual")
                                .AddModelBoundView<HeadingAndDescription>("Styleguide-Tracking");

        return renderingEngineOptions;
    }

    public static RenderingEngineOptions AddStarterKitViews(this RenderingEngineOptions renderingEngineOptions)
    {
        renderingEngineOptions.AddModelBoundView<Title>("Title")
                              .AddModelBoundView<Container>("Container")
                              .AddModelBoundView<ColumnSplitter>("ColumnSplitter")
                              .AddModelBoundView<RowSplitter>("RowSplitter")
                              .AddModelBoundView<PageContent>("PageContent")
                              .AddModelBoundView<RichText>("RichText")
                              .AddModelBoundView<Promo>("Promo")
                              .AddModelBoundView<LinkList>("LinkList")
                              .AddModelBoundView<Image>("Image")
                              .AddModelBoundView<PartialDesignDynamicPlaceholder>("PartialDesignDynamicPlaceholder")
                              .AddModelBoundView<Navigation>("Navigation");

        return renderingEngineOptions;
    }
}