using System.Collections.Generic;

namespace WMS.Ui.Middleware.SecurityHeaders
{
    /// <summary>
    /// Defines the policy for customizing security headers for a request.
    /// </summary>
    public class SecurityHeadersPolicy
    {
        public IDictionary<string, string> SetHeaders { get; } = new Dictionary<string, string>();

        public ISet<string> RemoveHeaders { get; } = new HashSet<string>();
        
    }

}
