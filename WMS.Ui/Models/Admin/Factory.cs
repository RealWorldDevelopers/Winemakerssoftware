
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS.Business.Recipe.Dto;
using WMS.Business.Shared;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Models.Admin
{
    public class Factory : IFactory
    {
        private readonly Business.Recipe.Queries.IFactory _recipeQueryFactory;
        private readonly Business.Yeast.Queries.IFactory _yeastQueryFactory;

        private readonly IQuery<ICode> _getCategoriesQuery;
        private readonly IQuery<ICode> _getVarietiesQuery;
        private readonly IQuery<Yeast> _getYeastsQuery;
        private readonly IQuery<YeastPair> _getYeastPairsQuery;
        private readonly IQuery<ICode> _getBrandsQuery;
        private readonly IQuery<ICode> _getStylesQuery;

        private readonly List<ICode> _categoriesDtoList;
        private readonly List<ICode> _varietiesDtoList;
        private readonly List<ICode> _brandsDtoList;
        private readonly List<ICode> _stylesDtoList;
        private readonly List<Yeast> _yeastsDtoList;
        private readonly List<YeastPair> _yeastPairingsDtoList;

        public Factory(Business.Recipe.Queries.IFactory recipeQueryFactory, Business.Yeast.Queries.IFactory yeastQueryFactory)
        {
            _recipeQueryFactory = recipeQueryFactory;
            _yeastQueryFactory = yeastQueryFactory;

            _getCategoriesQuery = _recipeQueryFactory.CreateCategoriesQuery();
            _getVarietiesQuery = _recipeQueryFactory.CreateVarietiesQuery();
            _getYeastsQuery = _yeastQueryFactory.CreateYeastsQuery();
            _getYeastPairsQuery = _yeastQueryFactory.CreateYeastPairQuery();
            _getBrandsQuery = _yeastQueryFactory.CreateBrandsQuery();
            _getStylesQuery = _yeastQueryFactory.CreateStylesQuery();

            _categoriesDtoList = _getCategoriesQuery.Execute();
            _varietiesDtoList = _getVarietiesQuery.Execute();
            _yeastsDtoList = _getYeastsQuery.Execute();
            _yeastPairingsDtoList = _getYeastPairsQuery.Execute();
            _brandsDtoList = _getBrandsQuery.Execute();
            _stylesDtoList = _getStylesQuery.Execute();
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
            model.RecipesViewModel = new RecipesViewModel();

            return model;
        }

        public RecipeViewModel CreateRecipeViewModel()
        {
            var model = new RecipeViewModel();
            model.Category = new CategoryViewModel();
            model.Variety = new VarietyViewModel();
            model.Images = new List<ImageViewModel>();
            model.Varieties = CreateSelectList("Variety", _varietiesDtoList, _categoriesDtoList, null);
            return model;
        }

        public RecipeViewModel CreateRecipeViewModel(Recipe recipeDto)
        {
            var model = new RecipeViewModel();
            model.Id = recipeDto.Id;
            model.Hits = recipeDto.Hits;
            model.Enabled = recipeDto.Enabled;
            model.NeedsApproved = recipeDto.NeedsApproved;
            if (recipeDto.Rating != null)
                model.Rating = Math.Round(recipeDto.Rating.TotalValue / recipeDto.Rating.TotalVotes, 2);
            model.SubmittedBy = recipeDto.SubmittedBy;
            model.Category = CreateCategoryViewModel(recipeDto.Category);
            model.Variety = CreateVarietyViewModel(recipeDto.Variety, null);
            model.Title = recipeDto.Title;
            model.Ingredients = recipeDto.Ingredients;
            model.Instructions = recipeDto.Instructions;
            model.Description = recipeDto.Description;
            model.Images = CreateImageViewModel(recipeDto.ImageFiles);  
            model.Varieties = CreateSelectList("Variety", _varietiesDtoList, _categoriesDtoList, null);
            return model;
        }

        public List<RecipeViewModel> CreateRecipeViewModel(List<Recipe> recipeDtoList)
        {
            var models = new List<RecipeViewModel>();
            foreach (var recipe in recipeDtoList.OrderBy(r => r.Enabled).ThenBy(r=>r.NeedsApproved).
                ThenBy(r => r.Category.Literal).ThenBy(r => r.Variety.Literal).ThenBy(r => r.Description))
            {
                models.Add(CreateRecipeViewModel(recipe));
            }
            return models;
        }


        public ImageViewModel CreateImageViewModel(ImageFile imageDto)
        {
            var model = new ImageViewModel
            {
                Id=imageDto.Id,
                ContentType= imageDto.ContentType,
                Data=imageDto.Data,
                FileName=imageDto.FileName,
                Length=imageDto.Length,
                Name=imageDto.Name,
                RecipeId=imageDto.RecipeId
            };

            return model;
        }

        public List<ImageViewModel> CreateImageViewModel(List<ImageFile> imageDtoList)
        {
            var models = new List<ImageViewModel>();
            foreach (var img in imageDtoList)
            {
                models.Add(CreateImageViewModel(img));
            }
            return models;
        }

        public CategoryViewModel CreateCategoryViewModel(ICode categoryDto)
        {
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
                model.Varieties = new List<VarietyViewModel>();
                foreach (var dto in children)
                {
                    var v = new VarietyViewModel
                    {
                        Description = dto.Description,
                        Enabled = dto.Enabled,
                        Id = dto.Id,
                        Literal = dto.Literal,
                        Categories = CreateSelectList("Parent Category", _categoriesDtoList)
                    };
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
            var model = new VarietyViewModel
            {
                Categories = CreateSelectList("Parent Category", _categoriesDtoList)
            };
            return model;
        }

        public VarietyViewModel CreateVarietyViewModel(ICode varietyDto, ICode parentDto)
        {
            var model = new VarietyViewModel
            {
                Description = varietyDto.Description,
                Enabled = varietyDto.Enabled,
                Id = varietyDto.Id,
                Literal = varietyDto.Literal,
                Categories = CreateSelectList("Parent Category", _categoriesDtoList)
            };
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



        public YeastViewModel CreateYeastViewModel()
        {
            var model = new YeastViewModel
            {
                Brands = CreateSelectList("Brand", _brandsDtoList),
                Styles = CreateSelectList("Style", _stylesDtoList)
            };
            return model;
        }

        public YeastViewModel CreateYeastViewModel(Yeast yeastDto)
        {
            var model = new YeastViewModel
            {
                Id = yeastDto.Id,
                Brand = CreateYeastBrandViewModel(yeastDto.Brand),
                Style = CreateYeastStyleViewModel(yeastDto.Style),
                Trademark = yeastDto.Trademark,
                TempMax = yeastDto.TempMax,
                TempMin = yeastDto.TempMin,
                Alcohol = yeastDto.Alcohol,
                Note = yeastDto.Note,
                Brands = CreateSelectList("Brand", _brandsDtoList),
                Styles = CreateSelectList("Style", _stylesDtoList),

            };

            var dto = new YeastPair { Yeast = yeastDto.Id };
            model.Pairing = CreateYeastPairingViewModel(dto);
            model.Pairing.Category = null;
            model.Pairing.Variety = null;

            var pairs = _yeastPairingsDtoList.Where(p => p.Yeast.Value == yeastDto.Id);
            if (pairs != null)
                model.Pairings = CreateYeastPairingViewModel(pairs.ToList());
            return model;
        }

        public List<YeastViewModel> CreateYeastViewModel(List<Yeast> yeastDtoList)
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
            foreach (var brand in brandDtoList)
            {
                models.Add(CreateYeastBrandViewModel(brand));
            }
            return models;
        }

        public YeastStyleViewModel CreateYeastStyleViewModel(ICode styleDto)
        {
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
            foreach (var style in styleDtoList)
            {
                models.Add(CreateYeastStyleViewModel(style));
            }
            return models;
        }

        public List<YeastPairingViewModel> CreateYeastPairingViewModel(List<YeastPair> pairingDtoList)
        {
            var models = new List<YeastPairingViewModel>();
            foreach (var p in pairingDtoList)
            {
                models.Add(CreateYeastPairingViewModel(p));
            }
            return models.OrderBy(p => p.Category.Literal).ThenBy(p => p.Variety.Literal).ToList();
        }

        public YeastPairingViewModel CreateYeastPairingViewModel(YeastPair pairingDto)
        {
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

            var existingPairings = _yeastPairingsDtoList.Where(p => p.Yeast.Value == yeastModel.Id && p.Id != pairingDto.Id);
            var model = new YeastPairingViewModel
            {
                Id = pairingDto.Id,
                Yeast = yeastModel,
                Category = categoryModel,
                Variety = varietyModel,
                Note = pairingDto.Note,
                Varieties = CreateSelectList("Variety", _varietiesDtoList, _categoriesDtoList, existingPairings.ToList())
            };

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
                        Value = dto.Id.ToString(),
                        Text = dto.Literal
                    };
                    list.Add(selectItem);
                }
            }

            return list;
        }

        public List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList, List<YeastPair> existingPairings)
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
                    Value = dto.Id.ToString(),
                    Text = dto.Literal
                };

                if (dto.ParentId.HasValue && dtoParentList != null)
                {
                    var parent = dtoParentList.Where(p => p.Id == dto.ParentId.Value).FirstOrDefault();
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

    }
}
