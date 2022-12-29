using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using WMS.Service.WebAPI.AuthorizationPolicies;

namespace WMS.Service.WebAPI.Extensions
{
   public static class AuthenticationServiceCollectionExtensions
   {
      public static IServiceCollection AddAuthenticationWithAuthorizationSupport(this IServiceCollection services, IConfiguration config)
      {
         services.AddMicrosoftIdentityWebApiAuthentication(config, "AzureAdB2C");

         services.AddSingleton<IAuthorizationHandler, ScopesHandler>();

         return services;
      }
   }
}
