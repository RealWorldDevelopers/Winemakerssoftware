
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Business.MaloCulture.Dto;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Mvc.Models.Journal
{
   public class Factory : IFactory
   {
      private readonly AppSettings _appSettings;

      private readonly Uri _batchUrl;

      public Factory(IOptions<AppSettings> appSettings)
      {
         _appSettings = appSettings?.Value;
         _batchUrl = new Uri(_appSettings.URLs.JournalBatch, UriKind.Relative);
      }

      public JournalViewModel CreateJournalModel()
      {
         return new JournalViewModel();
      }

      public Task<BatchListItemViewModel> BuildBatchListItemModel(BatchDto batchDto)
      {
         Task<BatchListItemViewModel> t = Task.Run(() =>
         {
            Uri batchUri = new Uri(_batchUrl + "/" + batchDto.Id.Value.ToString(CultureInfo.CurrentCulture), UriKind.Relative);

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

      public List<BatchListItemViewModel> BuildBatchListItemModels(List<BatchDto> dtoBatchList)
      {
         var modelList = new List<BatchListItemViewModel>();

         var batchStack = new Stack<BatchDto>(dtoBatchList);

         // Create 1 per core, and then as they finish, create another:   
         List<Task<BatchListItemViewModel>> tasks = new List<Task<BatchListItemViewModel>>();

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

      public TargetViewModel CreateTargetViewModel(TargetDto target)
      {
         return CreateTargetViewModel(target, null, null);
      }

      public TargetViewModel CreateTargetViewModel(TargetDto target, List<IUnitOfMeasure> dtoSugarUOMList, List<IUnitOfMeasure> dtoTempUOMList)
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

      public BatchViewModel CreateBatchViewModel(BatchDto dto)
      {
         return CreateBatchViewModel(dto, null, null, null, null, null, null, null, null);
      }

      public BatchViewModel CreateBatchViewModel(BatchDto dto, List<BatchEntryDto> entriesDto, List<ICode> dtoVarietyList, List<ICode> dtoCategoryList, List<YeastDto> dtoYeastList,
         List<MaloCultureDto> cultureList, List<IUnitOfMeasure> dtoVolumeUOMList, List<IUnitOfMeasure> dtoSugarUOMList, List<IUnitOfMeasure> dtoTempUOMList)
      {
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
            newModel.Title = dto.Title;
            newModel.VarietyId = dto.Variety?.Id;
            newModel.Vintage = dto.Vintage;
            newModel.Volume = dto.Volume;
            newModel.VolumeUOM = dto.VolumeUom?.Id;
            newModel.YeastId = dto.Yeast?.Id;
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

      public BatchEntryViewModel CreateBatchEntryViewModel(BatchEntryDto entry)
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

      public BatchSummaryViewModel CreatBatchSummaryViewModel(List<BatchEntryDto> entriesDto)
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


      public List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList)
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
                  Value = dto.Id.ToString(CultureInfo.CurrentCulture),
                  Text = dto.Literal
               };
               if (dto.ParentId.HasValue && dtoParentList != null)
               {
                  var parent = dtoParentList.FirstOrDefault(p => p.Id == dto.ParentId.Value);
                  if (group.Name != parent?.Literal)
                     group = new SelectListGroup { Name = parent.Literal };

                  selectItem.Group = group;
               }
               list.Add(selectItem);

            }
         }
         return list;
      }


      public List<SelectListItem> CreateSelectList(string title, List<IUnitOfMeasure> dtoList)
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
            foreach (var dto in dtoList.OrderBy(c => c.UnitOfMeasure))
            {
               selectItem = new SelectListItem
               {
                  Value = dto.Id.ToString(CultureInfo.CurrentCulture),
                  Text = dto.UnitOfMeasure
               };
               list.Add(selectItem);
            }
         }

         return list;
      }


      public List<SelectListItem> CreateSelectList(string title, List<YeastDto> dtoList)
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
                  Value = dto.Id.ToString(CultureInfo.CurrentCulture),
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


      public List<SelectListItem> CreateSelectList(string title, List<MaloCultureDto> dtoList)
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
                  Value = dto.Id.ToString(CultureInfo.CurrentCulture),
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
