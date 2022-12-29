using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace WMS.Service.WebAPI.AuthorizationPolicies
{
   public class ScopesHandler : AuthorizationHandler<ScopesRequirement>
   {
      protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopesRequirement requirement)
      {
         if (!context.User.Claims.Any(x => x.Type == ClaimConstants.Scope)
            && !context.User.Claims.Any(y => y.Type == ClaimConstants.Scp))
         {
            return Task.CompletedTask;
         }

         Claim scopeClaim = context?.User?.FindFirst(ClaimConstants.Scp);

         if (scopeClaim == null)
            scopeClaim = context?.User?.FindFirst(ClaimConstants.Scope);

         if (scopeClaim != null && scopeClaim.Value.Equals(requirement.ScopeName, StringComparison.InvariantCultureIgnoreCase))
         {
            // Success only when there is a specific claim presented in the access token:
            context.Succeed(requirement);
         }

         return Task.CompletedTask;
      }
   }
}
