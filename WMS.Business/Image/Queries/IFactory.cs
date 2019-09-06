using WMS.Business.Image.Dto;
using WMS.Business.Shared;

namespace WMS.Business.Image.Queries
{
    public interface IFactory
    {
        IQuery<Dto.Image> CreateImageQuery();
    }
}