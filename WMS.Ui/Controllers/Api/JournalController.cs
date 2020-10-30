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
   /// API Controller for Ajax Journal calls
   /// </summary>
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   [Route("api/[controller]")]
   [ApiController]
   public class JournalController : ControllerBase
   {
      private readonly WMSContext _journalContext;
      private readonly Business.Journal.Queries.IFactory _queryFactory;
      private readonly Business.Journal.Commands.IFactory _commandsFactory;

      public JournalController(WMSContext dbContext, Business.Journal.Queries.IFactory queryFactory, Business.Journal.Commands.IFactory commandsFactory)
      {
         _journalContext = dbContext;
         _queryFactory = queryFactory;
         _commandsFactory = commandsFactory;
      }

      public async Task<IActionResult> AddBatchEntryAsync(int id, [FromBody] JObject input)
      {
         try
         {
            // check if valid input
            dynamic album = input;
            if (!double.TryParse(album.starValue?.Value, out double newValue))
               return NoContent();

            // get record from db
            var getBatchEntriesQuery = _queryFactory.CreateBatchEntriesQuery();
            var batch = await getBatchEntriesQuery.ExecuteAsync(id).ConfigureAwait(false);

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
