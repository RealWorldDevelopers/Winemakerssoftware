namespace WMS.Ui.Mvc6.Models.Conversions
{
   public class Factory : IFactory
   {
      public ConversionsViewModel CreateConversionsModel()
      {
         return new ConversionsViewModel();
      }
   }
}
