
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WMS.Business.Common;

namespace WMS.Service.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    //[ApiVersion("1.1")]
    [Route("api/uom")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class UOMController : ControllerBase
    {
        private readonly Business.Recipe.IFactory _factory;

        public UOMController(Business.Recipe.IFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Get a UOM by Primary Key
        /// </summary>
        /// <returns><see cref="UnitOfMeasureDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("{id:int}", Name = "GetUonById")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTempUOMs(int id)
        {
            try
            {
                var qry = _factory.CreateUOMQuery();
                var dto = await qry.ExecuteAsync(id).ConfigureAwait(false);
                return Ok(dto);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Get a list of All Temperature UOMs
        /// </summary>
        /// <returns><see cref="List{UnitOfMeasureDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("temp",Name = "GetAllTempUOMs")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTempUOMs()
        {
            try
            {
                var qry = _factory.CreateUOMQuery();
                var dto = await qry.ExecuteAsync(Business.Common.Subsets.Temperature.Standard).ConfigureAwait(false);
                return Ok(dto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get a list of All Volume UOMs
        /// </summary>
        /// <returns><see cref="List{UnitOfMeasureDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("vol", Name = "GetAllVolumeUOMs")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVolumeUOMs()
        {
            try
            {
                var qry = _factory.CreateUOMQuery();
                var dto = await qry.ExecuteAsync(Business.Common.Subsets.Volume.Standard).ConfigureAwait(false);
                return Ok(dto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get a list of All Sugar UOMs
        /// </summary>
        /// <returns><see cref="List{UnitOfMeasureDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("sugar", Name = "GetAllSugarUOMs")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSugarUOMs()
        {
            try
            {
                var qry = _factory.CreateUOMQuery();
                var dto = await qry.ExecuteAsync(Business.Common.Subsets.Sugar.Standard).ConfigureAwait(false);
                return Ok(dto);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
