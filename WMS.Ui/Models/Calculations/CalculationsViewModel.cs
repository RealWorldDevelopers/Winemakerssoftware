﻿

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WMS.Ui.Models.Calculations
{
   public class CalculatorViewModel
   {
      public string GroupName { get; set; }
   }

   public class ChaptalizationViewModel
   { 
      [Required]
      [Range(0, 999)]
      [DisplayName("Must Volume")]
      public decimal? Volume { set; get; }

      [Required]
      [Range(-5, 35)]
      [DisplayName("Current Value")]
      public decimal? CurrentReading { set; get; }

      [Required]
      [Range(-5, 35)]
      [DisplayName("Goal")]
      public decimal? Goal { set; get; }
            
      [DisplayName("Sugar")]
      public decimal? Sugar { set; get; }
   }

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

   public class CalculationsViewModel
   {
      public CalculationsViewModel()
      {
         CalculatorGroups = new List<CalculatorViewModel>();
      }

      public List<CalculatorViewModel> CalculatorGroups { get; }

      public ChaptalizationViewModel ChaptalizationCalculator { get; set; }

      public AlcoholViewModel AlcoholCalculator { get; set; }
   }

}
