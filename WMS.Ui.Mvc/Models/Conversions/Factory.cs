namespace WMS.Ui.Mvc.Models.Conversions
{
   public class Factory : IFactory
   {
      public ConversionsViewModel CreateConversionsModel()
      {
         return new ConversionsViewModel();
      }
   }
}
