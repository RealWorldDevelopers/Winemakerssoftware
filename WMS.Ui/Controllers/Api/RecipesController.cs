using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WMS.Data;
using Microsoft.AspNetCore.Authorization;


namespace WMS.Ui.Controllers.Api
{
    /// <summary>
    /// API Controller for Ajax Recipe calls
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly WMSContext _recipeContext;
        private readonly Business.Recipe.Queries.IFactory _queryFactory;
        private readonly Business.Recipe.Commands.IFactory _commandsFactory;

        public RecipesController(IMapper mapper, WMSContext dbContext, Business.Recipe.Queries.IFactory queryFactory, Business.Recipe.Commands.IFactory commandsFactory)
        {
            _mapper = mapper;
            _recipeContext = dbContext;
            _queryFactory = queryFactory;
            _commandsFactory = commandsFactory;
        }

        /// <summary>
        /// Increment Hit Count of a Recipe
        /// </summary>
        /// <param name="id">Recipe Primary Key as <see cref="string"</param>
        /// <returns>HTML Status Code</returns>
        [HttpPut("hits/{id}")]
        public async Task<IActionResult> UpdateHitsAsync(int id)
        {
            try
            {
                var getRecipesQuery = _queryFactory.CreateRecipesQuery();
                var recipeDto = await getRecipesQuery.ExecuteAsync(id);
                recipeDto.Hits++;
                var modifyRecipesCommand = _commandsFactory.CreateRecipesCommand();
                await modifyRecipesCommand.UpdateAsync(recipeDto);
                return Accepted();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Submit Ratings Vote for Recipe
        /// </summary>
        /// <param name="id">Recipe Primary Key as <see cref="string"</param>
        /// <param name="input">User Selected Rating Value</param>
        /// <returns>HTML Status Code</returns>     
        [HttpPut("rating/{id}")]
        public async Task<IActionResult> UpdateRatingAsync(int id, [FromBody] JObject input)
        {
            try
            {
                // check if valid input
                dynamic album = input;
                if (!double.TryParse(album.starValue?.Value, out double newValue))
                    return NoContent();

                // get record from db
                var getRecipesQuery = _queryFactory.CreateRecipesQuery();
                var recipe = await getRecipesQuery.ExecuteAsync(id);
                var rating = recipe.Rating;

                // if never rated, create first rating
                var newRating = false;
                if (rating == null)
                {
                    rating = new Business.Recipe.Dto.Rating
                    {
                        RecipeId = recipe.Id,
                        TotalValue = 0,
                        TotalVotes = 0,
                        OriginIp = string.Empty
                    };
                    newRating = true;
                }

                // IP check if in list stop else add to array and continue
                var separator = "|";
                string[] ipArray = rating.OriginIp.Split(separator);
                string incomingIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                int pos = Array.IndexOf(ipArray, incomingIp);
                if (pos > -1)
                {
                    return NoContent();
                }
                else
                {
                    var ipList = ipArray.ToList();
                    ipList.Add(incomingIp);
                    if (string.IsNullOrWhiteSpace(rating.OriginIp))
                        rating.OriginIp = incomingIp;
                    else
                        rating.OriginIp = string.Join(separator, ipList);
                }

                // add together the current rating value and the supplied rating value for a new rating value
                var current_rating = rating.TotalValue;
                var rating_sum = newValue + current_rating;

                // increment the current number of votes
                var total_votes = rating.TotalVotes;
                total_votes++;

                // adjust rating
                rating.TotalValue = rating_sum;
                rating.TotalVotes = total_votes;

                // save new values to table
                var modifyRatingsCommand = _commandsFactory.CreateRatingsCommand();

                if (newRating)
                    await modifyRatingsCommand.AddAsync(rating);
                else
                    await modifyRatingsCommand.UpdateAsync(rating);

                return Accepted();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


    }
}
