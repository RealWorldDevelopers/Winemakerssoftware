using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Mvc.Models.Calculations
{
   public class DoseSO2ViewModel
   {
      [Range(0, 999)]
      [DisplayName("Volume")]
      public decimal? MustVolumeSO2 { set; get; }

      [Range(2.5, 4.5)]
      [DisplayName("pH")]
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
      public decimal? pH { set; get; }

      [Required]
      [Range(0, 80)]
      [DisplayName("Current SO2")]
      public decimal? CurrentSO2Reading { set; get; }

      [Required]
      [Range(1, 50)]
      [DisplayName("Goal SO2")]
      public decimal? GoalSO2 { set; get; }

      [DisplayName("Dose Rate")]
      public decimal? DoseRateSO2 { set; get; }

      [DisplayName("Dose Need")]
      public decimal? DoseAmount { set; get; }
   }

}
