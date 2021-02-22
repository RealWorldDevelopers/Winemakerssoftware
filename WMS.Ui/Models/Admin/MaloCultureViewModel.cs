using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
{
    public class MaloCultureViewModel
    {
        public MaloCultureViewModel()
        {
            Yeasts = new List<YeastViewModel>();
        }
        public List<YeastViewModel> Yeasts { get; }
    }

}
