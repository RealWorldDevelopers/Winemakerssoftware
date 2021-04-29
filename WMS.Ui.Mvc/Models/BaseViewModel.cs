namespace WMS.Ui.Mvc.Models
{
   public class BaseViewModel
   {
      /// <summary>
      /// Used to access the Hit Count Increment Web API call
      /// </summary>
      public string HitCounterJwt { get; set; }

      /// <summary>
      /// Used to access the Rating Web API call
      /// </summary>
      public string RatingJwt { get; set; }

      /// <summary>
      /// Used to access the Batch Web API calls
      /// </summary>
      public string BatchJwt { get; set; }

   }
}
