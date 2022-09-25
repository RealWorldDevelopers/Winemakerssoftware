using Microsoft.AspNetCore.Mvc;
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
    public class RatingsController : ControllerBase
    {
        private readonly Business.Recipe.IFactory _factory;

        public RatingsController(Business.Recipe.IFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Get a list of All Ratings
        /// </summary>
        /// <returns><see cref="List{RatingsDto}"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>    
        [HttpGet(Name = "GetAllRatings")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var qry = _factory.CreateRatingsQuery();
                var dto = await qry.Execute().ConfigureAwait(false);
                return Ok(dto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add a new Rating
        /// </summary>
        /// <param name="rating">New Values as <see cref="RatingDto"/></param>
        /// <returns><see cref="RatingDto"/></returns>
        /// <response code = "200" > Returns items in collection</response>
        /// <response code = "204" > If items collection is null</response>
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
       // [MapToApiVersion("1.1")]
        [HttpPost(Name = "AddRating")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(RatingDto rating)
        {
            var cmd = _factory.CreateRatingsCommand();
            var dto = await cmd.Add(rating).ConfigureAwait(false);
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
        /// <response code = "401" > If access is Unauthorized</response>
        /// <response code = "403" > If access is Forbidden</response>
        /// <response code = "405" > If access is Not Allowed</response>
        /// <response code = "500" > If unhandled error</response>          
        //[MapToApiVersion("1.1")]
        [HttpPut("{id:int}", Name = "UpdateRating")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status201Created)]
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
            return Ok(dto);
        }


    }

}
