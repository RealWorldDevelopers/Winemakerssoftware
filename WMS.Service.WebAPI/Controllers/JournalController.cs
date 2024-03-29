﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
   [Authorize(Policy = "AccessAsUser")]
   public class JournalController : ControllerBase
   {
      private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
      private const string getAllBatchesCacheKey = "getAllBatches";
      private readonly Business.Journal.IFactory _factory;
      private readonly IMemoryCache _cache;
      private readonly AppSettings _appSettings;

      public JournalController(Business.Journal.IFactory factory, IOptions<AppSettings> appSettings, IMemoryCache cache)
      {
         _factory = factory ?? throw new ArgumentNullException(nameof(factory));
         _cache = cache ?? throw new ArgumentNullException(nameof(cache));
         _appSettings = appSettings.Value;
      }

      /// <summary>
      /// Get a list of All Batches
      /// </summary>
      /// <returns><see cref="List{BatchDto}"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>    
      [HttpGet(Name = "GetAllBatches")]
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
         // check cache
         if (!_cache.TryGetValue(getAllBatchesCacheKey, out IEnumerable<BatchDto> dto))
         {
            try
            {
               // lock inputs
               await semaphore.WaitAsync();

               // double check cache
               if (!_cache.TryGetValue(getAllBatchesCacheKey, out dto))
               {
                  // fetch data
                  var qry = _factory.CreateBatchesQuery();
                  dto = await qry.Execute().ConfigureAwait(false);

                  // cash options
                  var cacheEntryOptions = new MemoryCacheEntryOptions()
                      .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                      .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                      .SetPriority(CacheItemPriority.Normal)
                      .SetSize(1024);

                  // cache data
                  _cache.Set(getAllBatchesCacheKey, dto, cacheEntryOptions);
               }

            }
            finally
            {
               // remove lock
               semaphore.Release();
            }

         }
         return Ok(dto);
      }

      /// <summary>
      /// Get a list of All Batches
      /// </summary>
      /// <returns><see cref="List{BatchDto}"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>    
      [HttpGet("{userId:guid}", Name = "GetAllBatchesByUser")]
      [SwaggerResponse(StatusCodes.Status200OK)]
      [SwaggerResponse(StatusCodes.Status201Created)]
      [SwaggerResponse(StatusCodes.Status400BadRequest)]
      [SwaggerResponse(StatusCodes.Status401Unauthorized)]
      [SwaggerResponse(StatusCodes.Status403Forbidden)]
      [SwaggerResponse(StatusCodes.Status404NotFound)]
      [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
      [SwaggerResponse(StatusCodes.Status500InternalServerError)]
      public async Task<IActionResult> Get(string userId)
      {
         var qry = _factory.CreateBatchesQuery();
         var dto = await qry.ExecuteByUser(userId).ConfigureAwait(false);
         return Ok(dto);
      }

      /// <summary>
      /// Get a list of All Batches Paginated
      /// </summary>
      /// <returns><see cref="List{BatchDto}"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>    
      [HttpGet("{start:int}/{length:int}", Name = "GetAllBatchesPaginated")]
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
         var qry = _factory.CreateBatchesQuery();
         var dto = await qry.Execute(start, length).ConfigureAwait(false);
         return Ok(dto);
      }

      /// <summary>
      /// Get a Journal by Primary Key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="BatchDto"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>           
      //[MapToApiVersion("1.1")]
      [HttpGet("{id:int}", Name = "GetBatchById")]
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
         var qry = _factory.CreateBatchesQuery();
         var dto = await qry.Execute(id).ConfigureAwait(false);
         return Ok(dto);

      }

      /// <summary>
      /// Add a new Journal
      /// </summary>
      /// <param name="batch">New Values as <see cref="BatchDto"/></param>
      /// <returns><see cref="BatchDto"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>          
      //[MapToApiVersion("1.1")]
      [HttpPost(Name = "AddBatch")]
      [SwaggerResponse(StatusCodes.Status200OK)]
      [SwaggerResponse(StatusCodes.Status201Created)]
      [SwaggerResponse(StatusCodes.Status400BadRequest)]
      [SwaggerResponse(StatusCodes.Status401Unauthorized)]
      [SwaggerResponse(StatusCodes.Status403Forbidden)]
      [SwaggerResponse(StatusCodes.Status404NotFound)]
      [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
      [SwaggerResponse(StatusCodes.Status500InternalServerError)]
      public async Task<IActionResult> Post(BatchDto batch)
      {
         var cmd = _factory.CreateBatchesCommand();
         var dto = await cmd.Add(batch).ConfigureAwait(false);

         // set cache to update
         _cache.Remove(getAllBatchesCacheKey);

         return Ok(dto);
      }

      /// <summary>
      /// Update a Journal
      /// </summary>
      /// <param name="batch">New Values as <see cref="BatchDto"/></param>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="BatchDto"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>          
      //[MapToApiVersion("1.1")]
      [HttpPut("{id:int}", Name = "UpdateJournal")]
      [SwaggerResponse(StatusCodes.Status200OK)]
      [SwaggerResponse(StatusCodes.Status201Created)]
      [SwaggerResponse(StatusCodes.Status400BadRequest)]
      [SwaggerResponse(StatusCodes.Status401Unauthorized)]
      [SwaggerResponse(StatusCodes.Status403Forbidden)]
      [SwaggerResponse(StatusCodes.Status404NotFound)]
      [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
      [SwaggerResponse(StatusCodes.Status500InternalServerError)]
      public async Task<IActionResult> Put(int id, BatchDto batch)
      {
         var cmd = _factory.CreateBatchesCommand();
         batch.Id = id;
         var dto = await cmd.Update(batch).ConfigureAwait(false);

         // set cache to update
         _cache.Remove(getAllBatchesCacheKey);

         return Ok(dto);
      }

      /// <summary>
      /// Delete a Journal by Primary Key
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
      [HttpDelete("{id:int}", Name = "DeleteBatchById")]
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
         var cmd = _factory.CreateBatchesCommand();
         await cmd.Delete(id).ConfigureAwait(false);

         // set cache to update
         _cache.Remove(getAllBatchesCacheKey);

         return Ok();
      }


   }

}
