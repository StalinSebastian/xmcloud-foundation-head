using GraphQL.Types;
using Sitecore;
using Sitecore.Data.Items;

namespace XmCloudAuthoring.Services.GraphQL.Types
{
    public class ItemInterfaceGraphType : InterfaceGraphType<Item>
    {
        public ItemInterfaceGraphType()
        {
            Name = "SitecoreItem";
            Description = "Represents a Sitecore content item";

            Field<NonNullGraphType<StringGraphType>>(
                "id",
                description: "The unique ID of the item",
                resolve: context => context.Source.ID.ToString()
            );

            Field<NonNullGraphType<StringGraphType>>(
                "name",
                description: "The name of the item",
                resolve: context => context.Source.Name
            );

            Field<StringGraphType>(
                "path",
                description: "The full path to the item",
                resolve: context => context.Source.Paths.FullPath
            );

            Field<StringGraphType>(
                "templateName",
                description: "The name of the item's template",
                resolve: context => context.Source.TemplateName
            );

            Field<NonNullGraphType<StringGraphType>>(
                "lockOwner",
                description: "The user who currently owns the lock",
                resolve: context => context.Source.Locking.GetOwner()
            );

            Field<DateTimeGraphType>(
                "lockDate",
                description: "When the item was locked",
                resolve: context => context.Source.Statistics.Updated
            );
            Field<NonNullGraphType<BooleanGraphType>>(
                "isLocked",
                description: "Indicates if the item is locked",
                resolve: context => context.Source.Locking.IsLocked()
            );
            Field<NonNullGraphType<BooleanGraphType>>(
                "isLockedByCurrentUser",
                description: "Indicates if the item is locked by the current user",
                resolve: context => context.Source.Locking.HasLock() && context.Source.Locking.GetOwner() == Context.User.Name
            );
        }
    }
}