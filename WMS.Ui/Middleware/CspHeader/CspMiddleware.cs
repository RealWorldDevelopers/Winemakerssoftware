using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WMS.Ui.Middleware.CspHeader
{
    /// <summary>
    /// An ASP.NET middleware for adding CSP header.
    /// </summary>
    public sealed class CspMiddleware
    {
        private const string POLICY_HEADER = "Content-Security-Policy";
        private const string REPORT_URI_HEADER = "report-uri";
        private readonly RequestDelegate _next;
        private readonly ICspDirectives _directives;

        /// <summary>
        /// Instantiates a new <see cref="CspMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="directives">An instance of <see cref="ICspDirectives"/>.</param>        
        public CspMiddleware(RequestDelegate next, ICspDirectives directives)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _directives = directives ?? throw new ArgumentNullException(nameof(directives));
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

            context.Response.Headers.Add(POLICY_HEADER, GetHeaderValue());
            await _next(context);
        }

        /// <summary>
        /// Supportive Function to build Header Value
        /// </summary>
        private string GetHeaderValue()
        {
            var value = string.Empty;
            value += GetDirective(_directives.Default_Src.Header, _directives.Default_Src.Sources);
            value += GetDirective(_directives.Script_Src.Header, _directives.Script_Src.Sources);
            value += GetDirective(_directives.Style_Src.Header, _directives.Style_Src.Sources);
            value += GetDirective(_directives.Img_Src.Header, _directives.Img_Src.Sources);
            value += GetDirective(_directives.Font_Src.Header, _directives.Font_Src.Sources);
            value += GetDirective(_directives.Media_Src.Header, _directives.Media_Src.Sources);
            value += GetDirective(_directives.Object_Src.Header, _directives.Object_Src.Sources);
            value += GetDirective(_directives.Connect_Src.Header, _directives.Connect_Src.Sources);
            value += GetDirective(_directives.Frame_Ancestors.Header, _directives.Frame_Ancestors.Sources);
            if (!string.IsNullOrWhiteSpace(_directives.ReportUri))
                value += $"{REPORT_URI_HEADER} {_directives.ReportUri}; ";

            return value;
        }

        /// <summary>
        /// Supportive Function to compile a directive for Header Value
        /// </summary>
        /// <param name="directive">Directive Name as <see cref="string"/></param>
        /// <param name="sources">List of acceptable sources as <see cref="IList{string}"/></param>
        /// <returns>Compiled directive as a <see cref="string"/></returns>
        private string GetDirective(string directive, IList<string> sources)
            => sources.Count > 0 ? $"{directive} {string.Join(" ", sources)}; " : "";

    }

}
