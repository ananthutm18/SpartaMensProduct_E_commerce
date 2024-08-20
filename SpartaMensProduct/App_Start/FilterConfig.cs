using SpartaMensProduct.Middleware;
using System.Web;
using System.Web.Mvc;

namespace SpartaMensProduct
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NoCacheAttribute()); // Apply globally change this nif u wnat to apply action individually

        }
    }
}
