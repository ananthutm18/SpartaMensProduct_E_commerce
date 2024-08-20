using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpartaMensProduct.Middleware
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
            cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();
            cache.SetAllowResponseInBrowserHistory(false);
            base.OnResultExecuting(filterContext);
        }
    }
}