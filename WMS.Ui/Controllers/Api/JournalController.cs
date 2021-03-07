using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WMS.Ui.Models.Journal;
using WMS.Business.Journal.Dto;
using System.Globalization;
using WMS.Business.Common;
using System.Linq;
using WMS.Business.Yeast.Dto;

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


      [HttpGet("batchChart/{id}")]
      public async Task<IActionResult> GetBatchEntryChartDataAsync(int id)
      {
         try
         {
            // get record from db
            var getBatchEntriesQuery = _queryFactory.CreateBatchEntriesQuery();
            var entries = await getBatchEntriesQuery.ExecuteByFKAsync(id).ConfigureAwait(false);
            var sortedEntries = entries.OrderBy(e => e.ActionDateTime);

            var chartData = new BatchEntryChartDataViewModel();

            // handle if entries is null
            if (entries != null)
            {
               foreach (var entry in sortedEntries)
               {
                  if (entry.Temp.HasValue && entry.Sugar.HasValue)
                  {
                     var tDate = entry.ActionDateTime.HasValue ?
                        entry.ActionDateTime.Value.ToLocalTime() :
                        entry.EntryDateTime.Value.ToLocalTime();

                     chartData.Times.Add(tDate.ToShortDateString());

                     // make sure is in F
                     if (entry.TempUom?.Abbreviation == "C")
                     {
                        var f = RWD.Toolbox.Conversion.Temperature.ConvertCelsiusToFahrenheit(entry.Temp.Value);
                        chartData.Temp.Add(f.Value);
                     }
                     else
                     {
                        chartData.Temp.Add(entry.Temp.Value);
                     }

                     // make sure is in SG
                     if (entry.SugarUom?.Abbreviation.ToUpper(CultureInfo.CurrentCulture) != "SG")
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

      [HttpPut("batchComplete/{id}")]
      public async Task<IActionResult> SetBatchEntryCompleteAsync(int id, [FromBody] bool complete)
      {
         try
         {
            var entriesQuery = _queryFactory.CreateBatchesQuery();
            var entryDto = await entriesQuery.ExecuteAsync(id).ConfigureAwait(false);

            if (entryDto != null)
            {
               entryDto.Complete = complete;
               var cmd = _commandsFactory.CreateBatchesCommand();
               await cmd.UpdateAsync(entryDto).ConfigureAwait(false);
            }
            return Ok();
         }
         catch (Exception)
         {
            return StatusCode(500);
            throw;
         }

      }

      [HttpPut("batchUpdate/{id}")]
      public async Task<IActionResult> UpdateBatchAsync(int id, [FromBody] BatchUpdateViewModel batch)
      {
         try
         {
            if (batch == null)
               return NoContent();

            var qry = _queryFactory.CreateBatchesQuery();
            var dto = await qry.ExecuteAsync(id).ConfigureAwait(false);

            if (dto == null)
               return NotFound();

            dto.Description = batch.Description;
            dto.Title = batch.Title;
            dto.Vintage = batch.Vintage;
            dto.Volume = batch.Volume;
            dto.MaloCultureId = batch.MaloCultureId;

            if (batch.YeastId.HasValue)
            {
               if (dto.Yeast == null)
                  dto.Yeast = new YeastDto { Id = batch.YeastId.Value };
               else
                  dto.Yeast.Id = batch.YeastId.Value;
            }


            if (batch.VarietyId.HasValue)
            {
               if (dto.Variety == null)
                  dto.Variety = new Code();
               if (dto.Variety.Id != batch.VarietyId.Value)
               {
                  dto.Variety.Id = batch.VarietyId.Value;
                  dto.RecipeId = null;
               }
            }

            if (batch.VolumeUOM.HasValue)
               dto.VolumeUom = new UnitOfMeasure { Id = batch.VolumeUOM.Value };

            var cmd = _commandsFactory.CreateBatchesCommand();
            var batchDto = await cmd.UpdateAsync(dto).ConfigureAwait(false);

            var model = _modelFactory.CreateBatchViewModel(batchDto);

            return Ok(model);

         }
         catch (Exception)
         {
            return StatusCode(500);
            throw;
         }
      }

      [HttpPut("batchTarget/{id}")]
      public async Task<IActionResult> UpdateBatchTargetAsync(int id, [FromBody] TargetUpdateViewModel target)
      {
         try
         {
            if (target == null)
               return NoContent();

            var qry = _queryFactory.CreateBatchesQuery();
            var dto = await qry.ExecuteAsync(id).ConfigureAwait(false);

            if (dto == null)
               return NotFound();

            if (dto.Target == null)
            {
               // create new target and link to this batch
               var newCmd = _commandsFactory.CreateTargetsCommand();
               dto.Target = await newCmd.AddAsync(new TargetDto()).ConfigureAwait(false);
               var batchCmd = _commandsFactory.CreateBatchesCommand();
               dto = await batchCmd.UpdateAsync(dto).ConfigureAwait(false);
            }

            dto.Target.EndSugar = target.EndingSugar;
            dto.Target.pH = target.pH;
            dto.Target.StartSugar = target.StartingSugar;
            dto.Target.TA = target.TA;
            dto.Target.Temp = target.FermentationTemp;

            if (target.EndSugarUOM.HasValue)
               dto.Target.EndSugarUom = new UnitOfMeasure { Id = target.EndSugarUOM.Value };
            if (target.StartSugarUOM.HasValue)
               dto.Target.StartSugarUom = new UnitOfMeasure { Id = target.StartSugarUOM.Value };
            if (target.TempUOM.HasValue)
               dto.Target.TempUom = new UnitOfMeasure { Id = target.TempUOM.Value };

            var cmd = _commandsFactory.CreateTargetsCommand();
            var targetDto = await cmd.UpdateAsync(dto.Target).ConfigureAwait(false);

            var model = _modelFactory.CreateTargetViewModel(targetDto);

            return Ok(model);

         }
         catch (Exception)
         {
            return StatusCode(500);
            throw;
         }
      }

   }

}
