using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WMS.Ui.Mvc6.Models;
using WMS.Ui.Mvc6.Models.Account;

namespace WMS.Ui.Mvc6.Controllers.Api
{
   /// <summary>
   /// Generate Public Private Tokens for Secure Web Api Calls
   /// </summary>    
   [Route("api/[controller]")]
   [ApiController]
   public class TokenController : ControllerBase
   {
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly SignInManager<ApplicationUser> _signInManager;
      private readonly IConfiguration _configuration;

      public TokenController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
      {
         _userManager = userManager;
         _signInManager = signInManager;
         _configuration = configuration;
      }


      // GET: api/token
      [HttpPost]
      public async Task<IActionResult> Get(LoginViewModel model)
      {
         if (ModelState.IsValid)
         {
            var user = await _userManager.FindByNameAsync(model?.UserName).ConfigureAwait(false);
            if (user != null)
            {

               var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false).ConfigureAwait(false);

               if (!result.Succeeded)
               {
                  return Unauthorized();
               }

               var claims = new[]
               {
                        new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

               double expirationMinutes = double.Parse(_configuration["JwtToken:ExpireMinutes"], CultureInfo.CurrentCulture);
               var token = new JwtSecurityToken
               (
                   issuer: _configuration["JwtToken:Issuer"],
                   audience: _configuration["JwtToken:Audience"],
                   claims: claims,
                   expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                   notBefore: DateTime.UtcNow,
                   signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:Key"])),
                           SecurityAlgorithms.HmacSha256)
               );

               return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
         }

         return BadRequest();
      }

   }
}