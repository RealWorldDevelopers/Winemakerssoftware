using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace WMS.Ui.Models.MaloCulture
{
   public class MaloCulturesViewModel
   {
      public MaloCulturesViewModel()
      {
         MaloCulturesGroups = new List<MaloCultureGroupListItemViewModel>();
      }
      public List<MaloCultureGroupListItemViewModel> MaloCulturesGroups { get; }

  
   }
}
