using WMS.Business.Image.Dto;
using WMS.Business.Common;

namespace WMS.Business.Image.Queries
{
    public interface IFactory
    {
        IQuery<Dto.ImageDto> CreateImageQuery();
    }
}