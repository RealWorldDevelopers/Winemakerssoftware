using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Models.Recipes
{
   public class Factory : IFactory
   {
      private readonly AppSettings _appSettings;

      private readonly Uri _recipeUrl;
      private readonly Uri _imagesUrl;

      private readonly Uri _streamUrl;
      private readonly Uri _streamThumbsUrl;

      public Factory(IOptions<AppSettings> appSettings)
      {
         _appSettings = appSettings?.Value;
         _recipeUrl = new Uri(_appSettings.URLs.RecipesRecipe, UriKind.Relative);
         _imagesUrl = new Uri(_appSettings.URLs.ImageRecipes, UriKind.Relative);
         _streamUrl = new Uri(_appSettings.URLs.Stream, UriKind.Relative);
         _streamThumbsUrl = new Uri(_appSettings.URLs.StreamThumbs, UriKind.Relative);
      }

      public RatingViewModel CreateRatingModel(Business.Recipe.Dto.RatingDto rating)
      {
         double ratingValue = 0;
         if (rating != null)
            ratingValue = rating.TotalValue / rating.TotalVotes;

         var model = new RatingViewModel()
         {
            Check = (ratingValue > 0 && ratingValue < 1) ? "checked" : "",
            Check1 = (ratingValue >= 1 && ratingValue < 1.5) ? "checked" : "",
            Check15 = (ratingValue >= 1.5 && ratingValue < 2) ? "checked" : "",
            Check2 = (ratingValue >= 2 && ratingValue < 2.5) ? "checked" : "",
            Check25 = (ratingValue >= 2.5 && ratingValue < 3) ? "checked" : "",
            Check3 = (ratingValue >= 3 && ratingValue < 3.5) ? "checked" : "",
            Check35 = (ratingValue >= 3.5 && ratingValue < 4) ? "checked" : "",
            Check4 = (ratingValue >= 4 && ratingValue < 4.5) ? "checked" : "",
            Check45 = (ratingValue >= 4.5 && ratingValue < 5) ? "checked" : "",
            Check5 = (ratingValue >= 5) ? "checked" : ""
         };
         return model;
      }

      public HitCounterViewModel CreateHitCounterModel(int hits)
      {
         char[] nlst = hits.ToString(CultureInfo.CurrentCulture).PadLeft(9, '0').ToCharArray();
         var model = new HitCounterViewModel
         {
            Digit9 = nlst[8].ToString(CultureInfo.CurrentCulture),
            Digit8 = nlst[7].ToString(CultureInfo.CurrentCulture),
            Digit7 = nlst[6].ToString(CultureInfo.CurrentCulture),
            Digit6 = nlst[5].ToString(CultureInfo.CurrentCulture),
            Digit5 = nlst[4].ToString(CultureInfo.CurrentCulture),
            Digit4 = nlst[3].ToString(CultureInfo.CurrentCulture),
            Digit3 = nlst[2].ToString(CultureInfo.CurrentCulture),
            Digit2 = nlst[1].ToString(CultureInfo.CurrentCulture),
            Digit1 = nlst[0].ToString(CultureInfo.CurrentCulture)
         };
         return model;
      }

      public ImageViewModel CreateImageModel(int id, Uri sourceUrl, Uri thumbUrl, string altTag, string title, string caption)
      {
         var model = new ImageViewModel
         {
            Id = id,
            Src = sourceUrl,
            SrcThumb = thumbUrl,
            Alt = altTag,
            Title = title,
            Caption = caption
         };

         return model;
      }

      public RecipeViewModel CreateRecipeModel(Business.Recipe.Dto.RecipeDto dto)
      {
         if (dto == null)
            throw new ArgumentNullException(nameof(dto));

         var model = new RecipeViewModel
         {
            Id = dto.Id,
            Title = dto.Title,
            Category = dto.Category != null ? dto.Category.Literal : string.Empty,
            Variety = dto.Variety != null ? dto.Variety.Literal : string.Empty,
            VarietyId = dto.Variety.Id,
            Yeast = dto.Yeast != null ? $"{dto.Yeast.Brand.Literal} - {dto.Yeast.Trademark}" : string.Empty,
            YeastId = dto.Yeast.Id,
            Description = dto.Description,
            Rating = CreateRatingModel(dto.Rating),
            Hits = CreateHitCounterModel(dto.Hits),
            Instructions = dto.Instructions,
            Ingredients = dto.Ingredients,
         };

         if (dto.Target != null)
         {
            model.TargetId = dto.Target.Id;
            if (dto.Target.Temp.HasValue)
               model.Targets.Add($"Fermentation Temp: {dto.Target.Temp.Value} \x00B0{dto.Target.TempUom.Abbreviation}");
            if (dto.Target.StartSugar.HasValue)
               model.Targets.Add($"Starting Sugar: {dto.Target.StartSugar.Value} {dto.Target.StartSugarUom.Abbreviation}");
            if (dto.Target.TA.HasValue)
               model.Targets.Add($"Acidity (Must): {dto.Target.TA.Value} g/L");
            if (dto.Target.pH.HasValue)
               model.Targets.Add($"pH (Must): {dto.Target.pH.Value}");
            if (dto.Target.EndSugar.HasValue)
               model.Targets.Add($"EndSugar Sugar: {dto.Target.EndSugar.Value} {dto.Target.EndSugarUom.Abbreviation}");
         }


         if (dto.ImageFiles != null)
         {
            model.Images.Clear();
            foreach (var file in dto.ImageFiles)
            {
               var src = new Uri("/" + _streamUrl.ToString() + "/" + file.Id, UriKind.Relative);
               var thumb = new Uri("/" + _streamThumbsUrl.ToString() + "/" + file.Id, UriKind.Relative);
               var picModel = CreateImageModel(file.Id, src, thumb, "Recipe Image", "Wine Image", file.Name);
               model.Images.Add(picModel);
            }
         }

         return model;
      }

      public RecipesViewModel CreateRecipesModel()
      {
         return new RecipesViewModel();
      }

      public Task<RecipeListItemViewModel> BuildRecipeListItemModel(Business.Recipe.Dto.RecipeDto recipeDto)
      {
         Task<RecipeListItemViewModel> t = Task.Run(() =>
         {
            Uri recipeUri = new Uri(_recipeUrl + "/" + recipeDto.Id.ToString(CultureInfo.CurrentCulture), UriKind.Relative);

            Uri picUri = new Uri(_imagesUrl + "/default.png", UriKind.Relative);
            if (recipeDto.ImageFiles?.Count > 0)
            {
               var displayFileId = recipeDto.ImageFiles.First().Id;
               picUri = new Uri("/" + _streamThumbsUrl.ToString() + "/" + displayFileId, UriKind.Relative);
            }

            var model = new RecipeListItemViewModel
            {
               Id = recipeDto.Id,
               Title = recipeDto.Title,
               Category = recipeDto.Category != null ? recipeDto.Category.Literal : string.Empty,
               Variety = recipeDto.Variety != null ? recipeDto.Variety.Literal : string.Empty,
               Description = recipeDto.Description,
               Rating = CreateRatingModel(recipeDto.Rating),
               RecipeUrl = recipeUri,
               PicPath = picUri.ToString()
            };
            return model;
         });
         return t;
      }

      public List<RecipeListItemViewModel> BuildRecipeListItemModels(List<Business.Recipe.Dto.RecipeDto> dtoRecipeList)
      {
         var modelList = new List<RecipeListItemViewModel>();

         var recipeStack = new Stack<Business.Recipe.Dto.RecipeDto>(dtoRecipeList.Where(r => r.Enabled == true));

         // Create 1 per core, and then as they finish, create another:   
         List<Task<RecipeListItemViewModel>> tasks = new List<Task<RecipeListItemViewModel>>();

         int numRecipes = recipeStack.Count;
         int numCores = Environment.ProcessorCount;

         // if numCors > N use only N  
         if (numCores > numRecipes)
            numCores = numRecipes;

         // create initial set of tasks:        
         for (int i = 0; i < numCores; i++)
         {
            Task<RecipeListItemViewModel> t = BuildRecipeListItemModel(recipeStack.Pop());
            tasks.Add(t);
         }

         // now, as they finish, create more:
         int done = 0;
         while (done < numRecipes)
         {
            int index = Task.WaitAny(tasks.ToArray());
            done++;
            modelList.Add(tasks[index].Result);
            tasks.RemoveAt(index);
            if (recipeStack.Count > 0)
            {
               Task<RecipeListItemViewModel> t = BuildRecipeListItemModel(recipeStack.Pop());
               tasks.Add(t);
            }
         }

         return modelList;
      }

      public AddRecipeViewModel CreateAddRecipeModel(List<ICode> dtoCategoryList, List<ICode> dtoVarietyList, List<YeastDto> dtoYeastList,
         List<IUnitOfMeasure> dtoSugarUOMList, List<IUnitOfMeasure> dtoTempUOMList,AddRecipeViewModel model = null)
      {
         var categories = CreateSelectList("Category", dtoCategoryList, null);
         var varieties = CreateSelectList("Variety", dtoVarietyList, dtoCategoryList);
         var yeasts = CreateSelectList("Yeast", dtoYeastList);
         var tempUOMs = CreateSelectList("UOM", dtoTempUOMList); ;
         var sugarUOMs = CreateSelectList("UOM", dtoSugarUOMList); ;

         var newModel = model;
         if (newModel == null)
         {
            newModel = new AddRecipeViewModel();
         }

         newModel.Varieties = varieties
           .OrderBy(v => v.Group.Name)
           .ThenBy(v => v.Text);

         newModel.Yeasts = yeasts
           .OrderBy(v => v.Group.Name)
           .ThenBy(v => v.Text);

         newModel.Target = new Journal.TargetViewModel { TempUOMs = tempUOMs, SugarUOMs = sugarUOMs };


         return newModel;
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


   }
}
