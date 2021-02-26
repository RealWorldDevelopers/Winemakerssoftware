using System.Collections.Generic;

namespace WMS.Ui.Models.Admin
{
   public class MaloCulturesViewModel
   {
      public MaloCulturesViewModel()
      {
         Cultures = new List<MaloCultureViewModel>();
      }
      public List<MaloCultureViewModel> Cultures { get; }
   }

}
