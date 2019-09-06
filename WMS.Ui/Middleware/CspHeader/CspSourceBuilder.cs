using System.Collections.Generic;

namespace WMS.Ui.Middleware.CspHeader
{
    /// <summary>
    /// Build the source list for a directive.
    /// </summary>
    public sealed class CspSourceBuilder 
    {
        internal CspSourceBuilder() { }

        internal IList<string> Sources { get; set; } = new List<string>();

        /// <summary>
        /// Add 'self' as a source to the directive
        /// </summary>
        public CspSourceBuilder AllowSelf() => Allow("'self'");

        /// <summary>
        /// Add 'none' as a source to the directive
        /// </summary>
        public CspSourceBuilder AllowNone() => Allow("'none'");

        /// <summary>
        /// Add * as a source to the directive
        /// </summary>
        public CspSourceBuilder AllowAny() => Allow("*");

        /// <summary>
        /// Add data: as a source to the directive
        /// </summary>
        public CspSourceBuilder AllowData() => Allow("data:");

        /// <summary>
        /// Add 'unsafe-inline' as a source to the directive
        /// </summary>
        public CspSourceBuilder AllowUnsafeInline() => Allow("'unsafe-inline'");

        /// <summary>
        /// Add 'unsafe-eval' as a source to the directive
        /// </summary>
        public CspSourceBuilder AllowUnsafeEval() => Allow("'unsafe-eval'");

        /// <summary>
        /// Add 'nonce-key' as a source to the directive
        /// </summary>
        /// <param name="key">key value for the nonce source</param>
        public CspSourceBuilder AllowNonce(string key) => Allow($"'nonce-{key}'");

        /// <summary>
        /// Ad-hoc source value to the directive
        /// </summary>
        /// <param name="source">Source Value</param>
        public CspSourceBuilder Allow(string source)
        {
            Sources.Add(source);
            return this;
        }
    }

}
