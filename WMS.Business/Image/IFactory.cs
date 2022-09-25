using WMS.Business.Common;
using WMS.Business.Image.Dto;

namespace WMS.Business.Image
{
    public interface IFactory
    {
        /// <summary>
        /// Create a <see cref="ImageDto"/> Query Object
        /// </summary>
        /// <returns><see cref="IQuery{ImageDto}"/></returns>
        IQuery<ImageDto> CreateImagesQuery();

        /// <summary>
        /// Create a <see cref="ImageDto"/> Command Object
        /// </summary>
        /// <returns><see cref="ICommand{ImageDto}"/></returns>
        ICommand<ImageDto> CreateImagesCommand();
    }
}