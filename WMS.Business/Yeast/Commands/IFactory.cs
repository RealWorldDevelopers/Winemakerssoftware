using WMS.Business.Shared;
using WMS.Business.Yeast.Dto;

namespace WMS.Business.Yeast.Commands
{
    public interface IFactory
    {
        ICommand<YeastPair> CreateYeastPairCommand();
        ICommand<Dto.Yeast> CreateYeastsCommand();
    }
}