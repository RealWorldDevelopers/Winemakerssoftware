﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Mvc6.Models.Calculations
{
   public class FortifyViewModel
   {
      [Required]
      [Range(1, 999)]
      [DisplayName("Volume")]
      public decimal? VolumeWine { set; get; }

      [Required]
      [Range(1, 99)]
      [DisplayName("Spirit Alcohol")]
      public decimal? SpiritReading { set; get; }

      [Required]
      [Range(1, 30)]
      [DisplayName("Alcohol Wine")]
      public decimal? InitialAlcohol { set; get; }

      [Required]
      [Range(1, 99)]
      [DisplayName("Goal Alcohol")]
      public decimal? GoalAlcohol { set; get; }

      [DisplayName("Volume of Spirit")]
      public decimal? Spirit { set; get; }
   }

}
