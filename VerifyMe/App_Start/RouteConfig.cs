﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VerifyMe
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute
  (
      name: "Detail",
      url: "{controller}/{id:Guid}",
      defaults: new { action = "Details", id = UrlParameter.Optional },
      constraints: new { id = "[A-Z0-9]{8}-([A-Z0-9]{4}-){3}[A-Z0-9]{12}" }
  );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
