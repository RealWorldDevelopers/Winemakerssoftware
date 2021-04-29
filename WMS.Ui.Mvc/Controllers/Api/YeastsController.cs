
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WMS.Ui.Mvc.Models.Yeasts;

namespace WMS.Ui.Mvc.Controllers.Api
{
   /// <summary>
   /// API Controller for Ajax Recipe calls
   /// </summary>
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   [Route("api/[controller]")]
   [ApiController]
   public class YeastsController : ControllerBase
   {
      private readonly IFactory _modelFactory;
      private readonly Business.Yeast.Queries.IFactory _queryFactory;

      public YeastsController(IFactory modelFactory, Business.Yeast.Queries.IFactory queryFactory)
      {
         _modelFactory = modelFactory;
         _queryFactory = queryFactory;
      }

      [AllowAnonymous]
      [HttpGet("pairs")]
      public async Task<IEnumerable<YeastPair>> GetPairsAsync()
      {
         var query = _queryFactory.CreateYeastPairQuery();
         var dto = await query.ExecuteAsync().ConfigureAwait(false);
         var pairs = _modelFactory.CreateYeastPairList(dto);

         return pairs;
      }

   }
}
