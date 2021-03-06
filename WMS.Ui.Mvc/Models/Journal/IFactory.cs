﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Journal.Dto;
using WMS.Business.MaloCulture.Dto;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Mvc.Models.Journal
{
   public interface IFactory
   {
      JournalViewModel CreateJournalModel();
      TargetViewModel CreateTargetViewModel(TargetDto target);
      TargetViewModel CreateTargetViewModel(TargetDto target, List<IUnitOfMeasure> dtoSugarUOMList, List<IUnitOfMeasure> dtoTempUOMList);
      BatchViewModel CreateBatchViewModel(BatchDto dto, List<BatchEntryDto> entriesDto, List<ICode> dtoVarietyList, List<ICode> dtoCategoryList,
         List<YeastDto> dtoYeastList, List<MaloCultureDto> dtoCultureList, List<IUnitOfMeasure> dtoVolumeUOMList, List<IUnitOfMeasure> dtoSugarUOMList, List<IUnitOfMeasure> dtoTempUOMList);
      BatchViewModel CreateBatchViewModel(BatchDto dto);
      BatchEntryViewModel CreateBatchEntryViewModel(BatchEntryDto entry);
      BatchSummaryViewModel CreatBatchSummaryViewModel(List<BatchEntryDto> entriesDto);
      Task<BatchListItemViewModel> BuildBatchListItemModel(BatchDto batchDto);
      List<BatchListItemViewModel> BuildBatchListItemModels(List<BatchDto> dtoBatchList);
      List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList);
      List<SelectListItem> CreateSelectList(string title, List<IUnitOfMeasure> dtoList);
      List<SelectListItem> CreateSelectList(string title, List<YeastDto> dtoList);
      List<SelectListItem> CreateSelectList(string title, List<MaloCultureDto> dtoList);
   }
}