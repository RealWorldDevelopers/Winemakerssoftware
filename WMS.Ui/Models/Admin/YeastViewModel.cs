
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
{
    public class YeastViewModel
    {
        public int Id { get; set; }
        public YeastBrandViewModel Brand { get; set; }
        public YeastStyleViewModel Style { get; set; }
        public string Trademark { get; set; }
        public int? TempMin { get; set; }
        public int? TempMax { get; set; }
        public double? Alcohol { get; set; }
        public string Note { get; set; }
        public YeastPairingViewModel Pairing { get; set; }
        public List<YeastPairingViewModel> Pairings { get; set; }
        public List<SelectListItem> Brands { get; set; }
        public List<SelectListItem> Styles { get; set; }

    }

}
