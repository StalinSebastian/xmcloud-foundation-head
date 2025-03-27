using GraphQL;
using GraphQL.Types;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.Services.GraphQL.Schemas;

namespace XmCloudAuthoring.Services.GraphQL.SchemaExtenders
{
    public class WhoAmISchemaExtender : SchemaExtender
    {
        public WhoAmISchemaExtender()
        {
            ExtendType<IComplexGraphType>("SitecorePrinciple", type =>
            {
                ExtendField(type, "isAdministrator", field =>
                {
                    field.Description = "Indicates if current user has administrator privileges";
                });
            });

            //Adds New Fields
            ExtendTypes<ObjectGraphType<User>>(type =>
            {
                type.Field<NonNullGraphType<BooleanGraphType>>(
                        "isAnonymous",
                        description: "Indicates if the user is Anonymous",
                        resolve: context => !context.Source.IsAuthenticated
                    );

                type.Fields.RemoveWhere(f => f.Name == "isAuthenticated");
            });

        }
    }
}