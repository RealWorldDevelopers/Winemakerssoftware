using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WMS.Business.Common;

namespace WMS.Service.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    //[ApiVersion("1.1")]
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class VarietiesController : ControllerBase
    {
        private readonly Business.Recipe.IFactory _factory;

        public VarietiesController(Business.Recipe.IFactory recipesQryFactory)
        {
            _factory = recipesQryFactory;
        }


        /// <summary>
        /// Get a list of All Varieties
        /// </summary>
        /// <returns></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>     
        [HttpGet(Name = "GetAllVarieties")]
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
            var qry = _factory.CreateVarietiesQuery();
            var dto = await qry.Execute().ConfigureAwait(false);
            return Ok(dto);
        }

        /// <summary>
        /// Get a Variety by Primary Key
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
        [HttpGet("{id:int}", Name = "GetVarietyById")]
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
            var qry = _factory.CreateVarietiesQuery();
            var dto = await qry.Execute(id).ConfigureAwait(false);
            return Ok(dto);

        }


        /// <summary>
        /// Add a new Variety
        /// </summary>
        /// <param name="variety">New Values as <see cref="CodeDto"/></param>
        /// <returns><see cref="ICodeDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPost(Name = "AddVariety")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CodeDto variety)
        {
            var cmd = _factory.CreateVarietiesCommand();
            var cat = await cmd.Add(variety).ConfigureAwait(false);
            return Ok(cat);
        }

        /// <summary>
        /// Update a Variety
        /// </summary>
        /// <param name="variety">New Values as <see cref="CodeDto"/></param>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="ICodeDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPut("{id:int}", Name = "UpdateVariety")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, CodeDto variety)
        {
            var cmd = _factory.CreateVarietiesCommand();
            variety.Id = id;
            var cat = await cmd.Update(variety).ConfigureAwait(false);
            return Ok(cat);
        }

        /// <summary>
        /// Delete a Variety by Primary Key
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
        [HttpDelete("{id:int}", Name = "DeleteVarietyById")]
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
            var cmd = _factory.CreateVarietiesCommand();
            await cmd.Delete(id).ConfigureAwait(false);
            return Ok();
        }



    }
}
