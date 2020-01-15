using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
{
    public class VarietiesViewModel
    {
        public VarietiesViewModel()
        {
            Varieties = new List<VarietyViewModel>();
        }
        public List<VarietyViewModel> Varieties { get; }
    }

}
