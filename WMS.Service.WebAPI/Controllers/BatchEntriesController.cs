
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
    public class BatchEntriesController : ControllerBase
    {
        private readonly Business.Journal.IFactory _factory;

        public BatchEntriesController(Business.Journal.IFactory batchEntriesQryFactory)
        {
            _factory = batchEntriesQryFactory;
        }


        /// <summary>
        /// Get a list of All Batch Entries by Batch Id
        /// </summary>
        /// <returns><see cref="List{BatchEntryDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("fk/{batchId:int}", Name = "GetAllBatchEntriesByBatchId")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByFK(int batchId)
        {
            var qry = _factory.CreateBatchEntriesQuery();
            var dto = await qry.ExecuteByFK(batchId).ConfigureAwait(false);
            return Ok(dto);
        }

        // TODO either fix or delete, not used yet
        ///// <summary>
        ///// Get a list of All Batch Entries by Batch Id Paginated
        ///// </summary>
        ///// <returns><see cref="List{BatchEntryDto}"/></returns>
        ///// <response code = "200" > Returns items in collection</response>
        ///// <response code = "204" > If items collection is null</response>
        ///// <response code = "400" > If access is Bad Request</response>
        ///// <response code = "401" > If access is Unauthorized</response>
        ///// <response code = "403" > If access is Forbidden</response>
        ///// <response code = "405" > If access is Not Allowed</response>
        ///// <response code = "500" > If unhandled error</response>    
        //[HttpGet("fk/{batchId:int},{start:int}/{length:int}", Name = "GetAllBatchEntriesPaginated")]
        //[SwaggerResponse(StatusCodes.Status200OK)]
        //[SwaggerResponse(StatusCodes.Status201Created)]
        //[SwaggerResponse(StatusCodes.Status400BadRequest)]
        //[SwaggerResponse(StatusCodes.Status401Unauthorized)]
        //[SwaggerResponse(StatusCodes.Status403Forbidden)]
        //[SwaggerResponse(StatusCodes.Status404NotFound)]
        //[SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        //[SwaggerResponse(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetByFK(int batchId, int start, int length)
        //{
            
        //    Debug.Assert(false);

        //    var qry = _factory.CreateBatchEntriesQuery();
        //    var dto = await qry.Execute(start, length).ConfigureAwait(false);
        //    return Ok(dto);
        //}

        /// <summary>
        /// Get a Batch Entry by Primary Key
        /// </summary>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="BatchEntryDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>           
        //[MapToApiVersion("1.1")]
        [HttpGet("{id:int}", Name = "GetBatchEntryById")]
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
            var qry = _factory.CreateBatchEntriesQuery();
            var dto = await qry.Execute(id).ConfigureAwait(false);
            return Ok(dto);

        }

        /// <summary>
        /// Add a new Batch Entry
        /// </summary>
        /// <param name="BatchEntry">New Values as <see cref="BatchEntryDto"/></param>
        /// <returns><see cref="BatchEntryDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPost(Name = "AddBatchEntry")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(BatchEntryDto BatchEntry)
        {
            var cmd = _factory.CreateBatchEntriesCommand();
            BatchEntry = await cmd.Add(BatchEntry).ConfigureAwait(false);
            return Ok(BatchEntry);
        }

        /// <summary>
        /// Update a Batch Entry
        /// </summary>
        /// <param name="BatchEntry">New Values as <see cref="BatchEntryDto"/></param>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="BatchEntryDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPut("{id:int}", Name = "UpdateBatchEntry")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, BatchEntryDto BatchEntry)
        {
            var cmd = _factory.CreateBatchEntriesCommand();
            BatchEntry.Id = id;
            BatchEntry = await cmd.Update(BatchEntry).ConfigureAwait(false);
            return Ok(BatchEntry);
        }

        /// <summary>
        /// Delete a Batch Entry by Primary Key
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
        [HttpDelete("{id:int}", Name = "DeleteBatchEntryById")]
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
            var cmd = _factory.CreateBatchEntriesCommand();
            await Task.Delay(100);
            await cmd.Delete(id).ConfigureAwait(false);
            return Ok();
        }


    }

}
