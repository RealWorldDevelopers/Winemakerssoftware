
using WMS.Domain;

namespace WMS.Ui.Mvc6.Models.MaloCulture
{
    public interface IFactory
   {
      MaloCultureListItemViewModel CreateMaloCultureListItemViewModel(Domain.MaloCulture dto);
      MaloCulturesViewModel CreateMaloCultureModel(List<ICode> dtoCategoryList, List<ICode> dtoVarietyList, List<Domain.MaloCulture> MaloCultures);
   }
}