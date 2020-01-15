
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WMS.Business.Recipe.Dto;
using WMS.Business.Common;
using WMS.Business.Yeast.Dto;

namespace WMS.Ui.Models.Admin
{
    public class Factory : IFactory
    {
        private readonly Business.Recipe.Queries.IFactory _recipeQueryFactory;
        private readonly Business.Yeast.Queries.IFactory _yeastQueryFactory;

        private readonly IQuery<ICode> _getCategoriesQuery;
        private readonly IQuery<ICode> _getVarietiesQuery;
        private readonly IQuery<YeastDto> _getYeastsQuery;
        private readonly IQuery<YeastPairDto> _getYeastPairsQuery;
        private readonly IQuery<ICode> _getBrandsQuery;
        private readonly IQuery<ICode> _getStylesQuery;

        private readonly List<ICode> _categoriesDtoList;
        private readonly List<ICode> _varietiesDtoList;
        private readonly List<ICode> _brandsDtoList;
        private readonly List<ICode> _stylesDtoList;
        private readonly List<YeastDto> _yeastsDtoList;
        private readonly List<YeastPairDto> _yeastPairingsDtoList;

        public Factory(Business.Recipe.Queries.IFactory recipeQueryFactory, Business.Yeast.Queries.IFactory yeastQueryFactory)
        {
            _recipeQueryFactory = recipeQueryFactory;
            _yeastQueryFactory = yeastQueryFactory;

            _getCategoriesQuery = _recipeQueryFactory?.CreateCategoriesQuery();
            _getVarietiesQuery = _recipeQueryFactory.CreateVarietiesQuery();
            _getYeastsQuery = _yeastQueryFactory?.CreateYeastsQuery();
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
            var model = new RecipeViewModel
            {
                Category = new CategoryViewModel(),
                Variety = new VarietyViewModel()
            };
            model.Varieties.Clear();
            model.Varieties.AddRange(CreateSelectList("Variety", _varietiesDtoList, _categoriesDtoList, null));
            return model;
        }

        public RecipeViewModel CreateRecipeViewModel(RecipeDto recipeDto)
        {
            if (recipeDto == null)
                throw new ArgumentNullException(nameof(recipeDto));

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
                Ingredients = recipeDto.Ingredients,
                Instructions = recipeDto.Instructions,
                Description = recipeDto.Description
            };
            model.Images.Clear();
            model.Images.AddRange(CreateImageViewModel(recipeDto.ImageFiles));
            model.Varieties.Clear();
            model.Varieties.AddRange(CreateSelectList("Variety", _varietiesDtoList, _categoriesDtoList, null));

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



        public YeastViewModel CreateYeastViewModel()
        {
            var model = new YeastViewModel();
            model.Brands.AddRange(CreateSelectList("Brand", _brandsDtoList));
            model.Styles.AddRange(CreateSelectList("Style", _stylesDtoList));
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
            model.Brands.AddRange(CreateSelectList("Brand", _brandsDtoList));
            model.Styles.AddRange(CreateSelectList("Style", _stylesDtoList));

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

        public static List<SelectListItem> CreateSelectList(string title, List<ICode> dtoList, List<ICode> dtoParentList, List<YeastPairDto> existingPairings)
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

    }
}
