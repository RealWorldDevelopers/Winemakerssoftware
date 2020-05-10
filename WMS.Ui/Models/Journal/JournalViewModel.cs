

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WMS.Ui.Models.Journal
{
   public class JournalViewModel
   {
      public JournalViewModel()
      {
         Batches = new List<BatchViewModel> {
            new BatchViewModel(),new BatchViewModel(), new BatchViewModel(),
            new BatchViewModel(), new BatchViewModel(), new BatchViewModel() };
      }

      public List<BatchViewModel> Batches { get; }

   }


   public class BatchViewModel
   {
      public BatchViewModel()
      {
         Entries = new List<BatchEntryViewModel> {
            new BatchEntryViewModel(), new BatchEntryViewModel(), new BatchEntryViewModel(),
            new BatchEntryViewModel(), new BatchEntryViewModel(), new BatchEntryViewModel()};
      }

      public int Id { get; set; }

      public int Vintage { get; set; } 

      public int Variety { get; set; } 

      public int RecipeId { get; set; }

      public List<BatchEntryViewModel> Entries { get; }

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

