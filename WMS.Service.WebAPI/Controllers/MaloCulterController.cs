
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using WMS.Business.Common;
using WMS.Business.MaloCulture.Dto;

namespace WMS.Service.WebAPI.Controllers
{
   [ApiVersion("1.0")]
   //[ApiVersion("1.1")]
   [Route("api/[controller]")]
   [ApiController]
   [Consumes("application/json")]
   [Produces("application/json")]
   [Authorize(Policy = "AccessAsUser")]
   public class MaloCultureController : ControllerBase
   {
      private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
      private const string getAllCulturesCacheKey = "getAllCultures";
      private const string getAllCultureBrandsCacheKey = "getAllCultureBrandsCacheKey";
      private const string getAllCultureStylesCacheKey = "getAllCultureStylesCacheKey";
      private readonly Business.MaloCulture.IFactory _factory;
      private readonly IMemoryCache _cache;
      private readonly AppSettings _appSettings;

      public MaloCultureController(Business.MaloCulture.IFactory factory, IOptions<AppSettings> appSettings, IMemoryCache cache)
      {
         _factory = factory ?? throw new ArgumentNullException(nameof(factory));
         _cache = cache ?? throw new ArgumentNullException(nameof(cache));
         _appSettings = appSettings.Value;
      }

      /// <summary>
      /// Get a list of All Cultures
      /// </summary>
      /// <returns><see cref="List{MaloCultureDto}"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>    
      [HttpGet(Name = "GetAllCultures")]
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
         if (!_cache.TryGetValue(getAllCulturesCacheKey, out IEnumerable<MaloCultureDto> dto))
         {
            try
            {
               // lock inputs
               await semaphore.WaitAsync();

               // double check cache
               if (!_cache.TryGetValue(getAllCulturesCacheKey, out dto))
               {
                  // fetch data
                  var qry = _factory.CreateMaloCulturesQuery();
                  dto = await qry.Execute().ConfigureAwait(false);

                  // cash options
                  var cacheEntryOptions = new MemoryCacheEntryOptions()
                      .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                      .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                      .SetPriority(CacheItemPriority.Normal)
                      .SetSize(1024);

                  // cache data
                  _cache.Set(getAllCulturesCacheKey, dto, cacheEntryOptions);
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

      // DELETE if Not Used
      ///// <summary>
      ///// Get a list of All Cultures Paginated
      ///// </summary>
      ///// <returns><see cref="List{MaloCultureDto}"/></returns>
      ///// <response code = "200" > Returns items in collection</response>
      ///// <response code = "204" > If items collection is null</response>
      ///// <response code = "400" > If access is Bad Request</response>
      ///// <response code = "401" > If access is Unauthorized</response>
      ///// <response code = "403" > If access is Forbidden</response>
      ///// <response code = "405" > If access is Not Allowed</response>
      ///// <response code = "500" > If unhandled error</response>    
      //[HttpGet("{start:int}/{length:int}", Name = "GetAllCulturesPaginated")]
      //[SwaggerResponse(StatusCodes.Status200OK)]
      //[SwaggerResponse(StatusCodes.Status201Created)]
      //[SwaggerResponse(StatusCodes.Status400BadRequest)]
      //[SwaggerResponse(StatusCodes.Status401Unauthorized)]
      //[SwaggerResponse(StatusCodes.Status403Forbidden)]
      //[SwaggerResponse(StatusCodes.Status404NotFound)]
      //[SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
      //[SwaggerResponse(StatusCodes.Status500InternalServerError)]
      //public async Task<IActionResult> Get(int start, int length)
      //{
      //    var qry = _factory.CreateMaloCulturesQuery();
      //    var dto = await qry.Execute(start, length).ConfigureAwait(false);
      //    return Ok(dto);
      //}

      /// <summary>
      /// Get a Culture by Primary Key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="MaloCultureDto"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>           
      //[MapToApiVersion("1.1")]
      [HttpGet("{id:int}", Name = "GetCultureById")]
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
         var qry = _factory.CreateMaloCulturesQuery();
         var dto = await qry.Execute(id).ConfigureAwait(false);
         return Ok(dto);

      }

      /// <summary>
      /// Add a new Culture
      /// </summary>
      /// <param name="culture">New Values as <see cref="MaloCultureDto"/></param>
      /// <returns><see cref="MaloCultureDto"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>          
      //[MapToApiVersion("1.1")]
      [HttpPost(Name = "AddCulture")]
      [SwaggerResponse(StatusCodes.Status200OK)]
      [SwaggerResponse(StatusCodes.Status201Created)]
      [SwaggerResponse(StatusCodes.Status400BadRequest)]
      [SwaggerResponse(StatusCodes.Status401Unauthorized)]
      [SwaggerResponse(StatusCodes.Status403Forbidden)]
      [SwaggerResponse(StatusCodes.Status404NotFound)]
      [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
      [SwaggerResponse(StatusCodes.Status500InternalServerError)]
      public async Task<IActionResult> Post(MaloCultureDto culture)
      {
         var cmd = _factory.CreateMaloCulturesCommand();
         var dto = await cmd.Add(culture).ConfigureAwait(false);

         // set cache to update
         _cache.Remove(getAllCulturesCacheKey);

         return Ok(dto);
      }

      /// <summary>
      /// Update a MaloCulture
      /// </summary>
      /// <param name="culture">New Values as <see cref="MaloCultureDto"/></param>
      /// <param name="id">Primary Key as <see cref="int"/></param>
      /// <returns><see cref="MaloCultureDto"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>          
      //[MapToApiVersion("1.1")]
      [HttpPut("{id:int}", Name = "UpdateCulture")]
      [SwaggerResponse(StatusCodes.Status200OK)]
      [SwaggerResponse(StatusCodes.Status201Created)]
      [SwaggerResponse(StatusCodes.Status400BadRequest)]
      [SwaggerResponse(StatusCodes.Status401Unauthorized)]
      [SwaggerResponse(StatusCodes.Status403Forbidden)]
      [SwaggerResponse(StatusCodes.Status404NotFound)]
      [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
      [SwaggerResponse(StatusCodes.Status500InternalServerError)]
      public async Task<IActionResult> Put(int id, MaloCultureDto culture)
      {
         var cmd = _factory.CreateMaloCulturesCommand();
         culture.Id = id;
         var dto = await cmd.Update(culture).ConfigureAwait(false);

         // set cache to update
         _cache.Remove(getAllCulturesCacheKey);

         return Ok(dto);
      }

      /// <summary>
      /// Delete a MaloCulture by Primary Key
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
      [HttpDelete("{id:int}", Name = "DeleteCultureById")]
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
         var cmd = _factory.CreateMaloCulturesCommand();
         await cmd.Delete(id).ConfigureAwait(false);

         // set cache to update
         _cache.Remove(getAllCulturesCacheKey);

         return Ok();
      }


      /// <summary>
      /// Get a list of All Brands 
      /// </summary>
      /// <returns><see cref="List{ICodeDto}"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>    
      [HttpGet("brands", Name = "GetAllMaloBrands")]
      [SwaggerResponse(StatusCodes.Status200OK)]
      [SwaggerResponse(StatusCodes.Status201Created)]
      [SwaggerResponse(StatusCodes.Status400BadRequest)]
      [SwaggerResponse(StatusCodes.Status401Unauthorized)]
      [SwaggerResponse(StatusCodes.Status403Forbidden)]
      [SwaggerResponse(StatusCodes.Status404NotFound)]
      [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
      [SwaggerResponse(StatusCodes.Status500InternalServerError)]
      public async Task<IActionResult> GetBrands()
      {
         // check cache
         if (!_cache.TryGetValue(getAllCultureBrandsCacheKey, out IEnumerable<ICodeDto> dto))
         {
            try
            {
               // lock inputs
               await semaphore.WaitAsync();

               // double check cache
               if (!_cache.TryGetValue(getAllCultureBrandsCacheKey, out dto))
               {
                  // fetch data
                  var qry = _factory.CreateBrandsQuery();
                  dto = await qry.Execute().ConfigureAwait(false);

                  // cash options
                  var cacheEntryOptions = new MemoryCacheEntryOptions()
                      .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                      .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                      .SetPriority(CacheItemPriority.Normal)
                      .SetSize(1024);

                  // cache data
                  _cache.Set(getAllCultureBrandsCacheKey, dto, cacheEntryOptions);
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
      /// Get a list of All Styles 
      /// </summary>
      /// <returns><see cref="List{ICodeDto}"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>    
      [HttpGet("styles", Name = "GetAllMaloStyles")]
      [SwaggerResponse(StatusCodes.Status200OK)]
      [SwaggerResponse(StatusCodes.Status201Created)]
      [SwaggerResponse(StatusCodes.Status400BadRequest)]
      [SwaggerResponse(StatusCodes.Status401Unauthorized)]
      [SwaggerResponse(StatusCodes.Status403Forbidden)]
      [SwaggerResponse(StatusCodes.Status404NotFound)]
      [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
      [SwaggerResponse(StatusCodes.Status500InternalServerError)]
      public async Task<IActionResult> GetStyles()
      {
         // check cache
         if (!_cache.TryGetValue(getAllCultureStylesCacheKey, out IEnumerable<ICodeDto> dto))
         {
            try
            {
               // lock inputs
               await semaphore.WaitAsync();

               // double check cache
               if (!_cache.TryGetValue(getAllCultureStylesCacheKey, out dto))
               {
                  // fetch data
                  var qry = _factory.CreateStylesQuery();
                  dto = await qry.Execute().ConfigureAwait(false);

                  // cash options
                  var cacheEntryOptions = new MemoryCacheEntryOptions()
                      .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                      .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                      .SetPriority(CacheItemPriority.Normal)
                      .SetSize(1024);

                  // cache data
                  _cache.Set(getAllCultureStylesCacheKey, dto, cacheEntryOptions);
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


   }

}
