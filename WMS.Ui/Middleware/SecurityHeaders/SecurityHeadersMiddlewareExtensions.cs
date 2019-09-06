using Microsoft.AspNetCore.Builder;
using System;

namespace WMS.Ui.Middleware.SecurityHeaders
{
    /// <summary>
    /// The <see cref="IApplicationBuilder"/> extensions for adding Security headers middleware support.
    /// </summary>
    public static class SecurityHeadersMiddlewareExtensions
    {
        /// <summary>
        /// Adds middleware to your web application pipeline to automatically add security headers to requests
        /// </summary>
        /// <param name="app">The IApplicationBuilder passed to your Configure method.</param>
        /// <param name="builder">A configured policy builder to build a policy.</param>
        /// <returns>The original app parameter</returns>
        public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app, SecurityHeadersBuilder builder)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            SecurityHeadersPolicy policy = builder.Build();
            return app.UseMiddleware<SecurityHeadersMiddleware>(policy);
        }

    }

}
