using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Sitecore;
using Sitecore.Abstractions;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Templates;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Services.GraphQL.Content.GraphTypes;
using Sitecore.Services.GraphQL.EdgeSchema.GraphTypes.FieldTypes;
using Sitecore.Services.GraphQL.Schemas;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FieldType = GraphQL.Types.FieldType;

namespace XmCloudAuthoring.Services.GraphQL.SchemaExtenders
{
    public class CustomFieldsSchemaExtender : SchemaExtender
    {
        private readonly GraphQLItemResolver _itemResolver;
        private CustomFieldsResolver _customFieldsResolver;

        internal CustomFieldsResolver CustomFieldsResolver
        {
            get => this._customFieldsResolver ?? (this._customFieldsResolver = new CustomFieldsResolver());
            set => this._customFieldsResolver = value;
        }

        public CustomFieldsSchemaExtender()
          : this(ServiceLocator.ServiceProvider.GetService<GraphQLItemResolver>())
        {
        }

        public CustomFieldsSchemaExtender(GraphQLItemResolver graphQLItemResolver)
        {
            Assert.ArgumentNotNull((object)graphQLItemResolver, nameof(graphQLItemResolver));
            this._itemResolver = graphQLItemResolver;
            this.ExtendTypes<Sitecore.Services.GraphQL.EdgeSchema.GraphTypes.ItemGraphType>((Action<Sitecore.Services.GraphQL.EdgeSchema.GraphTypes.ItemGraphType>)(type =>
            {
                this.ExtendField<ItemFieldInterfaceGraphType>((IComplexGraphType)type, (Action<FieldType>)(field => field.Resolver = (IFieldResolver)new FuncFieldResolver<Item, object>(new Func<ResolveFieldContext<Item>, object>(this.ResolveField))));
                this.ExtendField<NonNullGraphType<ListGraphType<ItemFieldInterfaceGraphType>>>((IComplexGraphType)type, (Action<FieldType>)(field => field.Resolver = (IFieldResolver)new FuncFieldResolver<Item, object>(new Func<ResolveFieldContext<Item>, object>(this.ResolveFields))));
            }));
        }

        private object ResolveFields(ResolveFieldContext<Item> context)
        {
            bool flag = context.GetArgument<bool>("ownFields");
            if (context.Source.Template == null)
                return (object)Enumerable.Empty<Field>();
            return !flag ? (object)this._itemResolver.ResolveFields(context.Source).Where<Field>((Func<Field, bool>)(field => this.IncludesField(field.Name) || !this.CustomFieldsResolver.IsStandardTemplateField(field))) : (object)((IEnumerable<TemplateFieldItem>)context.Source.Template.OwnFields).Select<TemplateFieldItem, Field>((Func<TemplateFieldItem, Field>)(f => this._itemResolver.ResolveField(context.Source, ((CustomItemBase)f).ID)));
        }

        private object ResolveField(ResolveFieldContext<Item> context)
        {
            string fieldName = context.GetArgument<string>("name");
            if (string.IsNullOrWhiteSpace(fieldName))
                return (object)null;
            Field field = this._itemResolver.ResolveField(context.Source, fieldName);
            return field == null || !this.IncludesField(field.Name) && this.CustomFieldsResolver.IsStandardTemplateField(field) ? (object)null : (object)field;
        }

        private bool IncludesField(string fieldName) => this.CustomFieldsResolver.StandardFieldConfigurations.Any<StandardFieldConfiguration>((Func<StandardFieldConfiguration, bool>)(f => f.Name == fieldName));
    }

    public class CustomFieldsResolver
    {
        private readonly BaseTemplateManager _templateManager;

        public CustomFieldsResolver()
          : this(ServiceLocator.ServiceProvider.GetService<BaseTemplateManager>())
        {
        }

        public CustomFieldsResolver(BaseTemplateManager templateManager)
        {
            this.StandardFieldConfigurations = (IList<StandardFieldConfiguration>)new List<StandardFieldConfiguration>();
            this._templateManager = templateManager;
        }

        internal virtual IList<StandardFieldConfiguration> StandardFieldConfigurations { get; }

        internal virtual bool IsStandardTemplateField(Field field)
        {
            Assert.ArgumentNotNull((object)field, nameof(field));
            Template template = this._templateManager.GetTemplate(TemplateIDs.StandardTemplate, field.Database);
            Assert.IsNotNull((object)template, "Standard Template missing");
            return template.ContainsField(field.ID);
        }

        internal void AddSupportedStandardField(XmlNode configNode)
        {
            StandardFieldConfiguration standardFieldConfiguration = CustomFieldsResolver.ParseField(configNode);
            if (this.StandardFieldConfigurations.Any<StandardFieldConfiguration>((Func<StandardFieldConfiguration, bool>)(c => c.Name.Equals(standardFieldConfiguration.Name, StringComparison.OrdinalIgnoreCase))))
                return;
            this.StandardFieldConfigurations.Add(standardFieldConfiguration);
        }

        internal static StandardFieldConfiguration ParseField(XmlNode configNode)
        {
            string attribute = XmlUtil.GetAttribute("name", configNode);
            return !string.IsNullOrWhiteSpace(attribute) ? new StandardFieldConfiguration()
            {
                Name = attribute
            } : throw new InvalidOperationException("The standard field configuration was missing its 'name' attribute: " + configNode.OuterXml);
        }
    }

    public class StandardFieldConfiguration
    {
        public string Name { get; set; }
    }

}