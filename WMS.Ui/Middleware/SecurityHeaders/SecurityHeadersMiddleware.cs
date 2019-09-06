using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using WMS.Ui.Middleware.SecurityHeaders.Constants;

namespace WMS.Ui.Middleware.SecurityHeaders
{
    /// <summary>
    /// An ASP.NET middleware for adding security headers.
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecurityHeadersPolicy _policy;     

        /// <summary>
        /// Instantiates a new <see cref="SecurityHeadersMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="policy">An instance of the <see cref="SecurityHeadersPolicy"/> which can be applied.</param>
        public SecurityHeadersMiddleware(RequestDelegate next, SecurityHeadersPolicy policy)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        /// <summary>
        /// Invoke the middleware
        /// </summary>
        /// <param name="context">The current context</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            IHeaderDictionary headers = context.Response.Headers;

            foreach (var headerValuePair in _policy.SetHeaders)
            {
                headers[headerValuePair.Key] = headerValuePair.Value;
            }

            foreach (var header in _policy.RemoveHeaders)
            {
                headers.Remove(header);
            }       

            await _next(context);
        }

    }

}
