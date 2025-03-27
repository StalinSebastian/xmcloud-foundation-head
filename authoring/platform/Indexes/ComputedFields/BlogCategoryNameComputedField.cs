using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing.Text;

namespace XmCloudAuthoring.Indexes.ComputedFields
{
    public class BlogCategoryNamesComputedField : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            const string fieldName = "BlogCategories"; //source catagories field from BlogPost item

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

                var categoryGuids = item.Fields[fieldName].Value.Split('|').Where(ID.IsID).ToList();
                List<string> categoryNames = new List<string>();

                foreach (var categoryGuid in categoryGuids)
                {
                    
                    var categoryItem = masterDb.GetItem(ID.Parse(categoryGuid));
                    if (categoryItem != null)
                    {
                        categoryNames.Add(categoryItem["BlogCategoryName"] ?? "None");
                        // Assuming "CategoryName" is the field where category names are stored
                    }
                }

                //return categoryNames.Any() ? string.Join(",", categoryNames) : null;
                return categoryNames.Any() ? categoryNames : null;
            }
            catch (Exception ex)
            {
                Log.Error($"[Indexing] Error computing blogCategoryNames for item {indexable?.UniqueId}: {ex.Message}", ex, this);
                return null;
            }
        }
    }
}