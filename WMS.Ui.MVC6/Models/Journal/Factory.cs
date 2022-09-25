
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Globalization;
using WMS.Communications;
using WMS.Domain;

namespace WMS.Ui.Mvc6.Models.Journal
{
    public class Factory : IFactory
    {
        private readonly AppSettings _appSettings;

        private readonly Uri _batchUrl;


        private readonly IVarietyAgent _varietyAgent;
        private readonly ICategoryAgent _categoryAgent;
        private readonly IYeastAgent _yeastAgent;
        private readonly ITempUOMAgent _tempUOMAgent;
        private readonly ISugarUOMAgent _sugarUOMAgent;
        private readonly IVolumeUOMAgent _volumeUOMAgent;
        private readonly IMaloCultureAgent _maloCultureAgent;

        public Factory(IVarietyAgent varietyAgent, ICategoryAgent categoryAgent, IYeastAgent yeastAgent,
            ITempUOMAgent tempUOMAgent, ISugarUOMAgent sugarUOMAgent, IVolumeUOMAgent volumeUOMAgent,
            IMaloCultureAgent maloCultureAgent, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _batchUrl = new Uri(_appSettings.URLs.JournalBatch, UriKind.Relative);
            _varietyAgent = varietyAgent;
            _categoryAgent = categoryAgent;
            _yeastAgent = yeastAgent;
            _tempUOMAgent = tempUOMAgent;
            _sugarUOMAgent = sugarUOMAgent;
            _volumeUOMAgent = volumeUOMAgent;
            _maloCultureAgent = maloCultureAgent;
        }

        public JournalViewModel CreateJournalModel()
        {
            return new JournalViewModel();
        }

        public Task<BatchListItemViewModel> BuildBatchListItemModel(Batch batchDto)
        {
            Task<BatchListItemViewModel> t = Task.Run(() =>
            {
                if (!batchDto.Id.HasValue)
                    throw new NullReferenceException(nameof(batchDto.Id));

                var batchUri = new Uri(_batchUrl + "/" + batchDto.Id.Value.ToString(CultureInfo.CurrentCulture), UriKind.Relative);

                var model = new BatchListItemViewModel
                {
                    BatchComplete = batchDto.Complete ?? false,
                    Id = batchDto.Id.Value,
                    Title = batchDto.Title,
                    Vintage = batchDto.Vintage,
                    Variety = batchDto.Variety != null ? batchDto.Variety.Literal : string.Empty,
                    Description = batchDto.Description,
                    Summary = CreatBatchSummaryViewModel(batchDto.Entries),
                    BatchUrl = batchUri
                };
                return model;
            });
            return t;
        }

        public IEnumerable<BatchListItemViewModel> BuildBatchListItemModels(IEnumerable<Batch> dtoBatchList)
        {
            var modelList = new List<BatchListItemViewModel>();

            var batchStack = new Stack<Batch>(dtoBatchList);

            // Create 1 per core, and then as they finish, create another:   
            var tasks = new List<Task<BatchListItemViewModel>>();

            int numBatches = batchStack.Count;
            int numCores = Environment.ProcessorCount;

            // if numCors > N use only N  
            if (numCores > numBatches)
                numCores = numBatches;

            // create initial set of tasks:        
            for (int i = 0; i < numCores; i++)
            {
                Task<BatchListItemViewModel> t = BuildBatchListItemModel(batchStack.Pop());
                tasks.Add(t);
            }

            // now, as they finish, create more:
            int done = 0;
            while (done < numBatches)
            {
                int index = Task.WaitAny(tasks.ToArray());
                done++;
                modelList.Add(tasks[index].Result);
                tasks.RemoveAt(index);
                if (batchStack.Count > 0)
                {
                    Task<BatchListItemViewModel> t = BuildBatchListItemModel(batchStack.Pop());
                    tasks.Add(t);
                }
            }

            return modelList;
        }

        public TargetViewModel CreateTargetViewModel(Target target)
        {
            return CreateTargetViewModel(target, null, null);
        }

        public TargetViewModel CreateTargetViewModel(Target? target, IEnumerable<IUnitOfMeasure>? dtoSugarUOMList, IEnumerable<IUnitOfMeasure>? dtoTempUOMList)
        {

            var uomSugarList = CreateSelectList("Unit of Measure", dtoSugarUOMList);
            var uomTempList = CreateSelectList("Unit of Measure", dtoTempUOMList);

            var model = new TargetViewModel
            {
                SugarUOMs = uomSugarList,
                TempUOMs = uomTempList
            };

            if (target != null)
            {
                model.Id = target.Id;
                model.EndingSugar = target.EndSugar;
                model.EndSugarUOM = target.EndSugarUom?.Id;
                model.FermentationTemp = target.Temp;
                model.TempUOM = target.TempUom?.Id;
                model.StartingSugar = target.StartSugar;
                model.StartSugarUOM = target.StartSugarUom?.Id;
                model.TA = target.TA;
                model.pH = target.pH;
            }

            return model;
        }

        public async Task<BatchViewModel> CreateBatchViewModel()
        {
            return await CreateBatchViewModel(null, null);
        }

        public async Task<BatchViewModel> CreateBatchViewModel(Batch dto)
        {
            return await CreateBatchViewModel(dto, null);
        }


        public async Task<BatchViewModel> CreateBatchViewModel(Batch? dto, IEnumerable<BatchEntry>? entriesDto)
        {
            var cultureList = await _maloCultureAgent.GetMaloCultures().ConfigureAwait(false);
            var dtoTempUOMList = await _tempUOMAgent.GetUOMs().ConfigureAwait(false);
            var dtoSugarUOMList = await _sugarUOMAgent.GetUOMs().ConfigureAwait(false);
            var dtoVolumeUOMList = await _volumeUOMAgent.GetUOMs().ConfigureAwait(false);
            var dtoCategoryList = await _categoryAgent.GetCategories().ConfigureAwait(false);
            var dtoVarietyList = await _varietyAgent.GetVarieties().ConfigureAwait(false);
            var dtoYeastList = await _yeastAgent.GetYeasts().ConfigureAwait(false);

            var varieties = CreateSelectList("Variety", dtoVarietyList, dtoCategoryList);
            var uomVolumeList = CreateSelectList("Unit of Measure", dtoVolumeUOMList);
            var yeastsList = CreateSelectList("Yeast", dtoYeastList);
            var maloList = CreateSelectList("MLF Culture", cultureList);

            var newModel = new BatchViewModel
            {
                Varieties = varieties,
                VolumeUOMs = uomVolumeList,
                Yeasts = yeastsList,
                MaloCultures = maloList
            };

            if (dto == null)
            {
                newModel.Target = CreateTargetViewModel(null, dtoSugarUOMList, dtoTempUOMList);
            }
            else
            {
                newModel.Id = dto.Id;
                newModel.Complete = dto.Complete ?? false;
                newModel.Description = dto.Description;
                newModel.RecipeId = dto.RecipeId;
                newModel.Title = dto.Title ?? String.Empty;
                if (dto.Variety != null && dto.Variety.Id.HasValue)
                    newModel.VarietyId = dto.Variety.Id.Value;
                if (dto.Vintage.HasValue)
                    newModel.Vintage = dto.Vintage.Value;
                if (dto.Volume.HasValue)
                    newModel.Volume = dto.Volume.Value;
                if (dto.VolumeUom != null && dto.VolumeUom.Id.HasValue)
                    newModel.VolumeUomId = dto.VolumeUom.Id.Value;
                if (dto.Yeast != null && dto.Yeast.Id.HasValue)
                    newModel.YeastId = dto.Yeast.Id.Value;
                newModel.MaloCultureId = dto.MaloCultureId;
                newModel.Target = CreateTargetViewModel(dto.Target, dtoSugarUOMList, dtoTempUOMList);
            }

            if (entriesDto != null)
            {
                foreach (var entry in entriesDto)
                {
                    var e = CreateBatchEntryViewModel(entry);
                    newModel.Entries.Add(e);
                }

                newModel.Summary = CreatBatchSummaryViewModel(entriesDto);
            }
            return newModel;
        }

        public BatchEntryViewModel CreateBatchEntryViewModel(BatchEntry entry)
        {
            if (entry == null)
            {
                return new BatchEntryViewModel();
            }
            else
            {
                var e = new BatchEntryViewModel
                {
                    Id = entry.Id,
                    BatchId = entry.BatchId,
                    Additions = entry.Additions,
                    Bottled = entry.Bottled,
                    Comments = entry.Comments,
                    ActionDateTime = entry.ActionDateTime ?? entry.EntryDateTime,
                    EntryDateTime = entry.EntryDateTime,
                    Filtered = entry.Filtered,
                    pH = entry.pH,
                    Racked = entry.Racked,
                    So2 = entry.So2,
                    Sugar = entry.Sugar,
                    SugarUomId = entry.SugarUom?.Id,
                    SugarUom = entry.SugarUom?.Abbreviation,
                    Ta = entry.Ta,
                    Temp = entry.Temp,
                    TempUomId = entry.TempUom?.Id,
                    TempUom = entry.TempUom?.Abbreviation
                };

                return e;
            }

        }

        public BatchSummaryViewModel CreatBatchSummaryViewModel(IEnumerable<BatchEntry> entriesDto)
        {
            // calculate status data
            var sortedEntries = entriesDto.OrderByDescending(e => e.ActionDateTime).ThenByDescending(e => e.EntryDateTime);
            var model = new BatchSummaryViewModel();

            var tmpEntry = sortedEntries.FirstOrDefault(e => e.Bottled.HasValue && e.Bottled.Value == true);
            model.BottledOnDate = tmpEntry?.ActionDateTime;

            tmpEntry = sortedEntries.FirstOrDefault(e => e.Racked.HasValue && e.Racked.Value == true);
            model.RackedOnDate = tmpEntry?.ActionDateTime;

            tmpEntry = sortedEntries.FirstOrDefault(e => e.Filtered.HasValue && e.Filtered.Value == true);
            model.FilteredOnDate = tmpEntry?.ActionDateTime?.ToLocalTime();

            tmpEntry = sortedEntries.FirstOrDefault(e => e.Sugar.HasValue);
            model.SugarOnDate = tmpEntry?.ActionDateTime?.ToLocalTime();
            model.SugarOnUom = tmpEntry?.SugarUom?.Abbreviation;
            model.SugarOnValue = tmpEntry?.Sugar;

            tmpEntry = sortedEntries.FirstOrDefault(e => e.Temp.HasValue);
            model.TempOnDate = tmpEntry?.ActionDateTime?.ToLocalTime();
            model.TempOnUom = tmpEntry?.TempUom?.Abbreviation;
            model.TempOnValue = tmpEntry?.Temp;

            tmpEntry = sortedEntries.FirstOrDefault(e => e.pH.HasValue);
            model.pHOnDate = tmpEntry?.ActionDateTime?.ToLocalTime();
            model.pHOnValue = tmpEntry?.pH;

            tmpEntry = sortedEntries.FirstOrDefault(e => e.Ta.HasValue);
            model.TaOnDate = tmpEntry?.ActionDateTime?.ToLocalTime();
            model.TaOnValue = tmpEntry?.Ta;

            tmpEntry = sortedEntries.FirstOrDefault(e => e.So2.HasValue);
            model.So2OnDate = tmpEntry?.ActionDateTime?.ToLocalTime();
            model.So2OnValue = tmpEntry?.So2;

            tmpEntry = sortedEntries.FirstOrDefault(e => string.IsNullOrEmpty(e.Comments) == false);
            model.CommentsOnDate = tmpEntry?.ActionDateTime?.ToLocalTime();
            model.CommentsOnValue = tmpEntry?.Comments;

            return model;
        }


        public IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<ICode> dtoList, IEnumerable<ICode> dtoParentList)
        {
            var list = new List<SelectListItem>();
            var group = new SelectListGroup { Name = "" };

            var selectItem = new SelectListItem
            {
                Value = "",
                Text = "Select a " + title,
                Disabled = true,
                Selected = true,
                Group = new SelectListGroup { Disabled = true, Name = "" }
            };
            list.Add(selectItem);

            if (dtoList != null)
            {
                var sortedList = dtoList.OrderBy(c => c.ParentId);
                foreach (var dto in sortedList)
                {
                    selectItem = new SelectListItem
                    {
                        Value = dto.Id?.ToString(CultureInfo.CurrentCulture),
                        Text = dto.Literal
                    };
                    if (dto.ParentId.HasValue && dtoParentList != null)
                    {
                        var parent = dtoParentList.FirstOrDefault(p => p.Id == dto.ParentId.Value);
                        if (group.Name != parent?.Literal)
                            group = new SelectListGroup { Name = parent?.Literal };

                        selectItem.Group = group;
                    }
                    list.Add(selectItem);

                }
            }
            return list;
        }


        public IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<IUnitOfMeasure>? dtoList)
        {
            var list = new List<SelectListItem>();

            var selectItem = new SelectListItem
            {
                Value = "",
                Text = "Select a " + title,
                Disabled = true,
                Selected = true
            };
            list.Add(selectItem);

            if (dtoList != null)
            {
                foreach (var dto in dtoList.OrderBy(c => c.Name))
                {
                    selectItem = new SelectListItem
                    {
                        Value = dto.Id?.ToString(CultureInfo.CurrentCulture),
                        Text = dto.Name
                    };
                    list.Add(selectItem);
                }
            }

            return list;
        }


        public IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<Yeast> dtoList)
        {
            var list = new List<SelectListItem>();
            var group = new SelectListGroup { Name = "" };

            var selectItem = new SelectListItem
            {
                Value = "",
                Text = "Select a " + title,
                Disabled = true,
                Selected = true,
                Group = new SelectListGroup { Disabled = true, Name = "" }
            };
            list.Add(selectItem);

            if (dtoList != null)
            {
                var sortedList = dtoList.OrderBy(c => c.Brand.Literal).ThenBy(c => c.Trademark);
                foreach (var dto in sortedList)
                {
                    selectItem = new SelectListItem
                    {
                        Value = dto.Id?.ToString(CultureInfo.CurrentCulture),
                        Text = dto.Trademark
                    };
                    if (dto.Brand != null)
                    {
                        if (group.Name != dto.Brand.Literal)
                            group = new SelectListGroup { Name = dto.Brand.Literal };

                        selectItem.Group = group;
                    }
                    list.Add(selectItem);
                }
            }
            return list;
        }


        public IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<Domain.MaloCulture> dtoList)
        {
            var list = new List<SelectListItem>();
            var group = new SelectListGroup { Name = "" };

            var selectItem = new SelectListItem
            {
                Value = "",
                Text = "Select a " + title,
                Disabled = true,
                Selected = true,
                Group = new SelectListGroup { Disabled = true, Name = "" }
            };
            list.Add(selectItem);

            if (dtoList != null)
            {
                var sortedList = dtoList.OrderBy(c => c.Brand.Literal).ThenBy(c => c.Trademark);
                foreach (var dto in sortedList)
                {
                    selectItem = new SelectListItem
                    {
                        Value = dto.Id?.ToString(CultureInfo.CurrentCulture),
                        Text = dto.Trademark
                    };
                    if (dto.Brand != null)
                    {
                        if (group.Name != dto.Brand.Literal)
                            group = new SelectListGroup { Name = dto.Brand.Literal };

                        selectItem.Group = group;
                    }
                    list.Add(selectItem);
                }
            }

            return list;
        }


    }
}
