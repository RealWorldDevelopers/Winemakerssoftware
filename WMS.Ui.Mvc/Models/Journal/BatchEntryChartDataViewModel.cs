using System;
using System.Collections.Generic;

namespace WMS.Ui.Mvc.Models.Journal
{
   public class BatchEntryChartDataViewModel
   {
      public BatchEntryChartDataViewModel()
      {
         Times = new List<string>();
         Sugar = new List<double>();
         Temp = new List<double>();
      }
      public List<string> Times { get; }
      public List<double> Sugar { get; }
      public List<double> Temp { get; }
   }
}
