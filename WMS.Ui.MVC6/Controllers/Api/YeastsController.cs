
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WMS.Ui.Mvc6.Models.Yeasts;
using WMS.Communications;

namespace WMS.Ui.Mvc6.Controllers.Api
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
        private readonly IYeastAgent _yeastAgent;

        public YeastsController(IFactory modelFactory, IYeastAgent yeastAgent)
        {
            _modelFactory = modelFactory;
            _yeastAgent = yeastAgent;
        }

        [AllowAnonymous]
        [HttpGet("pairs")]
        public async Task<IEnumerable<YeastPairViewModel>> GetPairsAsync()
        {
            //var query = _queryFactory.CreateYeastPairQuery();
            var dto = await _yeastAgent.GetYeastPairs().ConfigureAwait(false);
            var pairs = _modelFactory.CreateYeastPairList(dto);

            return pairs;
        }

    }
}
