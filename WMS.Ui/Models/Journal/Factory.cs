

namespace WMS.Ui.Models.Journal
{
    public class Factory : IFactory
    {
        public JournalViewModel CreateJournalModel()
        {
            return new JournalViewModel();
        }
    }
}
