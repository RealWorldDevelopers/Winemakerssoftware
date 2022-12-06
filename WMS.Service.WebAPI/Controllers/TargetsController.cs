
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WMS.Business.Journal.Dto;

namespace WMS.Service.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    //[ApiVersion("1.1")]
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class TargetsController : ControllerBase
    {
        private readonly Business.Journal.IFactory _factory;

        public TargetsController(Business.Journal.IFactory TargetsQryFactory)
        {
            _factory = TargetsQryFactory;
        }


        // TODO Delete if not used
        ///// <summary>
        ///// Get a list of All Targets by Foreign Key
        ///// </summary>
        ///// <returns><see cref="List{TargetDto}"/></returns>
        ///// <response code = "200" > Returns items in collection</response>
        ///// <response code = "204" > If items collection is null</response>
        ///// <response code = "400" > If access is Bad Request</response>
        ///// <response code = "401" > If access is Unauthorized</response>
        ///// <response code = "403" > If access is Forbidden</response>
        ///// <response code = "405" > If access is Not Allowed</response>
        ///// <response code = "500" > If unhandled error</response>    
        //[HttpGet("fk/{TargetId:int}", Name = "GetAllTargetsByTargetId")]
        //[SwaggerResponse(StatusCodes.Status200OK)]
        //[SwaggerResponse(StatusCodes.Status201Created)]
        //[SwaggerResponse(StatusCodes.Status400BadRequest)]
        //[SwaggerResponse(StatusCodes.Status401Unauthorized)]
        //[SwaggerResponse(StatusCodes.Status403Forbidden)]
        //[SwaggerResponse(StatusCodes.Status404NotFound)]
        //[SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        //[SwaggerResponse(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetByFK(int TargetId)
        //{
        //        var qry = _factory.CreateTargetsQuery();
        //        var dto = await qry.ExecuteByFK(TargetId).ConfigureAwait(false);
        //        return Ok(dto);
        //}

        ///// <summary>
        ///// Get a list of All Target Entries by Target Id Paginated
        ///// </summary>
        ///// <returns><see cref="List{TargetDto}"/></returns>
        ///// <response code = "200" > Returns items in collection</response>
        ///// <response code = "204" > If items collection is null</response>
        ///// <response code = "400" > If access is Bad Request</response>
        ///// <response code = "401" > If access is Unauthorized</response>
        ///// <response code = "403" > If access is Forbidden</response>
        ///// <response code = "405" > If access is Not Allowed</response>
        ///// <response code = "500" > If unhandled error</response>    
        //[HttpGet("fk/{TargetId:int},{start:int}/{length:int}", Name = "GetAllTargetsPaginated")]
        //[SwaggerResponse(StatusCodes.Status200OK)]
        //[SwaggerResponse(StatusCodes.Status201Created)]
        //[SwaggerResponse(StatusCodes.Status400BadRequest)]
        //[SwaggerResponse(StatusCodes.Status401Unauthorized)]
        //[SwaggerResponse(StatusCodes.Status403Forbidden)]
        //[SwaggerResponse(StatusCodes.Status404NotFound)]
        //[SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        //[SwaggerResponse(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetByFK(int TargetId, int start, int length)
        //{
        //        var qry = _factory.CreateTargetsQuery();
        //        var dto = await qry.Execute(start, length).ConfigureAwait(false);
        //        return Ok(dto);
        //}

        /// <summary>
        /// Get a Target by Primary Key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="TargetDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>           
        //[MapToApiVersion("1.1")]
        [HttpGet("{id:int}", Name = "GetTargetById")]
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
            var qry = _factory.CreateTargetsQuery();
            var dto = await qry.Execute(id).ConfigureAwait(false);
            return Ok(dto);

        }

        /// <summary>
        /// Add a new Target 
        /// </summary>
        /// <param name="target">New Values as <see cref="TargetDto"/></param>
        /// <returns><see cref="TargetDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPost(Name = "AddTarget")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(TargetDto target)
        {
            var cmd = _factory.CreateTargetsCommand();
            var dto = await cmd.Add(target).ConfigureAwait(false);
            return Ok(dto);
        }

        /// <summary>
        /// Update a Target 
        /// </summary>
        /// <param name="target">New Values as <see cref="TargetDto"/></param>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="TargetDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPut("{id:int}", Name = "UpdateTarget")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, TargetDto target)
        {
            var cmd = _factory.CreateTargetsCommand();
            target.Id = id;
            var dto = await cmd.Update(target).ConfigureAwait(false);
            return Ok(dto);
        }

        /// <summary>
        /// Delete a Target by Primary Key
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
        [HttpDelete("{id:int}", Name = "DeleteTargetById")]
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
            var cmd = _factory.CreateTargetsCommand();
            await cmd.Delete(id).ConfigureAwait(false);
            return Ok();
        }


    }

}
