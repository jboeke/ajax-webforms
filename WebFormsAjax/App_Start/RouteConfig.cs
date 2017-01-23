using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace WebFormsAjax
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            // HINT: http://stackoverflow.com/questions/23033614/asp-net-calling-webmethod-with-jquery-ajax-401-unauthorized
            // settings.AutoRedirectMode = RedirectMode.Permanent;
            settings.AutoRedirectMode = RedirectMode.Off;
            routes.EnableFriendlyUrls(settings);
        }
    }
}
