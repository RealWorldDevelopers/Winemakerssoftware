
namespace WMS.Ui.Middleware.SecurityHeaders.Constants
{
    /// <summary>
    /// Strict-Transport-Security-related constants.
    /// </summary>
    public static class StrictTransportSecurityConstants
    {
        /// <summary>
        /// The number of seconds in one year
        /// </summary>
        public const int OneYearInSeconds = 60 * 60 * 24 * 365;

        /// <summary>
        /// Header value for Strict-Transport-Security
        /// </summary>
        public static readonly string Header = "Strict-Transport-Security";

        /// <summary>
        /// Tells the user-agent to cache the domain in the STS list for the provided number of seconds {0} 
        /// </summary>
        public static readonly string MaxAge = $"max-age={OneYearInSeconds}";

        /// <summary>
        /// Tells the user-agent to cache the domain in the STS list for the provided number of seconds {0} and include any sub-domains.
        /// </summary>
        public static readonly string MaxAgeIncludeSubdomains = $"max-age={OneYearInSeconds}; includeSubDomains";

        /// <summary>
        /// Tells the user-agent to remove, or not cache the host in the STS cache.
        /// </summary>
        public static readonly string NoCache = "max-age=0";

    }


}
