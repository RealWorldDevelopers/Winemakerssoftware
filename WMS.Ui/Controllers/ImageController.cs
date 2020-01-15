
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WMS.Ui.Controllers
{
    public class ImageController : Controller
    {
        private readonly Business.Image.Queries.IFactory _imageQueryFactory;

        public ImageController(Business.Image.Queries.IFactory imageQueryFactory)
        {
            _imageQueryFactory = imageQueryFactory;
        }

        [HttpGet]
        public async Task<FileStreamResult> ViewImage(int id)
        {
            var imageQry = _imageQueryFactory.CreateImageQuery();
            var dto = await imageQry.ExecuteAsync(id).ConfigureAwait(false);
            var data = dto.Data();
            MemoryStream ms = new MemoryStream(data);
            return new FileStreamResult(ms, dto.ContentType);
        }

        [HttpGet]
        public async Task<FileStreamResult> ViewThumbnail(int id)
        {
            var imageQry = _imageQueryFactory.CreateImageQuery();
            var dto = await imageQry.ExecuteAsync(id).ConfigureAwait(false);
            var thumb = dto.Thumbnail();
            MemoryStream ms = new MemoryStream(thumb);
            return new FileStreamResult(ms, dto.ContentType);
        }
    }
}