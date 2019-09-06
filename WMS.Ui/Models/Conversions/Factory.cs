namespace WMS.Ui.Models.Conversions
{
    public class Factory : IFactory
    {
        public ConversionsViewModel CreateConversionsModel()
        {
            return new ConversionsViewModel();
        }
    }
}
