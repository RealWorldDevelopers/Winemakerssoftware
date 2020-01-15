﻿
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Models.Yeasts
{
    public class Factory : IFactory
    {
        public YeastsViewModel CreateYeastModel(List<ICode> dtoCategoryList, List<ICode> dtoVarietyList, List<YeastDto> yeasts)
        {
            var model = new YeastsViewModel();

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
                        GroupName = y.Brand.Literal
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

        public YeastListItemViewModel CreateYeastListItemViewModel(YeastDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

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

        public List<YeastPair> CreateYeastPairList(List<Business.Yeast.Dto.YeastPairDto> dtoList)
        {    
            var list = new List<YeastPair>();

            if (dtoList != null)
            {
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
            }
            return list;
        }

    }

}

