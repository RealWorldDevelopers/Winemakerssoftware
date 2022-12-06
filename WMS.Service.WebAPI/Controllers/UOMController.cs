
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private const string getAllTempUOMsCacheKey = "getAllTempUOMs";
        private const string getAllSugarUOMsCacheKey = "getAllSugarUOMs";
        private const string getAllVolumeUOMsCacheKey = "getAllVolumeUOMs";
        private readonly Business.Recipe.IFactory _factory;
        private readonly IMemoryCache _cache;
        private readonly AppSettings _appSettings;

        public UOMController(Business.Recipe.IFactory factory, IOptions<AppSettings> appSettings, IMemoryCache cache)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Get a UOM by Primary Key
        /// </summary>
        /// <returns><see cref="UnitOfMeasureDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("{id:int}", Name = "GetUomById")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUOM(int id)
        {
            var qry = _factory.CreateUOMQuery();
            var dto = await qry.ExecuteAsync(id).ConfigureAwait(false);
            return Ok(dto);
        }


        /// <summary>
        /// Get a list of All Temperature UOMs
        /// </summary>
        /// <returns><see cref="List{UnitOfMeasureDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("temp", Name = "GetAllTempUOMs")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTempUOMs()
        {
            // check cache
            if (!_cache.TryGetValue(getAllTempUOMsCacheKey, out IEnumerable<UnitOfMeasureDto> dto))
            {
                try
                {
                    // lock inputs
                    await semaphore.WaitAsync();

                    // double check cache
                    if (!_cache.TryGetValue(getAllTempUOMsCacheKey, out dto))
                    {
                        // fetch data
                        var qry = _factory.CreateUOMQuery();
                        dto = await qry.ExecuteAsync(Business.Common.Subsets.Temperature.Standard).ConfigureAwait(false);

                        // cash options
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1024);

                        // cache data
                        _cache.Set(getAllTempUOMsCacheKey, dto, cacheEntryOptions);
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
        /// Get a list of All Volume UOMs
        /// </summary>
        /// <returns><see cref="List{UnitOfMeasureDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("vol", Name = "GetAllVolumeUOMs")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVolumeUOMs()
        {


            // check cache
            if (!_cache.TryGetValue(getAllVolumeUOMsCacheKey, out IEnumerable<UnitOfMeasureDto> dto))
            {
                try
                {
                    // lock inputs
                    await semaphore.WaitAsync();

                    // double check cache
                    if (!_cache.TryGetValue(getAllVolumeUOMsCacheKey, out dto))
                    {
                        // fetch data
                        var qry = _factory.CreateUOMQuery();
                        dto = await qry.ExecuteAsync(Business.Common.Subsets.Volume.Standard).ConfigureAwait(false);

                        // cash options
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1024);

                        // cache data
                        _cache.Set(getAllVolumeUOMsCacheKey, dto, cacheEntryOptions);
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
        /// Get a list of All Sugar UOMs
        /// </summary>
        /// <returns><see cref="List{UnitOfMeasureDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("sugar", Name = "GetAllSugarUOMs")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSugarUOMs()
        {
            // check cache
            if (!_cache.TryGetValue(getAllSugarUOMsCacheKey, out IEnumerable<UnitOfMeasureDto> dto))
            {
                try
                {
                    // lock inputs
                    await semaphore.WaitAsync();

                    // double check cache
                    if (!_cache.TryGetValue(getAllSugarUOMsCacheKey, out dto))
                    {
                        // fetch data
                        var qry = _factory.CreateUOMQuery();
                        dto = await qry.ExecuteAsync(Business.Common.Subsets.Sugar.Standard).ConfigureAwait(false);

                        // cash options
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1024);

                        // cache data
                        _cache.Set(getAllSugarUOMsCacheKey, dto, cacheEntryOptions);
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
