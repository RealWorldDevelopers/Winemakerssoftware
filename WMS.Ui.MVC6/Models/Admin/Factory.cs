
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using WMS.Communications;
using WMS.Domain;

namespace WMS.Ui.Mvc6.Models.Admin
{
    public class Factory : IFactory
    {        
        private readonly IVarietyAgent _varietyAgent;
        private readonly ICategoryAgent _categoryAgent;
        private readonly IYeastAgent _yeastAgent;
        private readonly ITempUOMAgent _tempUOMAgent;
        private readonly ISugarUOMAgent _sugarUOMAgent;
        private readonly IVolumeUOMAgent _volumeUOMAgent;
        private readonly IMaloCultureAgent _maloCultureAgent;

        private IEnumerable<Category> _categoriesDtoList;
        private IEnumerable<Variety> _varietiesDtoList;
        private IEnumerable<ICode> _yeastBrandsDtoList;
        private IEnumerable<ICode> _yeastStylesDtoList;

        private IEnumerable<Domain.MaloCulture> _maloDtoList;
        private IEnumerable<ICode> _maloBrandsDtoList;
        private IEnumerable<ICode> _maloStylesDtoList;
        private IEnumerable<Yeast> _yeastsDtoList;
        private IEnumerable<YeastPair> _yeastPairingsDtoList;

        private IEnumerable<IUnitOfMeasure> _getSugarUomList;
        private IEnumerable<IUnitOfMeasure> _getTempUomList;
        private IEnumerable<IUnitOfMeasure> _getVolumeUomList;

        public Factory(IMaloCultureAgent maloCultureAgent, IVarietyAgent varietyAgent, ICategoryAgent categoryAgent, IYeastAgent yeastAgent,
            ITempUOMAgent tempUOMAgent, ISugarUOMAgent sugarUOMAgent, IVolumeUOMAgent volumeUOMAgent)
        {
            _varietyAgent = varietyAgent;
            _categoryAgent = categoryAgent;
            _yeastAgent = yeastAgent;
            _tempUOMAgent = tempUOMAgent;
            _sugarUOMAgent = sugarUOMAgent;
            _volumeUOMAgent = volumeUOMAgent;
            _maloCultureAgent = maloCultureAgent;

            Task.Run(Init).Wait();
        }

        private async Task Init()
        {
            _categoriesDtoList = await _categoryAgent.GetCategories().ConfigureAwait(false);
            _varietiesDtoList = await _varietyAgent.GetVarieties().ConfigureAwait(false);
            _yeastsDtoList = await _yeastAgent.GetYeasts().ConfigureAwait(false);
            _yeastPairingsDtoList = await _yeastAgent.GetYeastPairs().ConfigureAwait(false);
            _yeastBrandsDtoList = await _yeastAgent.GetBrands();
            _yeastStylesDtoList = await _yeastAgent.GetStyles();

            _maloDtoList = await _maloCultureAgent.GetMaloCultures().ConfigureAwait(false);
            _maloBrandsDtoList = await _maloCultureAgent.GetBrands();
            _maloStylesDtoList = await _maloCultureAgent.GetStyles();

            _getSugarUomList = await _sugarUOMAgent.GetUOMs().ConfigureAwait(false);
            _getTempUomList = await _tempUOMAgent.GetUOMs().ConfigureAwait(false);
            _getVolumeUomList = await _volumeUOMAgent.GetUOMs().ConfigureAwait(false);
        }

        public AdminViewModel CreateAdminModel(string startingTab)
        {
            var model = new AdminViewModel
            {
                TabToShow = startingTab
            };

            // gather category / variety data
            model.CategoriesViewModel.Categories.Clear();
            model.CategoriesViewModel.Categories.AddRange(CreateCategoryViewModel(_categoriesDtoList));
            model.VarietiesViewModel.Varieties.Clear();
            model.VarietiesViewModel.Varieties.AddRange(CreateVarietyViewModel(_varietiesDtoList));

            // gather yeast data   
            model.YeastsViewModel.Yeasts.Clear();
            model.YeastsViewModel.Yeasts.AddRange(CreateYeastViewModel(_yeastsDtoList));

            // gather malolactic data   
            model.MaloCulturesViewModel.Cultures.Clear();
            model.MaloCulturesViewModel.Cultures.AddRange(CreateMaloCultureViewModel(_maloDtoList));

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

        public RecipeViewModel CreateRecipeViewModel(Recipe recipeDto)
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
                Enabled = recipeDto.Enabled ?? false,
                NeedsApproved = recipeDto.NeedsApproved ?? false,
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

            if (recipeDto.Rating != null && recipeDto.Rating.TotalValue != null)
                model.Rating = Math.Round(recipeDto.Rating.TotalValue / recipeDto.Rating.TotalVotes ?? 0, 2);
            
            return model;
        }

        public IEnumerable<RecipeViewModel> CreateRecipeViewModel(IEnumerable<Recipe> recipeDtoList)
        {
            var models = new List<RecipeViewModel>();
            foreach (var recipe in recipeDtoList.OrderBy(r => r.Enabled).ThenBy(r => r.NeedsApproved).
                ThenBy(r => r.Category?.Literal).ThenBy(r => r.Variety?.Literal).ThenBy(r => r.Description))
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

        public JournalViewModel CreateJournalViewModel(Batch batchDto, UserViewModel user)
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

        public IEnumerable<JournalViewModel> CreateJournalViewModel(IEnumerable<Batch> batchDtoList, IEnumerable<UserViewModel> users)
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

        public JournalEntryViewModel CreateBatchEntryViewModel(BatchEntry entry)
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
                    ActionDateTime = entry.ActionDateTime ?? entry.EntryDateTime,
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


                return e;
            }

        }


        public ImageViewModel CreateImageViewModel(ImageFile imageDto)
        {
            if (imageDto == null)
                throw new ArgumentNullException(nameof(imageDto));

            var model = new ImageViewModel()
            {
                Id = imageDto.Id,
                ContentType = imageDto.ContentType,
                FileName = imageDto.FileName,
                Length = imageDto.Length,
                Name = imageDto.Name,
                RecipeId = imageDto.RecipeId,
                Data = imageDto.Data
            };

            return model;
        }

        public IEnumerable<ImageViewModel> CreateImageViewModel(IEnumerable<ImageFile> imageDtoList)
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

        public CategoryViewModel CreateCategoryViewModel()
        {
            var model = new CategoryViewModel(); 
            return model;
        }

        public CategoryViewModel CreateCategoryViewModel(ICode? categoryDto)
        {
            if (categoryDto == null)
                throw new ArgumentNullException(nameof(categoryDto));

            var model = new CategoryViewModel
            {
                Description = categoryDto.Description,
                Enabled = categoryDto.Enabled ?? false,
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
                        Enabled = dto.Enabled ?? false,
                        Id = dto.Id,
                        Literal = dto.Literal
                    };
                    v.Categories.Clear();
                    v.Categories.AddRange(CreateSelectList("Parent Category", _categoriesDtoList));

                    v.Parent = new CategoryViewModel
                    {
                        Description = categoryDto.Description,
                        Enabled = categoryDto.Enabled ?? false,
                        Id = categoryDto.Id,
                        Literal = categoryDto.Literal
                    };
                    model.Varieties.Add(v);
                }
            }

            return model;
        }

        public IEnumerable<CategoryViewModel> CreateCategoryViewModel(IEnumerable<ICode> catDtoList)
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

        public VarietyViewModel CreateVarietyViewModel(ICode? varietyDto, ICode? parentDto)
        {
            if (varietyDto == null)
                throw new ArgumentNullException(nameof(varietyDto));

            var model = new VarietyViewModel
            {
                Description = varietyDto.Description,
                Enabled = varietyDto.Enabled ?? false,
                Id = varietyDto.Id,
                Literal = varietyDto.Literal
            };
            model.Categories.AddRange(CreateSelectList("Parent Category", _categoriesDtoList));

            if (parentDto != null)
                model.Parent = CreateCategoryViewModel(parentDto);

            return model;
        }

        public IEnumerable<VarietyViewModel> CreateVarietyViewModel(IEnumerable<ICode>? varDtoList)
        {
            if (varDtoList == null)
                throw new ArgumentNullException(nameof(varDtoList));

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

        public MaloCultureViewModel CreateMaloCultureViewModel(Domain.MaloCulture maloCultureDto)
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

        public IEnumerable<MaloCultureViewModel> CreateMaloCultureViewModel(IEnumerable<Domain.MaloCulture> maloDtoList)
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
                // Enabled = brandDto.Enabled,
                //Id = brandDto.Id,
                Literal = brandDto.Literal
            };
            return model;
        }

        public IEnumerable<MaloBrandViewModel> CreateMaloBrandViewModel(IEnumerable<ICode> brandDtoList)
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

        public IEnumerable<MaloStyleViewModel> CreateMaloStyleViewModel(IEnumerable<ICode> styleDtoList)
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

        public YeastViewModel CreateYeastViewModel(Yeast? yeastDto)
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

            var dto = new YeastPair { Yeast = yeastDto.Id };
            model.Pairing = CreateYeastPairingViewModel(dto);
            model.Pairing.Category = null;
            model.Pairing.Variety = null;

            var pairs = _yeastPairingsDtoList.Where(p => p.Yeast != null && p.Yeast.Value == yeastDto.Id).ToList();
            if (pairs != null)
            {
                model.Pairings.Clear();
                model.Pairings.AddRange(CreateYeastPairingViewModel(pairs));
            }

            return model;
        }

        public IEnumerable<YeastViewModel> CreateYeastViewModel(IEnumerable<Yeast> yeastDtoList)
        {
            var models = new List<YeastViewModel>();
            foreach (var yeast in yeastDtoList.OrderBy(y => y.Brand?.Literal).ThenBy(y => y.Trademark))
            {
                models.Add(CreateYeastViewModel(yeast));
            }
            return models;
        }

        public YeastBrandViewModel CreateYeastBrandViewModel(ICode? brandDto)
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

        public IEnumerable<YeastBrandViewModel> CreateYeastBrandViewModel(IEnumerable<ICode> brandDtoList)
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

        public YeastStyleViewModel CreateYeastStyleViewModel(ICode? styleDto)
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

        public IEnumerable<YeastStyleViewModel> CreateYeastStyleViewModel(IEnumerable<ICode> styleDtoList)
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

        public IEnumerable<YeastPairingViewModel> CreateYeastPairingViewModel(IEnumerable<YeastPair> pairingDtoList)
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

        public YeastPairingViewModel CreateYeastPairingViewModel(YeastPair pairingDto)
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





        public IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<ICode> dtoList)
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
                        Value = dto.Id?.ToString(CultureInfo.CurrentCulture),
                        Text = dto.Literal
                    };
                    list.Add(selectItem);
                }
            }

            return list;
        }

        public IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<ICode> dtoList, IEnumerable<ICode> dtoParentList, IEnumerable<YeastPair> existingPairings)
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
                    Value = dto.Id?.ToString(CultureInfo.CurrentCulture),
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
                            Value = parent.Id?.ToString(CultureInfo.CurrentCulture),
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

        public IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<IUnitOfMeasure> dtoList)
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

        public IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<Yeast> dtoList)
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

        public IEnumerable<SelectListItem> CreateSelectList(string title, IEnumerable<Domain.MaloCulture> dtoList)
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


    }
}
