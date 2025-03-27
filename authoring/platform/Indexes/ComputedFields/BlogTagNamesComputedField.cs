using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace XmCloudAuthoring.Indexes.ComputedFields
{
    public class BlogTagNamesComputedField : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            const string fieldName = "BlogTags"; ; //source tags field from BlogPost item
            try
            {
                Item item = (indexable as SitecoreIndexableItem)?.Item;
                if (item == null || item.Fields[fieldName] == null || !item.Fields[fieldName].HasValue)
                    return null;

                Database masterDb = Sitecore.Data.Database.GetDatabase("master"); // Explicitly use master database
                if (masterDb == null)
                {
                    Log.Error("[Indexing] Master database not found!", this);
                    return null;
                }

                var tagGuids = item.Fields[fieldName].Value.Split('|').Where(ID.IsID).ToList();
                List<string> tagNames = new List<string>();

                foreach (var tagGuid in tagGuids)
                {
                    var tagItem = masterDb.GetItem(ID.Parse(tagGuid));
                    if (tagItem != null)
                    {
                        tagNames.Add(tagItem["BlogTagName"] ?? "None");
                        // Assuming "blogTagName" is the field where tag names are stored
                    }
                }

                //return tagNames.Any() ? string.Join(",", tagNames) : null;
                return tagNames.Any() ? tagNames : null;
            }
            catch (Exception ex)
            {
                Log.Error($"[Indexing] Error computing blogTagNames for item {indexable?.UniqueId}: {ex.Message}", ex, this);
                return null;
            }
        }
    }
}