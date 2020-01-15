using System.Collections.Generic;

namespace WMS.Ui.Models.Yeasts
{
    public class YeastGroupListItemViewModel
    {
        public YeastGroupListItemViewModel()
        {
            Yeasts = new List<YeastListItemViewModel>();
        }

        public int BrandId { get; set; }
        public string GroupName { get; set; }

        public List<YeastListItemViewModel> Yeasts { get; }
    }
}
