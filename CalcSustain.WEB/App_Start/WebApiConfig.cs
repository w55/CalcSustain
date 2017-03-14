using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace CalcSustain.WEB
{
    public static class WebApiConfig
    {
        /// <summary>
        /// В методе происходит настройка аутентификации Web API, чтобы она использовала только аутентификацию токенов
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы Web API
            // Настройка Web API для использования только проверки подлинности посредством маркера-носителя.

            // указывает Web API игнорировать любую аутентификацию, которая происходит до того, как обработка запроса достигнет конвейера Web API
            config.SuppressDefaultHostAuthentication();

            // Класс HostAuthenticationFilter подключает аутентификацию токенов
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Маршруты Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
