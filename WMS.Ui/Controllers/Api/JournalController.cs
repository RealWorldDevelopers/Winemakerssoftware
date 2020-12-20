using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WMS.Ui.Models.Journal;
using WMS.Business.Journal.Dto;
using System.Globalization;

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
      private readonly Business.Journal.Queries.IFactory _queryFactory;
      private readonly Business.Journal.Commands.IFactory _commandsFactory;
      private readonly Models.Journal.IFactory _modelFactory;

      public JournalController(Business.Journal.Queries.IFactory queryFactory,
         Business.Journal.Commands.IFactory commandsFactory, Models.Journal.IFactory modelFactory)
      {
         _queryFactory = queryFactory;
         _commandsFactory = commandsFactory;
         _modelFactory = modelFactory;
      }

      [HttpPost("batchEntry/{id}")]
      public async Task<IActionResult> DeleteBatchEntryAsync(int id)
      {
         try
         {
            var entriesQuery = _queryFactory.CreateBatchEntriesQuery();
            var entryDto = await entriesQuery.ExecuteAsync(id).ConfigureAwait(false);

            if (entryDto != null)
            {
               var cmd = _commandsFactory.CreateBatchEntriesCommand();
               await cmd.DeleteAsync(entryDto).ConfigureAwait(false);
            }
            return Ok();
         }
         catch (Exception)
         {
            return StatusCode(500);
            throw;
         }

      }

      [HttpPut("batchEntry/{id}")]
      public async Task<IActionResult> AddBatchEntryAsync(int id, [FromBody] BatchEntryViewModel batchEntry)
      {
         try
         {
            if (batchEntry == null)
               return NoContent();

            if (!batchEntry.HasEntryData())
               return NoContent();

            BatchEntryDto dto = new BatchEntryDto
            {
               BatchId = id,
               Additions = batchEntry.Additions,
               Bottled = batchEntry.Bottled,
               Comments = batchEntry.Comments,
               EntryDateTime = DateTime.UtcNow,
               ActionDateTime = batchEntry.ActionDateTime,
               Filtered = batchEntry.Filtered,
               pH = batchEntry.pH,
               Racked = batchEntry.Racked,
               So2 = batchEntry.So2,
               Sugar = batchEntry.Sugar,
               Ta = batchEntry.Ta,
               Temp = batchEntry.Temp
            };

            if (batchEntry.SugarUomId.HasValue)
            {
               var uom = _queryFactory.CreateBatchSugarUOMQuery();
               dto.SugarUom = await uom.ExecuteAsync(batchEntry.SugarUomId.Value).ConfigureAwait(false);
            }

            if (batchEntry.TempUomId.HasValue)
            {
               var uom = _queryFactory.CreateBatchTempUOMQuery();
               dto.TempUom = await uom.ExecuteAsync(batchEntry.TempUomId.Value).ConfigureAwait(false);
            }

            var cmd = _commandsFactory.CreateBatchEntriesCommand();
            var entryDto = await cmd.AddAsync(dto).ConfigureAwait(false);

            var model = _modelFactory.CreateBatchEntryViewModel(entryDto);

            return Ok(model);

         }
         catch (Exception)
         {
            return StatusCode(500);
            throw;
         }
      }

      // TODO DELETE if not used
      //[HttpGet("batchEntrySummary/{id}")]
      //public async Task<IActionResult> GetBatchEntrySummaryAsync(int id)
      //{
      //   try
      //   {
      //      var entriesQuery = _queryFactory.CreateBatchEntriesQuery();
      //      var entriesDto = await entriesQuery.ExecuteAsync().ConfigureAwait(false);
      //      var batchEntriesDto = entriesDto.Where(e => e.BatchId == id)
      //         .OrderByDescending(e => e.ActionDateTime).ThenByDescending(e => e.EntryDateTime).ToList();

      //      var model = _modelFactory.CreatBatchSummaryViewModel(batchEntriesDto);

      //      return Ok(model);

      //   }
      //   catch (Exception)
      //   {
      //      return StatusCode(500);
      //      throw;
      //   }
      //}

      [HttpGet("batchChart/{id}")]
      public async Task<IActionResult> GetBatchEntryChartDataAsync(int id)
      {
         try
         {
            // get record from db
            var getBatchEntriesQuery = _queryFactory.CreateBatchEntriesQuery();
            var entries = await getBatchEntriesQuery.ExecuteByFKAsync(id).ConfigureAwait(false);

            var chartData = new BatchEntryChartDataViewModel();

            // handle if entries is null
            if (entries != null)
            {
               foreach (var entry in entries)
               {
                  if (entry.Temp.HasValue && entry.Sugar.HasValue)
                  {
                     var tDate = entry.ActionDateTime.HasValue ?
                        entry.ActionDateTime.Value.ToLocalTime() :
                        entry.EntryDateTime.Value.ToLocalTime();

                     chartData.Times.Add(tDate.ToShortDateString());

                     // make sure is in F
                     if (entry.TempUom.Abbreviation == "C")
                     {
                        var f = RWD.Toolbox.Conversion.Temperature.ConvertCelsiusToFahrenheit(entry.Temp.Value);
                        chartData.Temp.Add(f.Value);
                     }
                     else
                     {
                        chartData.Temp.Add(entry.Temp.Value);
                     }

                     // make sure is in SG
                     if (entry.SugarUom.Abbreviation.ToUpper(CultureInfo.CurrentCulture) != "SG")
                     {
                        var brix = entry.Sugar.Value;
                        var sg = (brix / (258.6 - ((brix / 258.2) * 227.1))) + 1;
                        chartData.Sugar.Add(sg);
                     }
                     else
                     {
                        chartData.Sugar.Add(entry.Sugar.Value);
                     }

                  }
               }
            }

            return Ok(chartData);

         }
         catch (Exception)
         {
            return StatusCode(500);
            throw;
         }

      }

   }

}
