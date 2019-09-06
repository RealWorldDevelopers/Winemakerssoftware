
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using WMS.Business.Shared;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Models.Yeasts
{
    public class Factory : IFactory
    {
        public YeastsViewModel CreateYeastModel(List<ICode> dtoCategoryList, List<ICode> dtoVarietyList, List<Yeast> yeasts)
        {
            var model = new YeastsViewModel
            {
                YeastsGroups = new List<YeastGroupListItemViewModel>()
            };

            int curBrandId = 0;
            YeastGroupListItemViewModel curGroup = null;
            foreach (var y in yeasts.OrderBy(y => y.Brand.Literal).ThenBy(y => y.Trademark))
            {
                if (curBrandId != y.Brand.Id)
                {
                    if (curGroup != null)
                        model.YeastsGroups.Add(curGroup);
                    curGroup = new YeastGroupListItemViewModel
                    {
                        BrandId = y.Brand.Id,
                        GroupName = y.Brand.Literal,
                        Yeasts = new List<YeastListItemViewModel>()
                    };
                    curBrandId = y.Brand.Id;
                }
                var yeastModel = CreateYeastListItemViewModel(y);
                curGroup.Yeasts.Add(yeastModel);
            }

            model.YeastsGroups.Add(curGroup);


            var categories = CreateSelectList("Category", dtoCategoryList, null);
            var varieties = CreateSelectList("Variety", dtoVarietyList, dtoCategoryList);

            model.YeastPairs = varieties
                .OrderBy(v => v.Group.Name)
                .ThenBy(v => v.Text);

            return model;
        }

        public YeastListItemViewModel CreateYeastListItemViewModel(Yeast dto)
        {

            var model = new YeastListItemViewModel
            {
                Id = dto.Id,
                Name = dto.Trademark,
                Style = dto.Style.Literal,
                TempMax = dto.TempMax.HasValue ? dto.TempMax.Value.FormatTempDisplay() : string.Empty, 
                TempMin = dto.TempMin.HasValue ? dto.TempMin.Value.FormatTempDisplay() : string.Empty,
                Alcohol = dto.Alcohol.HasValue ? dto.Alcohol.Value.FormatAsPercent() : string.Empty,  
                Note = dto.Note,
                Display = dto.Note.TruncateForDisplay(100)
            };
            return model;
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
                    Value = dto.Id.ToString(),
                    Text = dto.Literal
                };
                if (dto.ParentId.HasValue && dtoParentList != null)
                {
                    var parent = dtoParentList.Where(p => p.Id == dto.ParentId.Value).FirstOrDefault();
                    if (group.Name != parent?.Literal)
                    {
                        group = new SelectListGroup { Name = parent.Literal };
                        var firstItem = new SelectListItem
                        {
                            Value = parent.Id.ToString(),
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

        public List<YeastPair> CreateYeastPairList(List<Business.Yeast.Dto.YeastPair> dtoList)
        {
            var list = new List<YeastPair>();

            foreach (var dto in dtoList)
            {
                var p = new YeastPair
                {
                    Id = dto.Id,
                    Yeast = dto.Yeast,
                    Category = dto.Category,
                    Variety = dto.Variety,
                    Note = dto.Note,
                    Display = dto.Note.TruncateForDisplay(100)
                };
                list.Add(p);
            }
            return list;
        }

       

    }

}

