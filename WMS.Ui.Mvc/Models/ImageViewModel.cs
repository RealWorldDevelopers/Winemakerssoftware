
using System;

namespace WMS.Ui.Mvc.Models
{
   public class ImageViewModel
   {
      public int Id { get; set; }
      public Uri Src { get; set; }
      public Uri SrcThumb { get; set; }
      public string Alt { get; set; }
      public string Title { get; set; }
      public string Caption { get; set; }

   }
}
