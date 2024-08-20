using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpartaMensProduct.Middleware
{

    public class SessionAdminAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly bool _requireAdmin;

        public SessionAdminAuthorizeAttribute(bool requireAdmin = false)
        {
            _requireAdmin = requireAdmin;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = HttpContext.Current.Session["UserId"];
            var isAdmin = HttpContext.Current.Session["IsAdmin"];

            if (userId == null)
            {
                // If not logged in, redirect to the login page
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "User", action = "Login" })
                );
            }
            else if (_requireAdmin && (isAdmin == null || !(bool)isAdmin))
            {
                // If user is not an admin, redirect to an access denied page
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "AccessDenied" })
                );
            }

            base.OnActionExecuting(filterContext);
        }
    }
}