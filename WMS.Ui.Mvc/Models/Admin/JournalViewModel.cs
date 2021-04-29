
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WMS.Business.Common;

namespace WMS.Ui.Mvc.Models.Admin
{
   public class JournalViewModel
   {
      public JournalViewModel()
      {
         Entries = new List<JournalEntryViewModel>();
         Varieties = new List<SelectListItem>();
         VolumeUOMs = new List<SelectListItem>();
         Yeasts = new List<SelectListItem>();
         MaloCultures = new List<SelectListItem>();
      }

      public int? Id { get; set; }
      public bool Complete { get; set; }
      public List<JournalEntryViewModel> Entries { get; }
      public string Title { get; set; }
      public string Description { get; set; }
      public double? Volume { get; set; }
      public int? VolumeUOM { get; set; }
      public int? Vintage { get; set; }
      public int? RecipeId { get; set; }
      public VarietyViewModel Variety { get; set; }
      public YeastViewModel Yeast { get; set; }
      public int? MaloCultureId { get; set; }
      public TargetViewModel Target { get; set; }
      public string SubmittedBy { get; set; }

      public List<SelectListItem> Varieties { get; }
      public List<SelectListItem> VolumeUOMs { get; }
      public List<SelectListItem> Yeasts { get; }
      public List<SelectListItem> MaloCultures { get; }

   }

}
