
namespace WMS.Ui.Middleware.SecurityHeaders.Constants
{
    /// <summary>
    /// Referrer-Policy related constants.
    /// </summary>
    public static class ReferrerPolicyConstants
    {
        /// <summary>
        /// The header value for Content-Security-Policy
        /// </summary>
        public static readonly string Header = "Referrer-Policy";

        /// <summary>
        /// The Referrer header will be omitted entirely. No referrer information is sent along with requests.
        /// </summary>
        public static readonly string No_Referrer = "no-referrer";

        /// <summary>
        /// This is the user agent's default behavior if no policy is specified. The URL is sent as a referrer when the protocol security level stays the same (HTTP→HTTP, HTTPS→HTTPS), but isn't sent to a less secure destination (HTTPS→HTTP).
        /// </summary>
        public static readonly string No_Referrer_When_Downgrade = "no-referrer-when-downgrade";

        /// <summary>
        /// Only send the origin of the document as the referrer in all cases. The document https://example.com/page.html will send the referrer https://example.com/.
        /// </summary>
        public static readonly string Origin = "origin";

        /// <summary>
        /// Send a full URL when performing a same-origin request, but only send the origin of the document for other cases.
        /// </summary>
        public static readonly string Origin_When_Cross_Origin = "origin-when-cross-origin";

        /// <summary>
        /// A referrer will be sent for same-site origins, but cross-origin requests will contain no referrer information.
        /// </summary>
        public static readonly string Same_Origin = "same-origin";

        /// <summary>
        /// Only send the origin of the document as the referrer when the protocol security level stays the same (HTTPS→HTTPS), but don't send it to a less secure destination (HTTPS→HTTP).
        /// </summary>
        public static readonly string Strict_Origin = "strict-origin";

        /// <summary>
        /// Send a full URL when performing a same-origin request, only send the origin when the protocol security level stays the same (HTTPS→HTTPS), and send no header to a less secure destination (HTTPS→HTTP).
        /// </summary>
        public static readonly string Strict_Origin_When_Cross_Origin = "strict-origin-when-cross-origin";

        /// <summary>
        /// Send a full URL when performing a same-origin or cross-origin request. This policy will leak origins and paths from TLS-protected resources to insecure origins. Carefully consider the impact of this setting.
        /// </summary>
        public static readonly string Unsafe_Url = "unsafe-url";
    }

}
