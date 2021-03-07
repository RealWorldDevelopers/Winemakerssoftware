
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WMS.Business.Recipe.Dto;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;
using WMS.Business.MaloCulture.Dto;
using WMS.Business.Journal.Dto;

namespace WMS.Ui.Models.Admin
{
   public class Factory : IFactory
   {
      private readonly Business.Recipe.Queries.IFactory _recipeQueryFactory;
      private readonly Business.Yeast.Queries.IFactory _yeastQueryFactory;
      private readonly Business.MaloCulture.Queries.IFactory _maloQueryFactory;
      private readonly Business.Journal.Queries.IFactory _journalQueryFactory;

      private readonly IQuery<ICode> _getCategoriesQuery;
      private readonly IQuery<ICode> _getVarietiesQuery;
      private readonly IQuery<YeastDto> _getYeastsQuery;
      private readonly IQuery<YeastPairDto> _getYeastPairsQuery;
      private readonly IQuery<ICode> _getYeastBrandsQuery;
      private readonly IQuery<ICode> _getYeastStylesQuery;

      private readonly IQuery<MaloCultureDto> _getMaloQuery;
      private readonly IQuery<ICode> _getMaloBrandsQuery;
      private readonly IQuery<ICode> _getMaloStylesQuery;

      private readonly IQuery<IUnitOfMeasure> _getSugarUOMQuery;
      private readonly IQuery<IUnitOfMeasure> _getTempUOMQuery;
      private readonly IQuery<IUnitOfMeasure> _getVolumeUOMQuery;

      private readonly List<ICode> _categoriesDtoList;
      private readonly List<ICode> _varietiesDtoList;
      private readonly List<ICode> _yeastBrandsDtoList;
      private readonly List<ICode> _yeastStylesDtoList;

      private readonly List<MaloCultureDto> _maloDtoList;
      private readonly List<ICode> _maloBrandsDtoList;
      private readonly List<ICode> _maloStylesDtoList;
      private readonly List<YeastDto> _yeastsDtoList;
      private readonly List<YeastPairDto> _yeastPairingsDtoList;

      private readonly List<IUnitOfMeasure> _getSugarUomList;
      private readonly List<IUnitOfMeasure> _getTempUomList;
      private readonly List<IUnitOfMeasure> _getVolumeUomList;

      public Factory(Business.Recipe.Queries.IFactory recipeQueryFactory, Business.Yeast.Queries.IFactory yeastQueryFactory,
         Business.MaloCulture.Queries.IFactory maloQueryFactory, Business.Journal.Queries.IFactory journalQueryFactory)
      {
         _recipeQueryFactory = recipeQueryFactory ?? throw new ArgumentNullException(nameof(recipeQueryFactory));
         _yeastQueryFactory = yeastQueryFactory ?? throw new ArgumentNullException(nameof(yeastQueryFactory));
         _journalQueryFactory = journalQueryFactory ?? throw new ArgumentNullException(nameof(journalQueryFactory));
         _maloQueryFactory = maloQueryFactory ?? throw new ArgumentNullException(nameof(maloQueryFactory));

         _getCategoriesQuery = _recipeQueryFactory.CreateCategoriesQuery();
         _getVarietiesQuery = _recipeQueryFactory.CreateVarietiesQuery();
         _getYeastsQuery = _yeastQueryFactory.CreateYeastsQuery();
         _getYeastPairsQuery = _yeastQueryFactory.CreateYeastPairQuery();
         _getYeastBrandsQuery = _yeastQueryFactory.CreateBrandsQuery();
         _getYeastStylesQuery = _yeastQueryFactory.CreateStylesQuery();
         _getMaloQuery = _maloQueryFactory.CreateMaloCulturesQuery();
         _getMaloBrandsQuery = _maloQueryFactory.CreateBrandsQuery();
         _getMaloStylesQuery = _maloQueryFactory.CreateStylesQuery();
         _getSugarUOMQuery = _journalQueryFactory.CreateBatchSugarUOMQuery();
         _getTempUOMQuery = _journalQueryFactory.CreateBatchTempUOMQuery();
         _getVolumeUOMQuery = _journalQueryFactory.CreateBatchVolumeUOMQuery();

         _categoriesDtoList = _getCategoriesQuery.Execute();
         _varietiesDtoList = _getVarietiesQuery.Execute();
         _yeastsDtoList = _getYeastsQuery.Execute();
         _yeastPairingsDtoList = _getYeastPairsQuery.Execute();
         _yeastBrandsDtoList = _getYeastBrandsQuery.Execute();
         _yeastStylesDtoList = _getMaloStylesQuery.Execute();

         _maloDtoList = _getMaloQuery.Execute();
         _maloBrandsDtoList = _getMaloBrandsQuery.Execute();
         _maloStylesDtoList = _getYeastStylesQuery.Execute();

         _getSugarUomList = _getSugarUOMQuery.Execute();
         _getTempUomList = _getTempUOMQuery.Execute();
         _getVolumeUomList = _getVolumeUOMQuery.Execute();
      }

      public AdminViewModel CreateAdminModel(string startingTab = null)
      {
         var model = new AdminViewModel();
         if (!string.IsNullOrWhiteSpace(startingTab))
            model.TabToShow = startingTab;
         model.RolesViewModel = new RolesViewModel();
         model.UsersViewModel = new UsersViewModel();
         model.CategoriesViewModel = new CategoriesViewModel();
         model.VarietiesViewModel = new VarietiesViewModel();
         model.YeastsViewModel = new YeastsViewModel();
         model.MaloCulturesViewModel = new MaloCulturesViewModel();
         model.RecipesViewModel = new RecipesViewModel();
         model.JournalsViewModel = new JournalsViewModel();

         return model;
      }

      public RecipeViewModel CreateRecipeViewModel()
      {
         var model = new RecipeViewModel
         {
            Target = new TargetViewModel(),
            Yeast = new YeastViewModel(),
            Category = new CategoryViewModel(),
            Variety = new VarietyViewModel()
         };
         model.Varieties.Clear();
         model.Varieties.AddRange(CreateSelectList("Variety", _varietiesDtoList, _categoriesDtoList, null));
         model.Yeasts.Clear();
         model.Yeasts.AddRange(CreateSelectList("Yeast", _yeastsDtoList));
         return model;
      }

      public RecipeViewModel CreateRecipeViewModel(RecipeDto recipeDto)
      {
         if (recipeDto == null)
            throw new ArgumentNullException(nameof(recipeDto));

         var target = new TargetViewModel();

         if (recipeDto.Target != null)
            target = new TargetViewModel
            {
               Id = recipeDto.Target.Id,
               EndingSugar = recipeDto.Target.EndSugar,
               EndSugarUOM = recipeDto.Target.EndSugarUom?.Id,
               FermentationTemp = recipeDto.Target.Temp,
               TempUOM = recipeDto.Target.TempUom?.Id,
               pH = recipeDto.Target.pH,
               StartingSugar = recipeDto.Target.StartSugar,
               StartSugarUOM = recipeDto.Target.StartSugarUom?.Id,
               TA = recipeDto.Target.TA
            };

         target.SugarUOMs = CreateSelectList("UOM", _getSugarUomList);
         target.TempUOMs = CreateSelectList("UOM", _getTempUomList);

         var model = new RecipeViewModel
         {
            Id = recipeDto.Id,
            Hits = recipeDto.Hits,
            Enabled = recipeDto.Enabled,
            NeedsApproved = recipeDto.NeedsApproved,
            SubmittedBy = recipeDto.SubmittedBy,
            Category = CreateCategoryViewModel(recipeDto.Category),
            Variety = CreateVarietyViewModel(recipeDto.Variety, null),
            Title = recipeDto.Title,
            Yeast = CreateYeastViewModel(recipeDto.Yeast),
            Target = target,
            Ingredients = recipeDto.Ingredients,
            Instructions = recipeDto.Instructions,
            Description = recipeDto.Description
         };

         model.Images.Clear();
         model.Images.AddRange(CreateImageViewModel(recipeDto.ImageFiles));
         model.Varieties.Clear();
         model.Varieties.AddRange(CreateSelectList("Variety", _varietiesDtoList, _categoriesDtoList, null));
         model.Yeasts.Clear();
         model.Yeasts.AddRange(CreateSelectList("Yeast", _yeastsDtoList));

         if (recipeDto.Rating != null)
            model.Rating = Math.Round(recipeDto.Rating.TotalValue / recipeDto.Rating.TotalVotes, 2);

         return model;
      }

      public List<RecipeViewModel> CreateRecipeViewModel(List<RecipeDto> recipeDtoList)
      {
         var models = new List<RecipeViewModel>();
         foreach (var recipe in recipeDtoList.OrderBy(r => r.Enabled).ThenBy(r => r.NeedsApproved).
             ThenBy(r => r.Category.Literal).ThenBy(r => r.Variety.Literal).ThenBy(r => r.Description))
         {
            models.Add(CreateRecipeViewModel(recipe));
         }
         return models;
      }


      public JournalViewModel CreateJournalViewModel()
      {
         var model = new JournalViewModel
         {
            Target = new TargetViewModel()
         };
         model.Varieties.Clear();
         model.Varieties.AddRange(CreateSelectList("Variety", _varietiesDtoList, _categoriesDtoList, null));
         model.Yeasts.Clear();
         model.Yeasts.AddRange(CreateSelectList("Yeast", _yeastsDtoList));
         return model;
      }

      public JournalViewModel CreateJournalViewModel(BatchDto batchDto, UserViewModel user)
      {
         if (batchDto == null)
            throw new ArgumentNullException(nameof(batchDto));

         var target = new TargetViewModel();
         if (batchDto.Target != null)
            target = new TargetViewModel
            {
               Id = batchDto.Target.Id,
               EndingSugar = batchDto.Target.EndSugar,
               EndSugarUOM = batchDto.Target.EndSugarUom?.Id,
               FermentationTemp = batchDto.Target.Temp,
               TempUOM = batchDto.Target.TempUom?.Id,
               pH = batchDto.Target.pH,
               StartingSugar = batchDto.Target.StartSugar,
               StartSugarUOM = batchDto.Target.StartSugarUom?.Id,
               TA = batchDto.Target.TA
            };

         target.SugarUOMs = CreateSelectList("UOM", _getSugarUomList);
         target.TempUOMs = CreateSelectList("UOM", _getTempUomList);

         var model = new JournalViewModel
         {
            Id = batchDto.Id,
            Complete = batchDto.Complete ?? false,
            Description = batchDto.Description,
            MaloCultureId = batchDto.MaloCultureId,
            RecipeId = batchDto.RecipeId,
            Title = batchDto.Title,
            Target = target,
            Variety = CreateVarietyViewModel(batchDto.Variety, null),
            Vintage = batchDto.Vintage,
            Volume = batchDto.Volume,
            VolumeUOM = batchDto.VolumeUom.Id,
            Yeast = batchDto.Yeast != null ? CreateYeastViewModel(batchDto.Yeast) : CreateYeastViewModel(),
            SubmittedBy = user?.UserName
         };


         model.Entries.Clear();
         foreach (var entry in batchDto.Entries)
         {
            var e = CreateBatchEntryViewModel(entry);
            model.Entries.Add(e);
         }


         model.MaloCultures.Clear();
         model.MaloCultures.AddRange(CreateSelectList("Culture", _maloDtoList)); // MaloCultures = null;

         model.Varieties.Clear();
         model.Varieties.AddRange(CreateSelectList("Variety", _varietiesDtoList, _categoriesDtoList, null));
         model.Yeasts.Clear();
         model.Yeasts.AddRange(CreateSelectList("Yeast", _yeastsDtoList));

         model.VolumeUOMs.Clear();
            model.VolumeUOMs.AddRange(CreateSelectList("UOM", _getVolumeUomList)); 

         return model;
      }

      public List<JournalViewModel> CreateJournalViewModel(List<BatchDto> batchDtoList, List<UserViewModel> users)
      {
         var models = new List<JournalViewModel>();
         foreach (var batch in batchDtoList.OrderBy(r => r.SubmittedBy)
            .ThenByDescending(r => r.Vintage)
            .ThenByDescending(r => r.Description))
         {
            models.Add(CreateJournalViewModel(batch, users.FirstOrDefault(u => u.Id == batch.SubmittedBy)));
         }
         return models;
      }

      public JournalEntryViewModel CreateBatchEntryViewModel(BatchEntryDto entry)
      {
         if (entry == null)
         {
            return new JournalEntryViewModel();
         }
         else
         {
            var e = new JournalEntryViewModel
            {
               Id = entry.Id,
               BatchId = entry.BatchId,
               Additions = entry.Additions,
               Bottled = entry.Bottled,
               Comments = entry.Comments,
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

            if (entry.ActionDateTime.HasValue)
               e.ActionDateTime = entry.ActionDateTime.Value.ToLocalTime();
            else
               e.ActionDateTime = entry.EntryDateTime.Value.ToLocalTime();

            return e;
         }

      }


      public ImageViewModel CreateImageViewModel(ImageFileDto imageDto)
      {
         if (imageDto == null)
            throw new ArgumentNullException(nameof(imageDto));

         var model = new ImageViewModel(imageDto.Data())
         {
            Id = imageDto.Id,
            ContentType = imageDto.ContentType,
            FileName = imageDto.FileName,
            Length = imageDto.Length,
            Name = imageDto.Name,
            RecipeId = imageDto.RecipeId
         };

         return model;
      }

      public List<ImageViewModel> CreateImageViewModel(List<ImageFileDto> imageDtoList)
      {
         var models = new List<ImageViewModel>();

         if (imageDtoList != null)
         {
            foreach (var img in imageDtoList)
            {
               models.Add(CreateImageViewModel(img));
            }
         }
         return models;
      }

      public CategoryViewModel CreateCategoryViewModel(ICode categoryDto)
      {
         if (categoryDto == null)
            throw new ArgumentNullException(nameof(categoryDto));

         var model = new CategoryViewModel
         {
            Description = categoryDto.Description,
            Enabled = categoryDto.Enabled,
            Id = categoryDto.Id,
            Literal = categoryDto.Literal
         };


         var children = _varietiesDtoList.Where(v => v.ParentId == categoryDto.Id);
         if (children != null)
         {
            model.Varieties.Clear();
            foreach (var dto in children)
            {
               var v = new VarietyViewModel
               {
                  Description = dto.Description,
                  Enabled = dto.Enabled,
                  Id = dto.Id,
                  Literal = dto.Literal
               };
               v.Categories.Clear();
               v.Categories.AddRange(CreateSelectList("Parent Category", _categoriesDtoList));

               v.Parent = new CategoryViewModel
               {
                  Description = categoryDto.Description,
                  Enabled = categoryDto.Enabled,
                  Id = categoryDto.Id,
                  Literal = categoryDto.Literal
               };
               model.Varieties.Add(v);
            }
         }

         return model;
      }

      public List<CategoryViewModel> CreateCategoryViewModel(List<ICode> catDtoList)
      {
         var models = new List<CategoryViewModel>();
         foreach (var cat in catDtoList.OrderBy(c => c.Literal))
         {
            models.Add(CreateCategoryViewModel(cat));
         }
         return models;
      }

      public VarietyViewModel CreateVarietyViewModel()
      {
         var model = new VarietyViewModel();
         model.Categories.AddRange(CreateSelectList("Parent Category", _categoriesDtoList));
         return model;
      }

      public VarietyViewModel CreateVarietyViewModel(ICode varietyDto, ICode parentDto)
      {
         if (varietyDto == null)
            throw new ArgumentNullException(nameof(varietyDto));

         var model = new VarietyViewModel
         {
            Description = varietyDto.Description,
            Enabled = varietyDto.Enabled,
            Id = varietyDto.Id,
            Literal = varietyDto.Literal
         };
         model.Categories.AddRange(CreateSelectList("Parent Category", _categoriesDtoList));

         if (parentDto != null)
            model.Parent = CreateCategoryViewModel(parentDto);

         return model;
      }

      public List<VarietyViewModel> CreateVarietyViewModel(List<ICode> varDtoList)
      {
         var models = new List<VarietyViewModel>();
         foreach (var variety in varDtoList.OrderBy(c => c.Literal))
         {
            var parent = _categoriesDtoList.FirstOrDefault(c => c.Id == variety.ParentId);
            models.Add(CreateVarietyViewModel(variety, parent));
         }
         return models;
      }




      public MaloCultureViewModel CreateMaloCultureViewModel()
      {
         var model = new MaloCultureViewModel();
         model.Brands.AddRange(CreateSelectList("Brand", _maloBrandsDtoList));
         model.Styles.AddRange(CreateSelectList("Style", _maloStylesDtoList));
         return model;
      }

      public MaloCultureViewModel CreateMaloCultureViewModel(MaloCultureDto maloCultureDto)
      {
         if (maloCultureDto == null)
            throw new ArgumentNullException(nameof(maloCultureDto));

         var model = new MaloCultureViewModel
         {
            Id = maloCultureDto.Id,
            Brand = CreateMaloBrandViewModel(maloCultureDto.Brand),
            Style = CreateMaloStyleViewModel(maloCultureDto.Style),
            Trademark = maloCultureDto.Trademark,
            TempMax = maloCultureDto.TempMax,
            TempMin = maloCultureDto.TempMin,
            Alcohol = maloCultureDto.Alcohol,
            pH = maloCultureDto.pH,
            SO2 = maloCultureDto.So2,
            Note = maloCultureDto.Note
         };
         model.Brands.AddRange(CreateSelectList("Brand", _maloBrandsDtoList));
         model.Styles.AddRange(CreateSelectList("Style", _maloStylesDtoList));

         return model;
      }

      public List<MaloCultureViewModel> CreateMaloCultureViewModel(List<MaloCultureDto> maloDtoList)
      {
         var models = new List<MaloCultureViewModel>();
         foreach (var dto in maloDtoList.OrderBy(m => m.Brand.Literal).ThenBy(m => m.Trademark))
         {
            models.Add(CreateMaloCultureViewModel(dto));
         }
         return models;
      }

      public MaloBrandViewModel CreateMaloBrandViewModel(ICode brandDto)
      {
         if (brandDto == null)
            throw new ArgumentNullException(nameof(brandDto));

         var model = new MaloBrandViewModel
         {
            Description = brandDto.Description,
            Enabled = brandDto.Enabled,
            Id = brandDto.Id,
            Literal = brandDto.Literal
         };
         return model;
      }

      public List<MaloBrandViewModel> CreateMaloBrandViewModel(List<ICode> brandDtoList)
      {
         var models = new List<MaloBrandViewModel>();
         if (brandDtoList != null)
         {
            foreach (var brand in brandDtoList)
            {
               models.Add(CreateMaloBrandViewModel(brand));
            }
         }
         return models;
      }

      public MaloStyleViewModel CreateMaloStyleViewModel(ICode styleDto)
      {
         if (styleDto == null)
            throw new ArgumentNullException(nameof(styleDto));

         var model = new MaloStyleViewModel
         {
            Description = styleDto.Description,
            Enabled = styleDto.Enabled,
            Id = styleDto.Id,
            Literal = styleDto.Literal
         };
         return model;
      }

      public List<MaloStyleViewModel> CreateMaloStyleViewModel(List<ICode> styleDtoList)
      {
         var models = new List<MaloStyleViewModel>();
         if (styleDtoList != null)
         {
            foreach (var style in styleDtoList)
            {
               models.Add(CreateMaloStyleViewModel(style));
            }
         }
         return models;
      }


      public YeastViewModel CreateYeastViewModel()
      {
         var model = new YeastViewModel();
         model.Brands.AddRange(CreateSelectList("Brand", _yeastBrandsDtoList));
         model.Styles.AddRange(CreateSelectList("Style", _yeastStylesDtoList));
         return model;
      }

      public YeastViewModel CreateYeastViewModel(YeastDto yeastDto)
      {
         if (yeastDto == null)
            throw new ArgumentNullException(nameof(yeastDto));

         var model = new YeastViewModel
         {
            Id = yeastDto.Id,
            Brand = CreateYeastBrandViewModel(yeastDto.Brand),
            Style = CreateYeastStyleViewModel(yeastDto.Style),
            Trademark = yeastDto.Trademark,
            TempMax = yeastDto.TempMax,
            TempMin = yeastDto.TempMin,
            Alcohol = yeastDto.Alcohol,
            Note = yeastDto.Note
         };
         model.Brands.AddRange(CreateSelectList("Brand", _yeastBrandsDtoList));
         model.Styles.AddRange(CreateSelectList("Style", _yeastStylesDtoList));

         var dto = new YeastPairDto { Yeast = yeastDto.Id };
         model.Pairing = CreateYeastPairingViewModel(dto);
         model.Pairing.Category = null;
         model.Pairing.Variety = null;

         var pairs = _yeastPairingsDtoList.Where(p => p.Yeast.Value == yeastDto.Id).ToList();
         if (pairs != null)
         {
            model.Pairings.Clear();
            model.Pairings.AddRange(CreateYeastPairingViewModel(pairs));
         }

         return model;
      }

      public List<YeastViewModel> CreateYeastViewModel(List<YeastDto> yeastDtoList)
      {
         var models = new List<YeastViewModel>();
         foreach (var yeast in yeastDtoList.OrderBy(y => y.Brand.Literal).ThenBy(y => y.Trademark))
         {
            models.Add(CreateYeastViewModel(yeast));
         }
         return models;
      }

      public YeastBrandViewModel CreateYeastBrandViewModel(ICode brandDto)
      {
         if (brandDto == null)
            throw new ArgumentNullException(nameof(brandDto));

         var model = new YeastBrandViewModel
         {
            Description = brandDto.Description,
            Enabled = brandDto.Enabled,
            Id = brandDto.Id,
            Literal = brandDto.Literal
         };
         return model;
      }

      public List<YeastBrandViewModel> CreateYeastBrandViewModel(List<ICode> brandDtoList)
      {
         var models = new List<YeastBrandViewModel>();
         if (brandDtoList != null)
         {
            foreach (var brand in brandDtoList)
            {
               models.Add(CreateYeastBrandViewModel(brand));
            }
         }
         return models;
      }

      public YeastStyleViewModel CreateYeastStyleViewModel(ICode styleDto)
      {
         if (styleDto == null)
            throw new ArgumentNullException(nameof(styleDto));

         var model = new YeastStyleViewModel
         {
            Description = styleDto.Description,
            Enabled = styleDto.Enabled,
            Id = styleDto.Id,
            Literal = styleDto.Literal
         };
         return model;
      }

      public List<YeastStyleViewModel> CreateYeastStyleViewModel(List<ICode> styleDtoList)
      {
         var models = new List<YeastStyleViewModel>();
         if (styleDtoList != null)
         {
            foreach (var style in styleDtoList)
            {
               models.Add(CreateYeastStyleViewModel(style));
            }
         }
         return models;
      }

      public List<YeastPairingViewModel> CreateYeastPairingViewModel(List<YeastPairDto> pairingDtoList)
      {
         var models = new List<YeastPairingViewModel>();
         if (pairingDtoList != null)
         {
            foreach (var p in pairingDtoList)
            {
               models.Add(CreateYeastPairingViewModel(p));
            }
         }
         return models.OrderBy(p => p.Category.Literal).ThenBy(p => p.Variety.Literal).ToList();
      }

      public YeastPairingViewModel CreateYeastPairingViewModel(YeastPairDto pairingDto)
      {
         if (pairingDto == null)
            throw new ArgumentNullException(nameof(pairingDto));

         var yeastDto = _yeastsDtoList.FirstOrDefault(y => y.Id == pairingDto.Yeast.Value);
         var yeastViewModel = new YeastViewModel
         {
            Id = yeastDto.Id,
            Brand = CreateYeastBrandViewModel(yeastDto.Brand),
            Style = CreateYeastStyleViewModel(yeastDto.Style),
            Trademark = yeastDto.Trademark,
            TempMax = yeastDto.TempMax,
            TempMin = yeastDto.TempMin,
            Alcohol = yeastDto.Alcohol,
            Note = yeastDto.Note
         };
         var yeastModel = yeastViewModel;

         var categoryModel = new CategoryViewModel();
         if (pairingDto.Category.HasValue)
         {
            var categoryDto = _categoriesDtoList.FirstOrDefault(c => c.Id == pairingDto.Category.Value);
            if (categoryDto != null)
               categoryModel = CreateCategoryViewModel(categoryDto);
         }

         var varietyModel = new VarietyViewModel();
         if (pairingDto.Variety.HasValue)
         {
            var varietyDto = _varietiesDtoList.FirstOrDefault(v => v.Id == pairingDto.Variety.Value);
            varietyModel = CreateVarietyViewModel(varietyDto, null);
         }

         var existingPairings = _yeastPairingsDtoList.Where(p => p.Yeast.Value == yeastModel.Id && p.Id != pairingDto.Id).ToList();
         var model = new YeastPairingViewModel
         {
            Id = pairingDto.Id,
            Yeast = yeastModel,
            Category = categoryModel,
            Variety = varietyModel,
            Note = pairingDto.Note
         };
         model.Varieties.AddRange(CreateSelectList("Variety", _varietiesDtoList, _categoriesDtoList, existingPairings));

         return model;
      }





      public List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList)
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
            foreach (var dto in dtoList.OrderBy(c => c.Literal))
            {
               selectItem = new SelectListItem
               {
                  Value = dto.Id.ToString(CultureInfo.CurrentCulture),
                  Text = dto.Literal
               };
               list.Add(selectItem);
            }
         }

         return list;
      }

      public List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList, List<YeastPairDto> existingPairings)
      {
         var list = new List<SelectListItem>();
         var group = new SelectListGroup { Name = "" };
         var sortedList = dtoList.OrderBy(c => c.ParentId).ThenBy(c => c.Literal);

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
            var disable = false;
            if (existingPairings != null)
            {
               if (existingPairings.Where(x => x.Variety.HasValue).Any(x => x.Variety.Value == dto.Id))
                  disable = true;
               else
                  disable = false;
            }

            selectItem = new SelectListItem
            {
               Disabled = disable,
               Value = dto.Id.ToString(CultureInfo.CurrentCulture),
               Text = dto.Literal
            };

            if (dto.ParentId.HasValue && dtoParentList != null)
            {
               var parent = dtoParentList.FirstOrDefault(p => p.Id == dto.ParentId.Value);
               if (group.Name != parent?.Literal)
               {
                  disable = false;
                  if (existingPairings != null)
                  {
                     if (existingPairings.Where(x => !x.Variety.HasValue).Any(x => x.Category.Value == parent.Id))
                        disable = true;
                     else
                        disable = false;
                  }
                  group = new SelectListGroup { Name = parent.Literal };
                  var firstItem = new SelectListItem
                  {
                     Disabled = disable,
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

      public List<SelectListItem> CreateSelectList(string title, List<MaloCultureDto> dtoList)
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
