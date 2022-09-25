
using Microsoft.AspNetCore.Html;

namespace WMS.Ui.Mvc6.Models
{
   public class Alert
   {
      public const string TempDataKey = "TempDataAlerts";
      public string? AlertStyle { get; set; }
      public HtmlString? Message { get; set; }
      public bool? Dismissable { get; set; }
   }

   public static class AlertStyles
   {
      public const string Success = "success";
      public const string Information = "info";
      public const string Warning = "warning";
      public const string Danger = "danger";
   }
}
