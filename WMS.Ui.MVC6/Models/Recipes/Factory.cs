
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Globalization;
using WMS.Communications;
using WMS.Domain;

namespace WMS.Ui.Mvc6.Models.Recipes
{
    public class Factory : IFactory
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly AppSettings _appSettings;

        private readonly Uri _recipeUrl;
        private readonly Uri _imagesUrl;

        private readonly Uri _streamUrl;
        private readonly Uri _streamThumbsUrl;

        public Factory(IServiceProvider serviceProvider, IOptions<AppSettings> appSettings)
        {

            _serviceProvider = serviceProvider;
            _appSettings = appSettings.Value;
            _recipeUrl = new Uri(_appSettings.URLs.RecipesRecipe, UriKind.Relative);
            _imagesUrl = new Uri(_appSettings.URLs.ImageRecipes, UriKind.Relative);
            _streamUrl = new Uri(_appSettings.URLs.Stream, UriKind.Relative);
            _streamThumbsUrl = new Uri(_appSettings.URLs.StreamThumbs, UriKind.Relative);
        }

        public RatingViewModel CreateRatingModel(Rating rating)
        {
            double? ratingValue = 0;
            if (rating != null)
                ratingValue = rating.TotalValue / rating.TotalVotes;

            var model = new RatingViewModel()
            {
                Check = ratingValue > 0 && ratingValue < 1 ? "checked" : "",
                Check1 = ratingValue >= 1 && ratingValue < 1.5 ? "checked" : "",
                Check15 = ratingValue >= 1.5 && ratingValue < 2 ? "checked" : "",
                Check2 = ratingValue >= 2 && ratingValue < 2.5 ? "checked" : "",
                Check25 = ratingValue >= 2.5 && ratingValue < 3 ? "checked" : "",
                Check3 = ratingValue >= 3 && ratingValue < 3.5 ? "checked" : "",
                Check35 = ratingValue >= 3.5 && ratingValue < 4 ? "checked" : "",
                Check4 = ratingValue >= 4 && ratingValue < 4.5 ? "checked" : "",
                Check45 = ratingValue >= 4.5 && ratingValue < 5 ? "checked" : "",
                Check5 = ratingValue >= 5 ? "checked" : ""
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

        public ImageViewModel CreateImageModel(int? id, Uri sourceUrl, Uri thumbUrl, string altTag, string title, string? caption)
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

        public RecipeViewModel CreateRecipeModel(Recipe recipe)
        {
            if (recipe == null)
                throw new ArgumentNullException(nameof(recipe));

            var model = new RecipeViewModel
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Category = recipe.Category != null ? recipe.Category.Literal : string.Empty,
                Variety = recipe.Variety != null ? recipe.Variety.Literal : string.Empty,
                VarietyId = recipe.Variety?.Id,
                Yeast = recipe.Yeast != null ? $"{recipe.Yeast.Brand?.Literal} - {recipe.Yeast.Trademark}" : string.Empty,
                YeastId = recipe.Yeast?.Id,
                Description = recipe.Description,
                Rating = recipe.Rating != null ? CreateRatingModel(recipe.Rating) : null,
                Hits = recipe.Hits != null ? CreateHitCounterModel(recipe.Hits.Value) : null,
                Instructions = recipe.Instructions,
                Ingredients = recipe.Ingredients,
            };

            if (recipe.Target != null)
            {
                model.TargetId = recipe.Target.Id;
                if (recipe.Target.Temp.HasValue)
                    model.Targets.Add($"Fermentation Temp: {recipe.Target.Temp.Value} \x00B0{recipe.Target.TempUom?.Abbreviation}");
                if (recipe.Target.StartSugar.HasValue)
                    model.Targets.Add($"Starting Sugar: {recipe.Target.StartSugar.Value} {recipe.Target.StartSugarUom?.Abbreviation}");
                if (recipe.Target.TA.HasValue)
                    model.Targets.Add($"Acidity: {recipe.Target.TA.Value} g/L");
                if (recipe.Target.pH.HasValue)
                    model.Targets.Add($"pH: {recipe.Target.pH.Value}");
                if (recipe.Target.EndSugar.HasValue)
                    model.Targets.Add($"EndSugar Sugar: {recipe.Target.EndSugar.Value} {recipe.Target.EndSugarUom?.Abbreviation}");
            }


            if (recipe.ImageFiles != null)
            {
                model.Images.Clear();
                foreach (var file in recipe.ImageFiles)
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

        public Task<RecipeListItemViewModel> BuildRecipeListItemModel(Recipe recipe)
        {
            Task<RecipeListItemViewModel> t = Task.Run(() =>
            {
                var recipeUri = new Uri(_recipeUrl + "/" + recipe.Id.ToString(), UriKind.Relative);

                var picUri = new Uri(_imagesUrl + "/default.png", UriKind.Relative);
                if (recipe.ImageFiles?.Count > 0)
                {
                    var displayFileId = recipe.ImageFiles.First().Id;
                    picUri = new Uri("/" + _streamThumbsUrl.ToString() + "/" + displayFileId, UriKind.Relative);
                }

                var model = new RecipeListItemViewModel
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Category = recipe.Category != null ? recipe.Category.Literal : string.Empty,
                    Variety = recipe.Variety != null ? recipe.Variety.Literal : string.Empty,
                    Description = recipe.Description,
                    Rating = recipe.Rating != null ? CreateRatingModel(recipe.Rating) : null,
                    RecipeUrl = recipeUri,
                    PicPath = picUri.ToString()
                };
                return model;
            });
            return t;
        }

        public List<RecipeListItemViewModel> BuildRecipeListItemModels(IEnumerable<Recipe> recipeList)
        {
            var modelList = new List<RecipeListItemViewModel>();

            var recipeStack = new Stack<Recipe>(recipeList.Where(r => r.Enabled == true));

            // Create 1 per core, and then as they finish, create another:   
            List<Task<RecipeListItemViewModel>> tasks = new();

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

        public async Task<AddRecipeViewModel> CreateAddRecipeModel(AddRecipeViewModel? model = null)
        {
            var _categoryAgent = ActivatorUtilities.GetServiceOrCreateInstance<ICategoryAgent>(_serviceProvider);
            var _varietyAgent = ActivatorUtilities.GetServiceOrCreateInstance<IVarietyAgent>(_serviceProvider);
            var _yeastAgent = ActivatorUtilities.GetServiceOrCreateInstance<IYeastAgent>(_serviceProvider);
            var _tempUOMAgent = ActivatorUtilities.GetServiceOrCreateInstance<ITempUOMAgent>(_serviceProvider);
            var _sugarUOMAgent = ActivatorUtilities.GetServiceOrCreateInstance<ISugarUOMAgent>(_serviceProvider);

            var dtoCategoryList = await _categoryAgent.GetCategories().ConfigureAwait(false);
            var dtoVarietyList = await _varietyAgent.GetVarieties().ConfigureAwait(false);
            var dtoYeastList = await _yeastAgent.GetYeasts().ConfigureAwait(false);
            var dtoTempUOMList = await _tempUOMAgent.GetUOMs().ConfigureAwait(false);
            var dtoSugarUOMList = await _sugarUOMAgent.GetUOMs().ConfigureAwait(false);

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

            newModel.Target.TempUOMs = tempUOMs;
            newModel.Target.SugarUOMs = sugarUOMs;

            return newModel;
        }

        public List<SelectListItem> CreateSelectList(string title, IEnumerable<IUnitOfMeasure> dtoList)
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

        public List<SelectListItem> CreateSelectList(string title, IEnumerable<Yeast> dtoList)
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

            return list;
        }

        public List<SelectListItem> CreateSelectList(string title, IEnumerable<ICode> dtoList, IEnumerable<ICode>? dtoParentList)
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

            return list;
        }


    }
}
