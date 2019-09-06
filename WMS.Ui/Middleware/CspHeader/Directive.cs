using System.Collections.Generic;

namespace WMS.Ui.Middleware.CspHeader
{
    public class Directive : IDirective
    {
        public string Header { get; set; }
        public IList<string> Sources { get; set; } = new List<string>();
    }

}
