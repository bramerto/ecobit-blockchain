using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace Ecobit_Blockchain_Frontend
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}