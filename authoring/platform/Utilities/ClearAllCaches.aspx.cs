﻿using System;
using System.Web;

namespace XmCloudAuthoring.Utilities
{
    public partial class ClearAllCaches : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Sitecore.Caching.CacheManager.ClearAllCaches();
        }
    }
}
