using WMS.Business.Common;
using WMS.Business.Yeast.Dto;

namespace WMS.Business.Yeast.Commands
{
    public interface IFactory
    {
        ICommand<YeastPairDto> CreateYeastPairCommand();
        ICommand<Dto.YeastDto> CreateYeastsCommand();
    }
}