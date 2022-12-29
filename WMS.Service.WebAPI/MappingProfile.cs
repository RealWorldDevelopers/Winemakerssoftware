using AutoMapper;
using System;
using WMS.Business.Recipe.Dto;

namespace WMS.Service.WebAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Models.Admin.RoleViewModel, Models.ApplicationRole>().ReverseMap();
            //CreateMap<Models.Admin.UserViewModel, Models.ApplicationUser>().ReverseMap();


            //CreateMap<Models.Admin.CategoryViewModel, Business.Common.ICode>().ConstructUsing(src => new Business.Common.Code());
            //CreateMap<Models.Admin.VarietyViewModel, Business.Common.ICode>()
            //   .ConstructUsing(src => new Business.Common.Code())
            //   .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Parent.Id));
            //CreateMap<Models.Admin.YeastBrandViewModel, Business.Common.ICode>()
            //   .ConstructUsing(src => new Business.Common.Code())
            //   .AfterMap((src, dest) =>
            //   {
            //       dest = src != null ? new Business.Common.Code
            //       {
            //           Id = src.Id,
            //           Literal = src.Literal,
            //           Description = src.Description,
            //           Enabled = src.Enabled,
            //           ParentId = null
            //       } : null;
            //   });
            //CreateMap<Models.Admin.YeastStyleViewModel, Business.Common.ICode>()
            //   .ConstructUsing(src => new Business.Common.Code())
            //   .AfterMap((src, dest) =>
            //   {
            //       dest = src != null ? new Business.Common.Code
            //       {
            //           Id = src.Id,
            //           Literal = src.Literal,
            //           Description = src.Description,
            //           Enabled = src.Enabled,
            //           ParentId = null
            //       } : null;
            //   });

            //CreateMap<WMS.Data.SQL.Entities.Images, Business.Image.Dto.ImageDto>().ReverseMap();
            CreateMap<WMS.Data.SQL.Entities.Image, Business.Image.Dto.ImageDto>()
                .AfterMap((src, dest) =>
                {
                    dest = new Business.Image.Dto.ImageDto()
                    {
                        Id = src.Id,
                        ContentType = src.ContentType ?? string.Empty,
                        FileName = src.FileName ?? string.Empty,
                        Length = src.Length ?? 0,
                        Name = src.Name,
                        Data = src.Data,
                        Thumbnail = src.Thumbnail
                    };
                });

            CreateMap<WMS.Data.SQL.Entities.UnitsOfMeasure, Business.Common.IUnitOfMeasureDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UnitOfMeasure));
            CreateMap<Business.Common.IUnitOfMeasureDto, WMS.Data.SQL.Entities.UnitsOfMeasure>()
                .ForMember(dest => dest.UnitOfMeasure, opt => opt.MapFrom(src => src.Name));

            CreateMap<WMS.Data.SQL.Entities.UnitsOfMeasure, Business.Common.UnitOfMeasureDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UnitOfMeasure));
            CreateMap<Business.Common.UnitOfMeasureDto, WMS.Data.SQL.Entities.UnitsOfMeasure>()
                .ForMember(dest => dest.UnitOfMeasure, opt => opt.MapFrom(src => src.Name));


            CreateMap<WMS.Data.SQL.Entities.BatchEntry, Business.Journal.Dto.BatchEntryDto>()
               .ForMember(dest => dest.SugarUom, opt => opt.Ignore())
               .ForMember(dest => dest.TempUom, opt => opt.Ignore())
               .AfterMap((src, dest) => { dest.TempUom = src.TempUomId.HasValue ? new Business.Common.UnitOfMeasureDto { Id = src.TempUomId.Value } : null; })
               .AfterMap((src, dest) => { dest.SugarUom = src.SugarUomId.HasValue ? new Business.Common.UnitOfMeasureDto { Id = src.SugarUomId.Value } : null; });

            CreateMap<Business.Journal.Dto.BatchEntryDto, WMS.Data.SQL.Entities.BatchEntry>()
                .ForMember(dest => dest.TempUomId, opt => opt.MapFrom(src => src.TempUom != null ? src.TempUom.Id : null))
                .ForMember(dest => dest.SugarUomId, opt => opt.MapFrom(src => src.SugarUom != null ? src.SugarUom.Id : null));

            CreateMap<WMS.Data.SQL.Entities.Batch, Business.Journal.Dto.BatchDto>()
               .ForMember(dest => dest.VolumeUom, opt => opt.Ignore())
               .ForMember(dest => dest.Variety, opt => opt.Ignore())
               .ForMember(dest => dest.Yeast, opt => opt.Ignore())
               .ForMember(dest => dest.Target, opt => opt.Ignore())
               .AfterMap((src, dest) => { dest.VolumeUom = src.VolumeUomId.HasValue ? new Business.Common.UnitOfMeasureDto { Id = src.VolumeUomId.Value } : null; })
               .AfterMap((src, dest) => { dest.Variety = src.VarietyId.HasValue ? new Business.Common.CodeDto { Id = src.VarietyId.Value } : null; })
               .AfterMap((src, dest) => { dest.Yeast = src.YeastId.HasValue ? new Business.Yeast.Dto.YeastDto { Id = src.YeastId.Value } : null; })
               .AfterMap((src, dest) => { dest.Target = src.TargetId.HasValue ? new Business.Journal.Dto.TargetDto { Id = src.TargetId.Value } : null; });

            CreateMap<Business.Journal.Dto.BatchDto, WMS.Data.SQL.Entities.Batch>()
               .ForMember(dest => dest.VolumeUom, opt => opt.Ignore())
               .ForMember(dest => dest.Variety, opt => opt.Ignore())
               .ForMember(dest => dest.Yeast, opt => opt.Ignore())
               .ForMember(dest => dest.Target, opt => opt.Ignore())
               .ForMember(dest => dest.VolumeUomId, opt => opt.MapFrom(src => src.VolumeUom != null ? src.VolumeUom.Id : null))
               .ForMember(dest => dest.VarietyId, opt => opt.MapFrom(src => src.Variety != null ? src.Variety.Id : null))
               .ForMember(dest => dest.YeastId, opt => opt.MapFrom(src => src.Yeast != null ? src.Yeast.Id : null))
               .ForMember(dest => dest.TargetId, opt => opt.MapFrom(src => src.Target != null ? src.Target.Id : null));

            CreateMap<WMS.Data.SQL.Entities.Target, Business.Journal.Dto.TargetDto>()
               .ForMember(dest => dest.TempUom, opt => opt.Ignore())
               .ForMember(dest => dest.StartSugarUom, opt => opt.Ignore())
               .ForMember(dest => dest.EndSugarUom, opt => opt.Ignore())
               .AfterMap((src, dest) => { dest.TempUom = src.TempUomId.HasValue ? new Business.Common.UnitOfMeasureDto { Id = src.TempUomId.Value } : null; })
               .AfterMap((src, dest) => { dest.StartSugarUom = src.StartSugarUomId.HasValue ? new Business.Common.UnitOfMeasureDto { Id = src.StartSugarUomId.Value } : null; })
               .AfterMap((src, dest) => { dest.EndSugarUom = src.EndSugarUomId.HasValue ? new Business.Common.UnitOfMeasureDto { Id = src.EndSugarUomId.Value } : null; });

            CreateMap<Business.Journal.Dto.TargetDto, WMS.Data.SQL.Entities.Target>()
               .ForMember(dest => dest.TempUom, opt => opt.Ignore())
               .ForMember(dest => dest.StartSugarUom, opt => opt.Ignore())
               .ForMember(dest => dest.EndSugarUom, opt => opt.Ignore())
               .ForMember(dest => dest.TempUomId, opt => opt.MapFrom(src => src.TempUom != null ? src.TempUom.Id : null))
               .ForMember(dest => dest.StartSugarUomId, opt => opt.MapFrom(src => src.StartSugarUom != null ? src.StartSugarUom.Id : null))
               .ForMember(dest => dest.EndSugarUomId, opt => opt.MapFrom(src => src.EndSugarUom != null ? src.EndSugarUom.Id : null));



            CreateMap<WMS.Data.SQL.Entities.PicturesXref, Business.Image.Dto.ImageDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ImageId));
            CreateMap<Business.Image.Dto.ImageDto, WMS.Data.SQL.Entities.PicturesXref>()
               .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.Id));

            CreateMap<WMS.Data.SQL.Entities.Rating, Business.Recipe.Dto.RatingDto>().ReverseMap();
            CreateMap<Business.Yeast.Dto.YeastPairDto, WMS.Data.SQL.Entities.YeastPair>().ReverseMap();

            CreateMap<Business.MaloCulture.Dto.MaloCultureDto, WMS.Data.SQL.Entities.MaloCulture>().ReverseMap();

            CreateMap<WMS.Data.SQL.Entities.Variety, Business.Common.ICodeDto>()
               .ConstructUsing(src => new Business.Common.CodeDto())
               .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Variety1))
               .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.CategoryId)).ReverseMap();

            CreateMap<WMS.Data.SQL.Entities.Category, Business.Common.ICodeDto>()
               .ConstructUsing(src => new Business.Common.CodeDto())
               .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Category1)).ReverseMap();

            CreateMap<WMS.Data.SQL.Entities.MaloCultureBrand, Business.Common.ICodeDto>()
               .ConstructUsing(src => new Business.Common.CodeDto())
               .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Brand));

            CreateMap<WMS.Data.SQL.Entities.MaloCultureStyle, Business.Common.ICodeDto>()
               .ConstructUsing(src => new Business.Common.CodeDto())
               .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Style));

            CreateMap<WMS.Data.SQL.Entities.YeastBrand, Business.Common.ICodeDto>()
               .ConstructUsing(src => new Business.Common.CodeDto())
               .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Brand));

            CreateMap<WMS.Data.SQL.Entities.YeastStyle, Business.Common.ICodeDto>()
               .ConstructUsing(src => new Business.Common.CodeDto())
               .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Style));

            // Convert Recipes Entity into Recipe DTO
            CreateMap<WMS.Data.SQL.Entities.Recipe, Business.Recipe.Dto.RecipeDto>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .AfterMap((src, dest) => { dest.Variety = src.VarietyId.HasValue ? new Business.Common.CodeDto { Id = src.VarietyId.Value } : null; })
                .AfterMap((src, dest) => { dest.Yeast = src.YeastId.HasValue ? new Business.Yeast.Dto.YeastDto { Id = src.YeastId.Value } : null; })
                .AfterMap((src, dest) => { dest.Target = src.TargetId.HasValue ? new Business.Journal.Dto.TargetDto { Id = src.TargetId.Value } : null; });


            // Convert Recipe DTO into Recipes Entity
            CreateMap<Business.Recipe.Dto.RecipeDto, WMS.Data.SQL.Entities.Recipe>().ConvertUsing(new RecipesConverter());

            CreateMap<WMS.Data.SQL.Entities.Yeast, Business.Yeast.Dto.YeastDto>()
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.Style, opt => opt.Ignore())
                .AfterMap((src, dest) => { dest.Brand = src.Brand.HasValue ? new Business.Common.CodeDto { Id = src.Brand.Value } : null; })
                .AfterMap((src, dest) => { dest.Style = src.Style.HasValue ? new Business.Common.CodeDto { Id = src.Style.Value } : null; });
            CreateMap<Business.Yeast.Dto.YeastDto, WMS.Data.SQL.Entities.Yeast>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Id: null))
                .ForMember(dest => dest.Style, opt => opt.MapFrom(src => src.Style != null ? src.Style.Id: null));

            CreateMap<WMS.Data.SQL.Entities.MaloCulture, Business.MaloCulture.Dto.MaloCultureDto>()
              .ForMember(dest => dest.Brand, opt => opt.Ignore())
              .ForMember(dest => dest.Style, opt => opt.Ignore())
              .AfterMap((src, dest) => { dest.Brand = src.Brand.HasValue ? new Business.Common.CodeDto { Id = src.Brand.Value } : null; })
              .AfterMap((src, dest) => { dest.Style = src.Style.HasValue ? new Business.Common.CodeDto { Id = src.Style.Value } : null; });
            CreateMap<Business.MaloCulture.Dto.MaloCultureDto, WMS.Data.SQL.Entities.MaloCulture>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Id : null))
                .ForMember(dest => dest.Style, opt => opt.MapFrom(src => src.Style != null ? src.Style.Id : null));
        }

        /// <summary>
        /// Convert Recipe DTO into Recipes Entity 
        /// </summary>
        sealed class RecipesConverter : ITypeConverter<Business.Recipe.Dto.RecipeDto, WMS.Data.SQL.Entities.Recipe>
        {

            WMS.Data.SQL.Entities.Recipe ITypeConverter<Business.Recipe.Dto.RecipeDto, WMS.Data.SQL.Entities.Recipe>
                .Convert(Business.Recipe.Dto.RecipeDto source, WMS.Data.SQL.Entities.Recipe destination, ResolutionContext context)
            {
                var entity = new WMS.Data.SQL.Entities.Recipe
                {
                    Title = source.Title ?? string.Empty,
                    VarietyId = source.Variety?.Id,
                    YeastId = source.Yeast?.Id,
                    TargetId = source.Target?.Id,
                    Description = source.Description,
                    Ingredients = source.Ingredients,
                    Instructions = source.Instructions,
                    SubmittedBy = source.SubmittedBy,
                    AddDate = DateTime.UtcNow,
                    Hits = source.Hits,
                    Enabled = source.Enabled,
                    NeedsApproved = source.NeedsApproved
                };

                return entity;
            }
        }

    }
}
