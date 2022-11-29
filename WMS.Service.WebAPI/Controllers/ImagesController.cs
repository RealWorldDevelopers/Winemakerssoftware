using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WMS.Business.Image.Dto;

namespace WMS.Service.WebAPI.Controllers
{

    [ApiVersion("1.0")]
    //[ApiVersion("1.1")]
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ImagesController : ControllerBase
    {
        private readonly Business.Image.IFactory _factory;

        public ImagesController(Business.Image.IFactory ImagesQryFactory)
        {
            _factory = ImagesQryFactory;
        }

        /// <summary>
        /// Get a list of All Images
        /// </summary>
        /// <returns><see cref="List{ImageDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet(Name = "GetAllImages")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var qry = _factory.CreateImagesQuery();
            var dto = await qry.Execute().ConfigureAwait(false);
            return Ok(dto);
        }

        /// <summary>
        /// Get a list of All Images Paginated
        /// </summary>
        /// <returns><see cref="List{ImageDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("{start:int}/{length:int}", Name = "GetAllImagesPaginated")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int start, int length)
        {
            var qry = _factory.CreateImagesQuery();
            var dto = await qry.Execute(start, length).ConfigureAwait(false);
            return Ok(dto);
        }

        /// <summary>
        /// Get a Image by Primary Key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="ImageDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>           
        //[MapToApiVersion("1.1")]
        [HttpGet("{id:int}", Name = "GetImageById")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            var qry = _factory.CreateImagesQuery();
            var dto = await qry.Execute(id).ConfigureAwait(false);
            return Ok(dto);

        }

        /// <summary>
        /// Add a new Image
        /// </summary>
        /// <param name="Image">New Values as <see cref="ImageDto"/></param>
        /// <returns><see cref="ImageDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPost(Name = "AddImage")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(ImageDto Image)
        {
            var cmd = _factory.CreateImagesCommand();
            Image = await cmd.Add(Image).ConfigureAwait(false);
            return Ok(Image);
        }

        /// <summary>
        /// Update a Image
        /// </summary>
        /// <param name="Image">New Values as <see cref="ImageDto"/></param>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="ImageDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPut("{id:int}", Name = "UpdateImage")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, ImageDto Image)
        {
            var cmd = _factory.CreateImagesCommand();
            Image.Id = id;
            Image = await cmd.Update(Image).ConfigureAwait(false);
            return Ok(Image);
        }

        /// <summary>
        /// Delete a Image by Primary Key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>        
        //[MapToApiVersion("1.1")]
        [HttpDelete("{id:int}", Name = "DeleteImageById")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var cmd = _factory.CreateImagesCommand();
            await Task.Delay(100);
            await cmd.Delete(id).ConfigureAwait(false);
            return Ok();
        }



    }


}
