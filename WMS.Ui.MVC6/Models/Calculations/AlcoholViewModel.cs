using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WMS.Ui.Mvc6.Models.Validation;

namespace WMS.Ui.Mvc6.Models.Calculations
{
   public class AlcoholViewModel
   {
      [Required]
      [Range(0, 32)] // Brix
      [RangeIf(.990, 1.130, "AlcoholCalculator_UseBrix", Comparison.IsEqualTo, false)]
      [DisplayName("Beginning Sugar")]
      public decimal? SugarStart { set; get; }

      [Required]
      [Range(0, 32)] // Brix
      [RangeIf(.990, 1.130, "AlcoholCalculator_UseBrix", Comparison.IsEqualTo, false)]
      [DisplayName("Ending Sugar")]
      public decimal? SugarEnd { set; get; }

      [DisplayName("ABV")]
      public decimal? Abv { set; get; }

   }

}
