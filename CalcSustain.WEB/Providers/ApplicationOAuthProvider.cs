using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using CalcSustain.WEB.Models;

namespace CalcSustain.WEB.Providers
{
    /// <summary>
    /// провайдер авторизации, который обеспечивает связь с компонентами OWIN
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }
            _publicClientId = publicClientId;
        }

        /// <summary>
        /// используется для генерации токена
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //--------------   Если требуется получить токен:   -----------------------

            // 1. Клиент обращается к ресурсу /Token для получения токена
            
            // 2. Если в пришедшем запросе есть заголовок "grant_type" и он имеет значение "password", 
            // то система вызывает метод GrantResourceOwnerCredentials() у провайдера авторизации ApplicationOAuthProvider
            
            // 3. Провайдер авторизации обращается к классу ApplicationUserManager 
            // для валидации поступивших данных (логина и пароля) и по ним создает объект claims identity
            
            // 4. Если валидация прошла успешно, то провайдер авторизации создает аутентификационный тикет, 
			// который применяется для генерации токена

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "Имя пользователя или пароль указаны неправильно.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            //++
            // AuthenticationProperties properties = CreateProperties(user.UserName);
            //
            AuthenticationProperties properties = CreateProperties("userName", user.UserName);

            //
            //----------    Добавляем сведения о ролях текущего пользователя - только для целей отладки   >>> --------------------
            //
            string roles = "";
            bool is_user = await userManager.IsInRoleAsync(user.Id, "user");
            if (is_user)
            {
                roles = "user";                    
            }

            bool is_manager = await userManager.IsInRoleAsync(user.Id, "manager");
            if (is_manager)
            {
                if (!string.IsNullOrWhiteSpace(roles))
                    roles += ",";
                roles += "manager";
            }

            bool is_admin = await userManager.IsInRoleAsync(user.Id, "admin");
            if (is_admin)
            {
                if (!string.IsNullOrWhiteSpace(roles))
                    roles += ",";
                roles += "admin";
            }

            if (!string.IsNullOrWhiteSpace(roles))
            {
                properties.Dictionary.Add("userRoles", roles);
            }
            //
            //----------  <<<  Добавляем сведения о ролях текущего пользователя - только для целей отладки   --------------------
            //

            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Учетные данные владельца ресурса не содержат идентификатор клиента.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }


        /// <summary>
        /// Добавляем в словарь AuthenticationProperties новую пару userProperty - userValue
        /// </summary>
        /// <param name="userProperty"></param>
        /// <param name="userValue"></param>
        /// <returns></returns>
        public static AuthenticationProperties CreateProperties(string userProperty, string userValue)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { userProperty, userValue }
            };
            return new AuthenticationProperties(data);
        }
    }
}