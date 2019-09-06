using System.Collections.Generic;

namespace WMS.Ui.Models.Yeasts
{
    public class YeastGroupListItemViewModel
    {
        public int BrandId { get; set; }
        public string GroupName { get; set; }
        public List<YeastListItemViewModel> Yeasts { get; set; }
    }
}
