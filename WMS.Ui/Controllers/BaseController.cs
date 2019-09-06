using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WMS.Ui.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TinifyAPI;
using Microsoft.Extensions.Options;

namespace WMS.Ui.Controllers
{
    /// <summary>
    /// Base Class for all Controllers in Project
    /// </summary>
    public class BaseController : Controller
    {

        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<ApplicationRole> _roleManager;
        protected readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Display a Success Alert Message on the view
        /// </summary>
        /// <param name="message">Message to User as <see cref="string"/></param>
        /// <param name="dismissable">States if the message removable from the view by the user as <see cref="bool"/></param>
        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        /// <summary>
        /// Display a Informational Alert Message on the view
        /// </summary>
        /// <param name="message">Message to User as <see cref="string"/></param>
        /// <param name="dismissable">States if the message removable from the view by the user as <see cref="bool"/></param>
        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        /// <summary>
        /// Display a Warning Alert Message on the view
        /// </summary>
        /// <param name="message">Message to User as <see cref="string"/></param>
        /// <param name="dismissable">States if the message removable from the view by the user as <see cref="bool"/></param>
        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        /// <summary>
        /// Display a Danger Alert Message on the view
        /// </summary>
        /// <param name="message">Message to User as <see cref="string"/></param>
        /// <param name="dismissable">States if the message removable from the view by the user as <see cref="bool"/></param>
        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        /// <summary>
        /// Add Alert Messages to the queue to be added on view rendering
        /// </summary>
        /// <param name="alertStyle">Formating of Alert Message as <see cref="AlertStyles"/></param>
        /// <param name="message">Message to User as <see cref="string"/></param>
        /// <param name="dismissable">States if the message removable from the view by the user as <see cref="bool"/></param>
        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey) ? (List<Alert>)TempData[Alert.TempDataKey] : new List<Alert>();
            alerts.Add(new Alert { AlertStyle = alertStyle, Message = message, Dismissable = dismissable });
            TempData[Alert.TempDataKey] = alerts;
        }


        /// <summary>
        /// Create a roles based access token with roles assigned by user
        /// </summary>
        /// <param name="user">User as <see cref="ApplicationUser"/></param>
        /// <param name="expireMinutes">Minutes Token will remain alive as <see cref="double?"/></param>
        /// <returns>JWT Token as <see cref="string"/></returns>
        protected async Task<string> CreateJwtTokenAsync(ApplicationUser user, double? expireMinutes = null)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
                return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userClaims);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            double expirationMinutes = expireMinutes ?? double.Parse(_configuration["JwtToken:ExpireMinutes"]);
            var token = new JwtSecurityToken
            (
                issuer: _configuration["JwtToken:Issuer"],
                audience: _configuration["JwtToken:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:Key"])), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


        /// <summary>
        /// Create a general access token with no roles assigned
        /// </summary>
        /// <param name="userName">Name of User as <see cref="string"/></param>
        /// <param name="expireMinutes">Minutes Token will remain alive as <see cref="double?"/></param>
        /// <returns>JWT Token as <see cref="string"/></returns>
        protected string CreateJwtToken(string userName, double? expireMinutes = null)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            double expirationMinutes = expireMinutes ?? double.Parse(_configuration["JwtToken:ExpireMinutes"]);
            var token = new JwtSecurityToken
            (
                issuer: _configuration["JwtToken:Issuer"],
                audience: _configuration["JwtToken:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:Key"])), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        /// <summary>
        /// Reduce the physical size and byte size of an image
        /// </summary>
        /// <param name="buffer">Image to reduce as <see cref="byte[]"/></param>
        /// <param name="maxWidth">Maximum Width of Image as <see cref="int?"/></param>
        /// <param name="maxHeight">Maximum Height of Image as <see cref="int?"/></param>
        /// <returns>Image as <see cref="byte[]</returns>
        public async Task<byte[]> ResizeImage(byte[] buffer, int? maxWidth, int? maxHeight)
        {
            try
            {
                // TinyPNG Developer API KEY  https://tinypng.com/developers              
                Tinify.Key = _configuration["ApplicationSettings:TinyPNG:ApiKey"]; 

                var source = await Tinify.FromBuffer(buffer);

                Source resized;
                if (maxWidth.HasValue && maxHeight.HasValue)
                    resized = source.Resize(new { method = "fit", width = maxWidth, height = maxHeight });
                else if (maxWidth.HasValue)
                    resized = source.Resize(new { method = "scale", width = maxWidth });
                else if (maxHeight.HasValue)
                    resized = source.Resize(new { method = "scale", height = maxHeight });
                else
                    return buffer;

                return await resized.ToBuffer();

            }
            catch (AccountException ex)
            {
                // Verify API key and account limit.
                var x = await Tinify.Validate();
                var compressionsThisMonth = Tinify.CompressionCount;
            }
            catch (ClientException ex)
            {
                // TODO Check your source image and request options.                
            }
            catch (ServerException ex)
            {
                // TODO  Temporary issue with the Tinify API.
            }
            catch (ConnectionException ex)
            {
                // TODO  A network connection error occurred.
            }
            catch (System.Exception ex)
            {
                // TODO  Something else went wrong, unrelated to the Tinify API.
            }

            // something went wrong, just return input value
            return buffer;
        }

    }
}
