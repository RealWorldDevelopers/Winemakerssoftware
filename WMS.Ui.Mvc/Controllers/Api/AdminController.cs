
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WMS.Ui.Mvc.Controllers.Api
{
   /// <summary>
   /// API Controller for Ajax Admin calls
   /// </summary>
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   [Route("api/[controller]")]
   [ApiController]
   public class AdminController : ControllerBase
   {
      [HttpGet("test")]
      public string Get()
      {
         if (User.IsInRole("Admin"))
            return "Hello User" + User.FindFirst(ClaimTypes.NameIdentifier).Value;
         else
            return "Hello Admin" + User.FindFirst(ClaimTypes.NameIdentifier).Value;
      }

   }
}