using System;
using System.Collections.Generic;

namespace WMS.Ui.Models.Journal
{
   public class BatchEntryChartDataViewModel
   {
      public BatchEntryChartDataViewModel()
      {
         Times = new List<string>();
         Sugar = new List<Double>();
         Temp = new List<Double>();
      }
      public List<string> Times { get;  }
      public List<Double> Sugar { get; }
      public List<Double> Temp { get; }
   }
}
