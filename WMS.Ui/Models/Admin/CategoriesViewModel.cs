using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
{
    public class CategoriesViewModel
    {
        public CategoriesViewModel()
        {
            Categories = new List<CategoryViewModel>();
        }

        public List<CategoryViewModel> Categories { get; }
    }

}
