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
using Microsoft.ApplicationInsights;
using System.Globalization;
using Microsoft.AspNetCore.Html;

namespace WMS.Ui.Controllers
{
    /// <summary>
    /// Base Class for all Controllers in Project
    /// </summary>
    public class BaseController : Controller
    {
        protected IConfiguration ConfigurationAgent { get; set; }
        protected RoleManager<ApplicationRole> RoleManagerAgent { get; set; }
        protected UserManager<ApplicationUser> UserManagerAgent { get; set; }
        protected TelemetryClient LoggingAgent { get; set; }

        public BaseController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, TelemetryClient telemetry)
        {
            UserManagerAgent = userManager;
            RoleManagerAgent = roleManager;
            ConfigurationAgent = configuration;
            LoggingAgent = telemetry;
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
            alerts.Add(new Alert { AlertStyle = alertStyle, Message = new HtmlString(message), Dismissable = dismissable });
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
            if (string.IsNullOrWhiteSpace(user?.UserName))
                return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userClaims = await UserManagerAgent.GetClaimsAsync(user).ConfigureAwait(false);
            var userRoles = await UserManagerAgent.GetRolesAsync(user).ConfigureAwait(false);
            claims.AddRange(userClaims);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await RoleManagerAgent.FindByNameAsync(userRole).ConfigureAwait(false);
                if (role != null)
                {
                    var roleClaims = await RoleManagerAgent.GetClaimsAsync(role).ConfigureAwait(false);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            double expirationMinutes = expireMinutes ?? double.Parse(ConfigurationAgent["JwtToken:ExpireMinutes"], CultureInfo.CurrentCulture);
            var token = new JwtSecurityToken
            (
                issuer: ConfigurationAgent["JwtToken:Issuer"],
                audience: ConfigurationAgent["JwtToken:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationAgent["JwtToken:Key"])), SecurityAlgorithms.HmacSha256)
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

            double expirationMinutes = expireMinutes ?? double.Parse(ConfigurationAgent["JwtToken:ExpireMinutes"], CultureInfo.CurrentCulture);
            var token = new JwtSecurityToken
            (
                issuer: ConfigurationAgent["JwtToken:Issuer"],
                audience: ConfigurationAgent["JwtToken:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationAgent["JwtToken:Key"])), SecurityAlgorithms.HmacSha256)
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
                Tinify.Key = ConfigurationAgent["ApplicationSettings:TinyPNG:ApiKey"];

                var source = await Tinify.FromBuffer(buffer).ConfigureAwait(false);

                Source resized;
                if (maxWidth.HasValue && maxHeight.HasValue)
                    resized = source.Resize(new { method = "fit", width = maxWidth, height = maxHeight });
                else if (maxWidth.HasValue)
                    resized = source.Resize(new { method = "scale", width = maxWidth });
                else if (maxHeight.HasValue)
                    resized = source.Resize(new { method = "scale", height = maxHeight });
                else
                    return buffer;

                return await resized.ToBuffer().ConfigureAwait(false);

            }
            catch (AccountException accountEx)
            {
                // Verify API key and account limit.
                var x = await Tinify.Validate().ConfigureAwait(false);
                var compressionsThisMonth = Tinify.CompressionCount;

                var properties = new Dictionary<string, string> {
                    { "compressionsThisMonth", compressionsThisMonth.HasValue ? compressionsThisMonth.Value.ToString(CultureInfo.CurrentCulture) : "0" } };
                LoggingAgent.TrackException(accountEx, properties);
            }

            catch (TinifyAPI.Exception tex)
            {
                LoggingAgent.TrackException(tex);
            }
            catch (System.Exception ex)
            {
                LoggingAgent.TrackException(ex);
                throw;
            }

            // something went wrong, just return input value
            return buffer;
        }

    }
}
