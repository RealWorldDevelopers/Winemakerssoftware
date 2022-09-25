namespace WMS.Ui.Mvc6.Models.Contact
{
   public class Factory : IFactory
   {
      public ContactViewModel CreateContactModel()
      {
         return new ContactViewModel();
      }
   }
}
