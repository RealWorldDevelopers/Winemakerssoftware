using WMS.Domain;

namespace WMS.Communications
{
    public interface IImageAgent
    {
        Task<ImageFile> AddImage(ImageFile image);
        Task<bool> DeleteImage(int id);
        Task<ImageFile> GetImage(int id);
        Task<IEnumerable<ImageFile>> GetImages();
        Task UpdateImage(ImageFile Image);
    }
}