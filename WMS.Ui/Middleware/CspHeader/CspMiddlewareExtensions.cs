using Microsoft.AspNetCore.Builder;
using System;

namespace WMS.Ui.Middleware.CspHeader
{
    /// <summary>
    /// The <see cref="IApplicationBuilder"/> extensions for adding CSP middleware support.
    /// </summary>
    public static class CspMiddlewareExtensions
    {
        /// <summary>
        /// Adds middleware to your web application pipeline to automatically add CSP headers to requests
        /// </summary>
        /// <param name="app">The IApplicationBuilder passed to your Configure method</param>
        /// <param name="builder">A configured policy builder to build a policy.</param>
        /// <returns>The original app parameter</returns>
        public static IApplicationBuilder UseCspMiddleware(this IApplicationBuilder app, Action<CspDirectivesBuilder> builder)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var newBuilder = new CspDirectivesBuilder();
            builder(newBuilder);

            var options = newBuilder.Build();
            return app.UseMiddleware<CspMiddleware>(options);

        }
    }

}
