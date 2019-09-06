using System.Collections.Generic;

namespace WMS.Ui.Middleware.CspHeader
{
    public interface IDirective
    {
        string Header { get; set; }
        IList<string> Sources { get; set; }
    }

}
