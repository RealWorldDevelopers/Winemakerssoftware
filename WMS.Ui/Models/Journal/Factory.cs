

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WMS.Business.Common;

namespace WMS.Ui.Models.Journal
{
   public class Factory : IFactory
   {
      public JournalViewModel CreateJournalModel()
      {
         return new JournalViewModel();
      }

      public AddBatchViewModel CreateAddBatchModel(List<ICode> dtoVarietyList, List<ICode> dtoCategoryList, 
         List<IUnitOfMeasure> dtoVolumeUOMList, List<IUnitOfMeasure> dtoSugarUOMList, List<IUnitOfMeasure> dtoTempUOMList, AddBatchViewModel model = null)
      {
         var varieties = CreateSelectList("Variety", dtoVarietyList, dtoCategoryList);
         var uomVolumeList = CreateSelectList("Unit of Measure", dtoVolumeUOMList);
         var uomSugarList = CreateSelectList("Unit of Measure", dtoSugarUOMList);
         var uomTempList = CreateSelectList("Unit of Measure", dtoTempUOMList);

         var newModel = model;
         if (newModel == null)
            newModel = new AddBatchViewModel();

         newModel.Varieties = varieties;
         newModel.VolumeUOMs = uomVolumeList;
         newModel.SugarUOMs = uomSugarList;
         newModel.TempUOMs = uomTempList;

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


   }
}
