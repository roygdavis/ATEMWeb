using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ATEMBrokerWebApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public SixteenMedia.ATEM.Broker.Atem atem;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            atem = new SixteenMedia.ATEM.Broker.Atem();
            atem.Connect(ConfigurationManager.AppSettings["atemIP"]);
        }
    }
}
