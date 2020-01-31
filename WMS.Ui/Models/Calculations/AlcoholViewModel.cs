using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   public class AlcoholViewModel
   {
      [Required]
      [Range(0, 999)]
      [DisplayName("Beginning Sugar")]
      public decimal? SugarStart { set; get; }

      [Required]
      [Range(-5, 35)]
      [DisplayName("Ending Sugar")]
      public decimal? SugarEnd { set; get; }

      [DisplayName("Alcohol By Volume")]
      public decimal? Abv { set; get; }
   }

}
