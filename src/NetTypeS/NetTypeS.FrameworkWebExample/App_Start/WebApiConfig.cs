using System.Web.Http;

namespace NetTypeS.FrameworkWebExample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}");
        }
    }
}
