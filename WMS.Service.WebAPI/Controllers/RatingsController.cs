using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using WMS.Business.Recipe.Dto;

namespace WMS.Service.WebAPI.Controllers
{
   [ApiVersion("1.0")]
   //[ApiVersion("1.1")]
   [Route("api/[controller]")]
   [ApiController]
   [Consumes("application/json")]
   [Produces("application/json")]
   [Authorize(Policy = "AccessAsUser")]
   public class RatingsController : ControllerBase
   {
      private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
      private const string getAllRatingsCacheKey = "getAllRatings";
      private readonly Business.Recipe.IFactory _factory;
      private readonly IMemoryCache _cache;
      private readonly AppSettings _appSettings;

      public RatingsController(Business.Recipe.IFactory factory, IOptions<AppSettings> appSettings, IMemoryCache cache)
      {
         _factory = factory ?? throw new ArgumentNullException(nameof(factory));
         _cache = cache ?? throw new ArgumentNullException(nameof(cache));
         _appSettings = appSettings.Value;
      }

      /// <summary>
      /// Get a list of All Ratings
      /// </summary>
      /// <returns><see cref="List{RatingsDto}"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>    
      [HttpGet(Name = "GetAllRatings")]
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
         if (!_cache.TryGetValue(getAllRatingsCacheKey, out IEnumerable<RatingDto> dto))
         {
            try
            {
               // lock inputs
               await semaphore.WaitAsync();

               // double check cache
               if (!_cache.TryGetValue(getAllRatingsCacheKey, out dto))
               {
                  // fetch data
                  var qry = _factory.CreateRatingsQuery();
                  dto = await qry.Execute().ConfigureAwait(false);

                  // cash options
                  var cacheEntryOptions = new MemoryCacheEntryOptions()
                      .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                      .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                      .SetPriority(CacheItemPriority.Normal)
                      .SetSize(1024);

                  // cache data
                  _cache.Set(getAllRatingsCacheKey, dto, cacheEntryOptions);
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
      /// Add a new Rating
      /// </summary>
      /// <param name="rating">New Values as <see cref="RatingDto"/></param>
      /// <returns><see cref="RatingDto"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>          
      // [MapToApiVersion("1.1")]
      [HttpPost(Name = "AddRating")]
      [SwaggerResponse(StatusCodes.Status200OK)]
      [SwaggerResponse(StatusCodes.Status201Created)]
      [SwaggerResponse(StatusCodes.Status400BadRequest)]
      [SwaggerResponse(StatusCodes.Status401Unauthorized)]
      [SwaggerResponse(StatusCodes.Status403Forbidden)]
      [SwaggerResponse(StatusCodes.Status404NotFound)]
      [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
      [SwaggerResponse(StatusCodes.Status500InternalServerError)]
      public async Task<IActionResult> Post(RatingDto rating)
      {
         var cmd = _factory.CreateRatingsCommand();
         var dto = await cmd.Add(rating).ConfigureAwait(false);

         // set cache to update
         _cache.Remove(getAllRatingsCacheKey);

         return Ok(dto);
      }

      /// <summary>
      /// Update a Rating Entry
      /// </summary>
      /// <param name="rating">New Values as <see cref="RatingDto"/></param>
      /// <param name="id">Primary Key as <see cref="int"/></param>        
      /// <returns><see cref="RatingDto"/></returns>
      /// <response code = "200" > Returns items in collection</response>
      /// <response code = "204" > If items collection is null</response>
      /// <response code = "400" > If access is Bad Request</response>
      /// <response code = "401" > If access is Unauthorized</response>
      /// <response code = "403" > If access is Forbidden</response>
      /// <response code = "405" > If access is Not Allowed</response>
      /// <response code = "500" > If unhandled error</response>          
      //[MapToApiVersion("1.1")]
      [HttpPut("{id:int}", Name = "UpdateRating")]
      [SwaggerResponse(StatusCodes.Status200OK)]
      [SwaggerResponse(StatusCodes.Status201Created)]
      [SwaggerResponse(StatusCodes.Status400BadRequest)]
      [SwaggerResponse(StatusCodes.Status401Unauthorized)]
      [SwaggerResponse(StatusCodes.Status403Forbidden)]
      [SwaggerResponse(StatusCodes.Status404NotFound)]
      [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
      [SwaggerResponse(StatusCodes.Status500InternalServerError)]
      public async Task<IActionResult> Put(int id, RatingDto rating)
      {
         // get record from db
         var cmd = _factory.CreateRatingsCommand();
         rating.Id = id;
         var dto = await cmd.Update(rating).ConfigureAwait(false);

         // set cache to update
         _cache.Remove(getAllRatingsCacheKey);

         return Ok(dto);
      }


   }

}
