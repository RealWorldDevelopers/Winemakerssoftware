using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Literal { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
        public List<VarietyViewModel> Varieties { get; set; }
    }

}
