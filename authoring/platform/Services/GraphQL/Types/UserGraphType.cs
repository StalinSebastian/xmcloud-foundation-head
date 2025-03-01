using GraphQL.Types;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XmCloudAuthoring.Services.GraphQL.Types
{
    public class UserGraphType : ObjectGraphType<User>
    {
        public UserGraphType()
        {
            Name = "SitecorePrinciple";
            Description = "Represents a Sitecore user or principal";

            // User properties
            Field<NonNullGraphType<StringGraphType>>(
                "name",
                description: "The username of the current user",
                resolve: context => context.Source.Name
            );

            Field<NonNullGraphType<StringGraphType>>(
                "fullName",
                description: "The full name of the user, falls back to username if not set",
                resolve: context => string.IsNullOrWhiteSpace(context.Source.Profile.FullName)
                    ? context.Source.Name
                    : context.Source.Profile.FullName
            );

            Field<NonNullGraphType<StringGraphType>>(
                "icon",
                description: "URL to the user's profile icon",
                resolve: context =>
                {
                    var baseUrl = HttpContext.Current?.Request.Url.GetLeftPart(UriPartial.Authority) ?? string.Empty;
                    return $"{baseUrl}/-/icon/{context.Source.Profile.Portrait}";
                }
            );

            Field<NonNullGraphType<BooleanGraphType>>(
                "isAuthenticated",
                description: "Indicates if the user is authenticated",
                resolve: context => context.Source.IsAuthenticated
            );

            Field<NonNullGraphType<BooleanGraphType>>(
                "isAdministrator",
                description: "Indicates if the user has administrator privileges",
                resolve: context => context.Source.IsAdministrator
            );

            // note that graph types can resolve other graph types; for example
            // it would be possible to add a `lockedItems` field here that would
            // resolve to an `Item[]` and map it onto `ListGraphType<ItemInterfaceGraphType>`

            Field<ListGraphType<ItemInterfaceGraphType>>(
                "lockedItems",
                description: "Items locked by the user",
                resolve: context => GetLockedItems(context.Source)
            );
        }
        private IEnumerable<Item> GetLockedItems(User user)
        {
            if (user == null || !user.IsAuthenticated)
                return Enumerable.Empty<Item>();

            // Using Sitecore's Lock API to get items locked by the user
            var masterDb = Sitecore.Data.Database.GetDatabase("master");
            if (masterDb == null)
                return Enumerable.Empty<Item>();

            // Query items where the lock owner matches the username
            return masterDb.SelectItems($"fast://*[@__lock='%{user.Name}%']")
                ?.Where(item => item.Locking.IsLocked() && item.Locking.GetOwner() == user.Name)
                ?? Enumerable.Empty<Item>();
        }

    }
}