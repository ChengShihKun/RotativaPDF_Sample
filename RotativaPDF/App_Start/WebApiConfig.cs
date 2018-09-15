using System.Web.Http;
using System.Web.Routing;

namespace RotativaPDF.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            var route = RouteTable.Routes.MapHttpRoute(
                  name: "GenPDF",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
        
        }
    }
}