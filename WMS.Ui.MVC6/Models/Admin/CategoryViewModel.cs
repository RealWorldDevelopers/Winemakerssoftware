﻿
namespace WMS.Ui.Mvc6.Models.Admin
{
   public class CategoryViewModel
   {
      public CategoryViewModel()
      {
         Varieties = new List<VarietyViewModel>();
      }

      public int? Id { get; set; }
      public string? Literal { get; set; }
      public bool Enabled { get; set; }
      public string? Description { get; set; }

      public List<VarietyViewModel> Varieties { get; }
   }

}
