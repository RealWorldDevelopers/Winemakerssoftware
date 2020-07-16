

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Models.Journal
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

      public Task<BatchListItemViewModel> BuildBatchListItemModel(Business.Journal.Dto.BatchDto batchDto)
      {
         Task<BatchListItemViewModel> t = Task.Run(() =>
         {
            Uri batchUri = new Uri(_batchUrl + "/" + batchDto.Id.Value.ToString(CultureInfo.CurrentCulture), UriKind.Relative);

            var model = new BatchListItemViewModel
            {
               Id = batchDto.Id.Value,
               Title = batchDto.Title,
               Vintage = batchDto.Vintage,
               Variety = batchDto.Variety != null ? batchDto.Variety.Literal : string.Empty,
               Description = batchDto.Description,
               CurrentStage = "CurrentStage",
               LastStatus = "01-01-2020: last SG and Temp or last so2 or last score",
               BatchUrl = batchUri
            };
            return model;
         });
         return t;
      }

      public List<BatchListItemViewModel> BuildBatchListItemModels(List<Business.Journal.Dto.BatchDto> dtoBatchList)
      {
         var modelList = new List<BatchListItemViewModel>();

         var batchStack = new Stack<Business.Journal.Dto.BatchDto>(dtoBatchList);

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
            model.EndingSugar = target.EndSugar;
            model.EndSugarUOM = target.EndSugarUom.Id;
            model.FermentationTemp = target.Temp;
            model.TempUOM = target.TempUom.Id;
            model.StartingSugar = target.StartSugar;
            model.StartSugarUOM = target.StartSugarUom.Id;
            model.TA = target.TA;
            model.pH = target.pH;
         }

         return model;
      }

      public BatchViewModel CreateBatchModel(List<ICode> dtoVarietyList, List<ICode> dtoCategoryList, List<YeastDto> dtoYeastList,
         List<IUnitOfMeasure> dtoVolumeUOMList, List<IUnitOfMeasure> dtoSugarUOMList, List<IUnitOfMeasure> dtoTempUOMList,
         TargetDto target = null)
      {
         var varieties = CreateSelectList("Variety", dtoVarietyList, dtoCategoryList);
         var uomVolumeList = CreateSelectList("Unit of Measure", dtoVolumeUOMList);    
         var yeastsList = CreateSelectList("Yeast", dtoYeastList);

         var newModel = new BatchViewModel
         {
            Varieties = varieties,
            VolumeUOMs = uomVolumeList,
            Yeasts = yeastsList,
            Target = CreateTargetViewModel(target, dtoSugarUOMList, dtoTempUOMList)
         };       

         return newModel;
      }


      public List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList)
      {
         var list = new List<SelectListItem>();
         var group = new SelectListGroup { Name = "" };
         var sortedList = dtoList.OrderBy(c => c.ParentId);

         var selectItem = new SelectListItem
         {
            Value = "",
            Text = "Select a " + title,
            Disabled = true,
            Selected = true,
            Group = new SelectListGroup { Disabled = true, Name = "" }
         };
         list.Add(selectItem);

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
         var sortedList = dtoList.OrderBy(c => c.Brand.Literal).ThenBy(c => c.Trademark);

         var selectItem = new SelectListItem
         {
            Value = "",
            Text = "Select a " + title,
            Disabled = true,
            Selected = true,
            Group = new SelectListGroup { Disabled = true, Name = "" }
         };
         list.Add(selectItem);

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

         return list;
      }


   }
}
