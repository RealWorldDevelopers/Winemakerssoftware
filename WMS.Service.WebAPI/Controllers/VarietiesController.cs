using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
   [Authorize(Policy = "AccessAsUser")]
   public class VarietiesController : ControllerBase
   {
      private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
      private const string getAllVarietiesCacheKey = "getAllVarieties";
      private readonly Business.Recipe.IFactory _factory;
      private readonly IMemoryCache _cache;
      private readonly AppSettings _appSettings;

      public VarietiesController(Business.Recipe.IFactory factory, IOptions<AppSettings> appSettings, IMemoryCache cache)
      {
         _factory = factory ?? throw new ArgumentNullException(nameof(factory));
         _cache = cache ?? throw new ArgumentNullException(nameof(cache));
         _appSettings = appSettings.Value;
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
         // check cache
         if (!_cache.TryGetValue(getAllVarietiesCacheKey, out IEnumerable<ICodeDto> dto))
         {
            try
            {
               // lock inputs
               await semaphore.WaitAsync();

               // double check cache
               if (!_cache.TryGetValue(getAllVarietiesCacheKey, out dto))
               {
                  // fetch data
                  var qry = _factory.CreateVarietiesQuery();
                  dto = await qry.Execute().ConfigureAwait(false);

                  // cash options
                  var cacheEntryOptions = new MemoryCacheEntryOptions()
                      .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                      .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                      .SetPriority(CacheItemPriority.Normal)
                      .SetSize(1024);

                  // cache data
                  _cache.Set(getAllVarietiesCacheKey, dto, cacheEntryOptions);
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
         var dto = await cmd.Add(variety).ConfigureAwait(false);

         // set cache to update
         _cache.Remove(getAllVarietiesCacheKey);

         return Ok(dto);
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
         var dto = await cmd.Update(variety).ConfigureAwait(false);

         // set cache to update
         _cache.Remove(getAllVarietiesCacheKey);

         return Ok(dto);
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

         // set cache to update
         _cache.Remove(getAllVarietiesCacheKey);

         return Ok();
      }



   }
}
