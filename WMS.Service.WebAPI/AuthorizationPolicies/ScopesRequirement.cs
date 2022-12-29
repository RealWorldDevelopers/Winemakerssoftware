using Microsoft.AspNetCore.Authorization;

namespace WMS.Service.WebAPI.AuthorizationPolicies
{
   public class ScopesRequirement : IAuthorizationRequirement
   {
      public readonly string ScopeName;

      public ScopesRequirement(string scopeName)
      {
         ScopeName = scopeName;
      }
   }
}
