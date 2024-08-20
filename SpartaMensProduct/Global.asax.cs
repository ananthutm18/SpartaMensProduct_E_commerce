using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

using SpartaMensProduct.Middleware;


using System.Web.Routing;
using System.Web.Http;

namespace SpartaMensProduct
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteConfig.RegisterRoutes(RouteTable.Routes);



            var logFilePath = Server.MapPath("~/App_Data/log.txt");
            Logger.Initialize(logFilePath);

            Logger.LogInformation("Application started");
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            Logger.LogError("An unhandled exception occurred", exception);
        }

        protected void Application_End()
        {
            Logger.LogInformation("Application ended");
        }

    }
}
