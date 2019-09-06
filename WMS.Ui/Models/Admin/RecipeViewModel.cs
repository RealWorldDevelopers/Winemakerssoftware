
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
{
    public class RecipeViewModel
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public bool NeedsApproved { get; set; }
        public int Hits { get; set; }
        public double? Rating { get; set; }
        public string Title { get; set; }
        public string SubmittedBy { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string Description { get; set; }
        public CategoryViewModel Category { get; set; }
        public VarietyViewModel Variety { get; set; }       
        public List<ImageViewModel> Images { get; set; }
        public List<SelectListItem> Varieties { get; set; }
    }

}
