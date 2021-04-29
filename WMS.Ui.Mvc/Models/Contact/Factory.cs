namespace WMS.Ui.Mvc.Models.Contact
{
   public class Factory : IFactory
   {
      public ContactViewModel CreateContactModel()
      {
         return new ContactViewModel();
      }
   }
}
