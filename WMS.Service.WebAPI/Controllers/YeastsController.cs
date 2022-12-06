using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;

namespace WMS.Service.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    //[ApiVersion("1.1")]
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class YeastsController : ControllerBase
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private const string getAllYeastsCacheKey = "getAllYeasts";
        private const string getAllYeastPairsCacheKey = "getAllYeastPairsCacheKey";
        private const string getAllYeastBrandsCacheKey = "getAllYeastBrandsCacheKey";
        private const string getAllYeastStylesCacheKey = "getAllYeastStylesCacheKey";
        private readonly Business.Yeast.IFactory _factory;
        private readonly IMemoryCache _cache;
        private readonly AppSettings _appSettings;

        public YeastsController(Business.Yeast.IFactory factory, IOptions<AppSettings> appSettings, IMemoryCache cache)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Get a list of All Yeasts
        /// </summary>
        /// <returns><see cref="List{YeastDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet(Name = "GetAllYeasts")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetYeasts()
        {
            // check cache
            if (!_cache.TryGetValue(getAllYeastsCacheKey, out IEnumerable<YeastDto> dto))
            {
                try
                {
                    // lock inputs
                    await semaphore.WaitAsync();

                    // double check cache
                    if (!_cache.TryGetValue(getAllYeastsCacheKey, out dto))
                    {
                        // fetch data
                        var qry = _factory.CreateYeastsQuery();
                        dto = await qry.Execute().ConfigureAwait(false);

                        // cash options
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1024);

                        // cache data
                        _cache.Set(getAllYeastsCacheKey, dto, cacheEntryOptions);
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
        /// Get a Yeast by Primary Key
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
        [HttpGet("{id:int}", Name = "GetYeastById")]
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
            var qry = _factory.CreateYeastsQuery();
            var dto = await qry.Execute(id).ConfigureAwait(false);
            return Ok(dto);

        }


        /// <summary>
        /// Add a new Yeast
        /// </summary>
        /// <param name="yeast">New Values as <see cref="YeastDto"/></param>
        /// <returns><see cref="YeastDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPost(Name = "AddYeast")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(YeastDto yeast)
        {
            var cmd = _factory.CreateYeastsCommand();
            var dto = await cmd.Add(yeast).ConfigureAwait(false);

            // set cache to update
            _cache.Remove(getAllYeastsCacheKey);

            return Ok(dto);
        }

        /// <summary>
        /// Update a Yeast
        /// </summary>
        /// <param name="yeast">New Values as <see cref="YeastDto"/></param>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="YeastDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPut("{id:int}", Name = "UpdateYeast")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, YeastDto yeast)
        {
            var cmd = _factory.CreateYeastsCommand();
            yeast.Id = id;
            var dto = await cmd.Update(yeast).ConfigureAwait(false);

            // set cache to update
            _cache.Remove(getAllYeastsCacheKey);

            return Ok(dto);
        }

        /// <summary>
        /// Delete a Yeast by Primary Key
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
        [HttpDelete("{id:int}", Name = "DeleteYeastById")]
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
            var cmd = _factory.CreateYeastsCommand();
            await cmd.Delete(id).ConfigureAwait(false);

            // set cache to update
            _cache.Remove(getAllYeastsCacheKey);

            return Ok();
        }






        /// <summary>
        /// Get a list of All Yeast Pairings 
        /// </summary>
        /// <returns><see cref="List{YeastPairDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet("pairs", Name = "GetAllYeastPairs")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetYeastPairs()
        {
            // check cache
            if (!_cache.TryGetValue(getAllYeastPairsCacheKey, out IEnumerable<YeastPairDto> dto))
            {
                try
                {
                    // lock inputs
                    await semaphore.WaitAsync();

                    // double check cache
                    if (!_cache.TryGetValue(getAllYeastPairsCacheKey, out dto))
                    {
                        // fetch data
                        var qry = _factory.CreateYeastPairQuery();
                        dto = await qry.Execute().ConfigureAwait(false);

                        // cash options
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(_appSettings.DefaultSlidingCacheMinutes))
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_appSettings.DefaultAbosoluteCacheMinutes))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1024);

                        // cache data
                        _cache.Set(getAllYeastPairsCacheKey, dto, cacheEntryOptions);
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
        /// Get a Yeast Pairing by Primary Key
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
        [HttpGet("pairs/{id:int}", Name = "GetYeastPairsById")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetYeastPairs(int id)
        {
            var qry = _factory.CreateYeastPairQuery();
            var dto = await qry.Execute(id).ConfigureAwait(false);
            return Ok(dto);

        }

        /// <summary>
        /// Add a new Yeast Pairing
        /// </summary>
        /// <param name="pair">New Values as <see cref="YeastPairDto"/></param>
        /// <returns><see cref="YeastPairDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPost("pairs", Name = "AddYeastPair")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostYeastPair(YeastPairDto pair)
        {
            var cmd = _factory.CreateYeastPairCommand();
            var dto = await cmd.Add(pair).ConfigureAwait(false);

            // set cache to update
            _cache.Remove(getAllYeastPairsCacheKey);

            return Ok(dto);
        }

        /// <summary>
        /// Update a Yeast Pairing
        /// </summary>
        /// <param name="pair">New Values as <see cref="YeastPairDto"/></param>
        /// <param name="id">Primary Key as <see cref="int"/></param>
        /// <returns><see cref="YeastPairDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "400" > If access is Bad Request</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPut("pairs/{id:int}", Name = "UpdateYeastPair")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutYeastPair(int id, YeastPairDto pair)
        {
            var cmd = _factory.CreateYeastPairCommand();
            pair.Id = id;
            var dto = await cmd.Update(pair).ConfigureAwait(false);

            // set cache to update
            _cache.Remove(getAllYeastPairsCacheKey);

            return Ok(dto);
        }

        /// <summary>
        /// Delete a Yeast Pairing by Primary Key
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
        [HttpDelete("pairs/{id:int}", Name = "DeleteYeastPairById")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteYeastPair(int id)
        {
            var cmd = _factory.CreateYeastPairCommand();
            await cmd.Delete(id).ConfigureAwait(false);

            // set cache to update
            _cache.Remove(getAllYeastPairsCacheKey);

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
        [HttpGet("brands", Name = "GetAllYeastBrands")]
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
            if (!_cache.TryGetValue(getAllYeastBrandsCacheKey, out IEnumerable<ICodeDto> dto))
            {
                try
                {
                    // lock inputs
                    await semaphore.WaitAsync();

                    // double check cache
                    if (!_cache.TryGetValue(getAllYeastBrandsCacheKey, out dto))
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
                        _cache.Set(getAllYeastBrandsCacheKey, dto, cacheEntryOptions);
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
        [HttpGet("styles", Name = "GetAllYeastStyles")]
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
            if (!_cache.TryGetValue(getAllYeastStylesCacheKey, out IEnumerable<ICodeDto> dto))
            {
                try
                {
                    // lock inputs
                    await semaphore.WaitAsync();

                    // double check cache
                    if (!_cache.TryGetValue(getAllYeastStylesCacheKey, out dto))
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
                        _cache.Set(getAllYeastStylesCacheKey, dto, cacheEntryOptions);
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
