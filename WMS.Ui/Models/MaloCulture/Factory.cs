
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WMS.Business.Common;
using WMS.Business.MaloCulture.Dto;

namespace WMS.Ui.Models.MaloCulture
{
   public class Factory : IFactory
   {
      public MaloCulturesViewModel CreateMaloCultureModel(List<ICode> dtoCategoryList, List<ICode> dtoVarietyList, List<MaloCultureDto> MaloCultures)
      {
         var model = new MaloCulturesViewModel();

         int curBrandId = 0;
         MaloCultureGroupListItemViewModel curGroup = null;
         foreach (var y in MaloCultures.OrderBy(y => y.Brand.Literal).ThenBy(y => y.Trademark))
         {
            if (curBrandId != y.Brand.Id)
            {
               if (curGroup != null)
                  model.MaloCulturesGroups.Add(curGroup);
               curGroup = new MaloCultureGroupListItemViewModel
               {
                  BrandId = y.Brand.Id,
                  GroupName = y.Brand.Literal
               };
               curBrandId = y.Brand.Id;
            }
            var MaloCultureModel = CreateMaloCultureListItemViewModel(y);
            curGroup.MaloCultures.Add(MaloCultureModel);
         }

         model.MaloCulturesGroups.Add(curGroup);       
         
         return model;
      }

      public MaloCultureListItemViewModel CreateMaloCultureListItemViewModel(MaloCultureDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var model = new MaloCultureListItemViewModel
         {
            Id = dto.Id,
            Name = dto.Trademark,
            Style = dto.Style.Literal,
            TempMax = dto.TempMax.HasValue ? dto.TempMax.Value.FormatTempDisplay() : string.Empty,
            TempMin = dto.TempMin.HasValue ? dto.TempMin.Value.FormatTempDisplay() : string.Empty,
            Alcohol = dto.Alcohol.HasValue ? dto.Alcohol.Value.FormatAsPercent() : string.Empty,
            pH = dto.pH.HasValue ? dto.pH.Value.ToString(CultureInfo.InvariantCulture) : string.Empty,
            So2 = dto.So2.HasValue ? dto.So2.Value.ToString(CultureInfo.InvariantCulture) : string.Empty,
            Note = dto.Note,
            Display = dto.Note.TruncateForDisplay(100)
         };
         return model;
      }

      public static List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList)
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
               {
                  group = new SelectListGroup { Name = parent.Literal };
                  var firstItem = new SelectListItem
                  {
                     Value = parent.Id.ToString(CultureInfo.CurrentCulture),
                     Text = "All(Any)" + parent.Literal,
                     Group = group
                  };
                  list.Add(firstItem);
               }

               selectItem.Group = group;
            }
            list.Add(selectItem);

         }

         return list;
      }


   }
}
