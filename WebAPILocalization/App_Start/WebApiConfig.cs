using System.Web.Http;

namespace WebAPILocalization
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new LanguageMessageHandler());
        }
    }
}
