
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using WMS.Ui.Mvc6.Models.Journal;
using WMS.Communications;
using WMS.Domain;

namespace WMS.Ui.Mvc6.Controllers.Api
{
    /// <summary>
    /// API Controller for Ajax Journal calls
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IFactory _modelFactory;
        private readonly IJournalAgent _journalAgent;
        private readonly ITempUOMAgent _tempUOMAgent;
        private readonly ISugarUOMAgent _sugarUOMAgent;
        private readonly ITargetAgent _targetAgent;

        public JournalController(IJournalAgent journalAgent, ITempUOMAgent tempUOMAgent, ISugarUOMAgent sugarUOMAgent, ITargetAgent targetAgent, IFactory modelFactory)
        {
            _modelFactory = modelFactory;
            _journalAgent = journalAgent;
            _tempUOMAgent = tempUOMAgent;
            _sugarUOMAgent = sugarUOMAgent;
            _targetAgent = targetAgent;
        }


        [HttpPost("batchEntry/{id}")]
        public async Task<IActionResult> DeleteBatchEntry(int id)
        {
            try
            {
                var entryDto = await _journalAgent.GetBatchEntry(id).ConfigureAwait(false);
                if (entryDto?.Id != null)
                {
                    await _journalAgent.DeleteBatchEntry(entryDto.Id.Value).ConfigureAwait(false);
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
        public async Task<IActionResult> AddBatchEntry(int id, [FromBody] BatchEntryViewModel batchEntry)
        {
            try
            {
                if (batchEntry == null)
                    return NoContent();

                if (!batchEntry.HasEntryData())
                    return NoContent();

                BatchEntry dto = new BatchEntry
                {
                    BatchId = id,
                    Additions = batchEntry.Additions,
                    Bottled = batchEntry.Bottled,
                    Comments = batchEntry.Comments,
                    EntryDateTime = DateTime.Now,
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
                    // var uom = _queryFactory.CreateBatchSugarUOMQuery();
                    dto.SugarUom = await _sugarUOMAgent.GetUOM(batchEntry.SugarUomId.Value).ConfigureAwait(false);
                }

                if (batchEntry.TempUomId.HasValue)
                {
                    // var uom = _queryFactory.CreateBatchTempUOMQuery();
                    dto.TempUom = await _tempUOMAgent.GetUOM(batchEntry.TempUomId.Value).ConfigureAwait(false);
                }

                // var cmd = _commandsFactory.CreateBatchEntriesCommand();
                var entryDto = await _journalAgent.AddBatchEntry(dto).ConfigureAwait(false);


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
        public async Task<IActionResult> GetBatchEntryChartData(int id)
        {
            try
            {
                // get record from db
                var entries = await _journalAgent.GetBatchEntries(id).ConfigureAwait(false);
                var sortedEntries = entries.OrderBy(e => e.ActionDateTime);

                var chartData = new BatchEntryChartDataViewModel();

                // handle if entries is null
                if (entries != null)
                {
                    foreach (var entry in sortedEntries)
                    {
                        if (entry.Temp.HasValue && entry.Sugar.HasValue)
                        {
                            var tDate = DateTime.Now.ToLocalTime();

                            if (entry.ActionDateTime.HasValue)
                                entry.ActionDateTime.Value.ToLocalTime();
                            else if (entry.EntryDateTime.HasValue)
                                entry.EntryDateTime.Value.ToLocalTime();

                            chartData.Times.Add(tDate.ToShortDateString());

                            // make sure is in F
                            if (entry.TempUom?.Abbreviation == "C")
                            {
                                var f = RWD.Toolbox.Conversion.Temperature.ConvertCelsiusToFahrenheit(entry.Temp.Value);
                                if (f != null) chartData.Temp.Add(f.Value);
                            }
                            else
                            {
                                chartData.Temp.Add(entry.Temp.Value);
                            }

                            // make sure is in SG
                            if (entry.SugarUom?.Abbreviation?.ToUpper(CultureInfo.CurrentCulture) != "SG")
                            {
                                var brix = entry.Sugar.Value;
                                var sg = brix / (258.6 - brix / 258.2 * 227.1) + 1;
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
        public async Task<IActionResult> SetBatchEntryComplete(int id, [FromBody] bool complete)
        {
            try
            {
                var entryDto = await _journalAgent.GetBatch(id).ConfigureAwait(false);

                if (entryDto != null)
                {
                    entryDto.Complete = complete;
                    await _journalAgent.UpdateBatch(entryDto).ConfigureAwait(false);
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
        public async Task<IActionResult> UpdateBatch(int id, [FromBody] BatchUpdateViewModel batch)
        {
            try
            {
                if (batch == null)
                    return NoContent();

                var dto = await _journalAgent.GetBatch(id).ConfigureAwait(false);

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
                        dto.Yeast = new Yeast { Id = batch.YeastId.Value };
                    else
                        dto.Yeast.Id = batch.YeastId.Value;
                }


                if (batch.VarietyId.HasValue)
                {
                    if (dto.Variety == null)
                        dto.Variety = new Variety();
                    if (dto.Variety.Id != batch.VarietyId.Value)
                    {
                        dto.Variety.Id = batch.VarietyId.Value;
                        dto.RecipeId = null;
                    }
                }

                if (batch.VolumeUomId.HasValue)
                    dto.VolumeUom = new UnitOfMeasure { Id = batch.VolumeUomId };

                var batchDto = await _journalAgent.UpdateBatch(dto).ConfigureAwait(false);

                var model = await _modelFactory.CreateBatchViewModel(batchDto).ConfigureAwait(false);

                return Ok(model);

            }
            catch (Exception)
            {
                return StatusCode(500);
                throw;
            }
        }

        [HttpPut("batchTarget/{id}")]
        public async Task<IActionResult> UpdateBatchTarget(int id, [FromBody] TargetUpdateViewModel target)
        {
            try
            {
                if (target == null)
                    return NoContent();

                var dto = await _journalAgent.GetBatch(id).ConfigureAwait(false);

                if (dto == null)
                    return NotFound();

                if (dto.Target == null)
                {
                    // create new target and link to this batch
                    // TODO create target then add
                    dto.Target = await _targetAgent.AddTarget(new Target()).ConfigureAwait(false);
                    dto = await _journalAgent.UpdateBatch(dto).ConfigureAwait(false);
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

                var targetDto = await _targetAgent.UpdateTarget(dto.Target).ConfigureAwait(false);
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
