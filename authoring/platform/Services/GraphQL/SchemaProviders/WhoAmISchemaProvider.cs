using GraphQL.Types;
using Sitecore;
using Sitecore.Security.Accounts;
using Sitecore.Services.GraphQL.Schemas;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using XmCloudAuthoring.Services.GraphQL.Types;

namespace XmCloudAuthoring.Services.GraphQL.SchemaProviders
{
    public class WhoAmISchemaProvider : SchemaProviderBase
    {
        public override IEnumerable<FieldType> CreateRootQueries()
        {
            yield return new WhoAmIQuery();
        }

        protected class WhoAmIQuery : RootFieldType<UserGraphType, User>
        {
            public WhoAmIQuery() : base("whoAmI", "Gets the current User.")
            {
                Metadata["Category"] = "User";
            }

            protected override User Resolve(ResolveFieldContext context)
            {
                return Context.User ?? User.FromName("anonymous", false);
            }
        }
    }
}