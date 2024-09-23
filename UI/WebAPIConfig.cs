using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Acurus.Capella.UI
{
    public static class WebAPIConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // Optional: Makes JSON response camelCase

            // Remove XML formatter to always return JSON
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}