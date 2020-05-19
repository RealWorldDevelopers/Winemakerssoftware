

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WMS.Ui.Models.Journal
{
   public class JournalViewModel
   {     
      public IEnumerable<BatchListItemViewModel> Batches { get; set; }      

   }

     
   public class BatchEntryViewModel
   {
      public BatchEntryViewModel()
      {
         Additions = new List<AdditionViewModel> {
            new AdditionViewModel(), new AdditionViewModel(), new AdditionViewModel(),
            new AdditionViewModel(), new AdditionViewModel(), new AdditionViewModel()};
      }

      public int Id { get; set; }
      public DateTime EntryDate { get; set; }
      public double Temp { get; set; }
      public int TempUOM { get; set; }
      public double TA { get; set; }
      public double pH { get; set; }
      public double Sugar { get; set; }
      public int SugarUOM { get; set; }
      public double SO2 { get; set; }
      public bool Racked { get; set; }
      public bool Filtered { get; set; }
      public bool Bottled { get; set; }
      public List<AdditionViewModel> Additions { get; }
      public string Note { get; set; }

      public IOrderedEnumerable<SelectListItem> SugarUOMs { get; set; }
      public IOrderedEnumerable<SelectListItem> TempUOMs { get; set; }
   }

   public class AdditionViewModel
   {
      public int Id { get; set; }
      public int IngredientId { get; set; }
      public double Volume { get; set; }
      public int UnitOfMeasure { get; set; }

      public IOrderedEnumerable<SelectListItem> UOMs { get; set; }
      public IOrderedEnumerable<SelectListItem> Ingredients { get; set; }
   }


}

