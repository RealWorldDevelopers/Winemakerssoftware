using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WMS.Communications;
using WMS.Domain;


namespace WMS.Ui.Mvc6.Controllers.Api
{
    /// <summary>
    /// API Controller for Ajax Recipe calls
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {        
        private readonly IRecipeAgent _recipeAgent;
        private readonly IRatingAgent _ratingAgent;

        public RecipesController(IRecipeAgent recipeAgent, IRatingAgent ratingAgent)
        {
            _recipeAgent = recipeAgent;
            _ratingAgent = ratingAgent;
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
                var recipeDto = await _recipeAgent.GetRecipe(id).ConfigureAwait(false);
                recipeDto.Hits++;
                await _recipeAgent.UpdateRecipe(recipeDto).ConfigureAwait(false);
                return Accepted();
            }
            catch (Exception)
            {
                return StatusCode(500);
                throw;
            }
        }

        /// <summary>
        /// Submit Ratings Vote for Recipe
        /// </summary>
        /// <param name="id">Recipe Primary Key as <see cref="string"</param>
        /// <param name="input">User Selected Rating Value</param>
        /// <returns>HTML Status Code</returns>     
        [HttpPut("rating/{id}")]
        public async Task<IActionResult> UpdateRatingAsync(int id, [FromBody] string value)
        {
            try
            {
                // check if valid input            
                if (!double.TryParse(value, out double newValue))
                    return BadRequest();

                // get record
                var recipe = await _recipeAgent.GetRecipe(id).ConfigureAwait(false);
                var rating = recipe.Rating;

                // if never rated, create first rating
                var newRating = false;
                if (rating == null)
                {
                    rating = new Rating
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
                var incomingIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                if (string.IsNullOrWhiteSpace(rating.OriginIp))
                    rating.OriginIp = incomingIp;
                else
                {
                    var ipArray = rating.OriginIp.Split(separator);
                    int pos = Array.IndexOf(ipArray, incomingIp);
                    if (pos > -1)
                    {
                        return NoContent();
                    }
                    else
                    {
                        var ipList = ipArray.ToList();
                        if (!string.IsNullOrWhiteSpace(incomingIp))
                            ipList.Add(incomingIp);
                        rating.OriginIp = string.Join(separator, ipList);
                    }
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

                if (newRating)
                    await _ratingAgent.AddRating(rating).ConfigureAwait(false);
                else
                    await _ratingAgent.UpdateRating(rating).ConfigureAwait(false);

                return Accepted();
            }
            catch (Exception)
            {
                return StatusCode(500);
                throw;
            }
        }


    }
}
