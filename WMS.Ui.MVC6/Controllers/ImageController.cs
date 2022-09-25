
using Microsoft.AspNetCore.Mvc;
using WMS.Communications;

namespace WMS.Ui.Mvc6.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageAgent _imageAgent;

        public ImageController(IImageAgent imageAgent)
        {
            _imageAgent = imageAgent;
        }

        [HttpGet]
        public async Task<FileStreamResult?> ViewImage(int id)
        {
            var dto = await _imageAgent.GetImage(id).ConfigureAwait(false);
            var data = dto.Data;
            if (data == null)
                return null;

            MemoryStream ms = new MemoryStream(data);
            return new FileStreamResult(ms, dto.ContentType ?? String.Empty);
        }

        [HttpGet]
        public async Task<FileStreamResult?> ViewThumbnail(int id)
        {
            var dto = await _imageAgent.GetImage(id).ConfigureAwait(false);
            var thumb = dto.Thumbnail;
            if (thumb == null)
                return null;
            MemoryStream ms = new MemoryStream(thumb);
            return new FileStreamResult(ms, dto.ContentType ?? String.Empty);
        }

    }
}