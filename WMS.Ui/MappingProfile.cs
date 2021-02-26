using AutoMapper;
using System;

namespace WMS.Ui
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<Models.Admin.RoleViewModel, Models.ApplicationRole>().ReverseMap();
         CreateMap<Models.Admin.UserViewModel, Models.ApplicationUser>().ReverseMap();


         CreateMap<Models.Admin.CategoryViewModel, Business.Common.ICode>().ConstructUsing(src => new Business.Common.Code());
         CreateMap<Models.Admin.VarietyViewModel, Business.Common.ICode>()
            .ConstructUsing(src => new Business.Common.Code())
            .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Parent.Id));
         CreateMap<Models.Admin.YeastBrandViewModel, Business.Common.ICode>()
            .ConstructUsing(src => new Business.Common.Code())
            .AfterMap((src, dest) =>
             {
                dest = src != null ? new Business.Common.Code
                {
                   Id = src.Id,
                   Literal = src.Literal,
                   Description = src.Description,
                   Enabled = src.Enabled,
                   ParentId = null
                } : null;
             });
         CreateMap<Models.Admin.YeastStyleViewModel, Business.Common.ICode>()
            .ConstructUsing(src => new Business.Common.Code())
            .AfterMap((src, dest) =>
             {
                dest = src != null ? new Business.Common.Code
                {
                   Id = src.Id,
                   Literal = src.Literal,
                   Description = src.Description,
                   Enabled = src.Enabled,
                   ParentId = null
                } : null;
             });

         //CreateMap<WMS.Data.Entities.Images, Business.Image.Dto.ImageDto>().ReverseMap();
         CreateMap<WMS.Data.Entities.Images, Business.Image.Dto.ImageDto>()
             .AfterMap((src, dest) =>
              {
                 dest = src != null ? new Business.Image.Dto.ImageDto(src.Thumbnail, src.Data)
                 {
                    Id = src.Id,
                    ContentType = src.ContentType,
                    FileName = src.FileName,
                    Length = src.Length ?? 0,
                    Name = src.Name
                 } : null;
              });


         CreateMap<WMS.Data.Entities.UnitsOfMeasure, Business.Common.IUnitOfMeasure>().ReverseMap();

         CreateMap<WMS.Data.Entities.BatchEntries, Business.Journal.Dto.BatchEntryDto>()
            .ForMember(dest => dest.SugarUom, opt => opt.Ignore())
            .ForMember(dest => dest.TempUom, opt => opt.Ignore())
            .AfterMap((src, dest) => { dest.TempUom = src.TempUomId.HasValue ? new Business.Common.UnitOfMeasure { Id = src.TempUomId.Value } : null; })
            .AfterMap((src, dest) => { dest.SugarUom = src.SugarUomId.HasValue ? new Business.Common.UnitOfMeasure { Id = src.SugarUomId.Value } : null; });

         CreateMap<Business.Journal.Dto.BatchEntryDto, WMS.Data.Entities.BatchEntries>()
             .ForMember(dest => dest.TempUomId, opt => opt.MapFrom(src => src.TempUom.Id))
             .ForMember(dest => dest.SugarUomId, opt => opt.MapFrom(src => src.SugarUom.Id));

         CreateMap<WMS.Data.Entities.Batches, Business.Journal.Dto.BatchDto>()
            .ForMember(dest => dest.VolumeUom, opt => opt.Ignore())
            .ForMember(dest => dest.Variety, opt => opt.Ignore())
            .ForMember(dest => dest.Target, opt => opt.Ignore())
            .AfterMap((src, dest) => { dest.VolumeUom = src.VolumeUomId.HasValue ? new Business.Common.UnitOfMeasure { Id = src.VolumeUomId.Value } : null; })
            .AfterMap((src, dest) => { dest.Variety = src.VarietyId.HasValue ? new Business.Common.Code { Id = src.VarietyId.Value } : null; })
            .AfterMap((src, dest) => { dest.Target = src.TargetId.HasValue ? new Business.Journal.Dto.TargetDto { Id = src.TargetId.Value } : null; });

         CreateMap<Business.Journal.Dto.BatchDto, WMS.Data.Entities.Batches>()
            .ForMember(dest => dest.VolumeUom, opt => opt.Ignore())
            .ForMember(dest => dest.Variety, opt => opt.Ignore())
            .ForMember(dest => dest.Target, opt => opt.Ignore())
            .ForMember(dest => dest.VolumeUomId, opt => opt.MapFrom(src => src.VolumeUom.Id))
            .ForMember(dest => dest.VarietyId, opt => opt.MapFrom(src => src.Variety.Id))
            .ForMember(dest => dest.TargetId, opt => opt.MapFrom(src => src.Target.Id));

         CreateMap<WMS.Data.Entities.Targets, Business.Journal.Dto.TargetDto>()
            .ForMember(dest => dest.TempUom, opt => opt.Ignore())
            .ForMember(dest => dest.StartSugarUom, opt => opt.Ignore())
            .ForMember(dest => dest.EndSugarUom, opt => opt.Ignore())
            .AfterMap((src, dest) => { dest.TempUom = src.TempUomId.HasValue ? new Business.Common.UnitOfMeasure { Id = src.TempUomId.Value } : null; })
            .AfterMap((src, dest) => { dest.StartSugarUom = src.StartSugarUomId.HasValue ? new Business.Common.UnitOfMeasure { Id = src.StartSugarUomId.Value } : null; })
            .AfterMap((src, dest) => { dest.EndSugarUom = src.EndSugarUomId.HasValue ? new Business.Common.UnitOfMeasure { Id = src.EndSugarUomId.Value } : null; });

         CreateMap<Business.Journal.Dto.TargetDto, WMS.Data.Entities.Targets>()
            .ForMember(dest => dest.TempUom, opt => opt.Ignore())
            .ForMember(dest => dest.StartSugarUom, opt => opt.Ignore())
            .ForMember(dest => dest.EndSugarUom, opt => opt.Ignore())
            .ForMember(dest => dest.TempUomId, opt => opt.MapFrom(src => src.TempUom.Id))
            .ForMember(dest => dest.StartSugarUomId, opt => opt.MapFrom(src => src.StartSugarUom.Id))
            .ForMember(dest => dest.EndSugarUomId, opt => opt.MapFrom(src => src.EndSugarUom.Id));

         CreateMap<WMS.Data.Entities.UnitsOfMeasure, Business.Common.UnitOfMeasure>().ReverseMap();

         CreateMap<WMS.Data.Entities.PicturesXref, Business.Recipe.Dto.ImageFileDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ImageId));
         CreateMap<Business.Recipe.Dto.ImageFileDto, WMS.Data.Entities.PicturesXref>()
            .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.Id));

         CreateMap<WMS.Data.Entities.Ratings, Business.Recipe.Dto.RatingDto>().ReverseMap();
         CreateMap<Business.Yeast.Dto.YeastPairDto, WMS.Data.Entities.YeastPair>().ReverseMap();

         CreateMap<Business.MaloCulture.Dto.MaloCultureDto, WMS.Data.Entities.MaloCultures>().ReverseMap();

         CreateMap<WMS.Data.Entities.Varieties, Business.Common.ICode>()
            .ConstructUsing(src => new Business.Common.Code())
            .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Variety))
            .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.CategoryId)).ReverseMap();

         CreateMap<WMS.Data.Entities.Categories, Business.Common.ICode>()
            .ConstructUsing(src => new Business.Common.Code())
            .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Category)).ReverseMap();

         CreateMap<WMS.Data.Entities.MaloCultureBrand, Business.Common.ICode>()
            .ConstructUsing(src => new Business.Common.Code())
            .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Brand));

         CreateMap<WMS.Data.Entities.MaloCultureStyle, Business.Common.ICode>()
            .ConstructUsing(src => new Business.Common.Code())
            .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Style));

         CreateMap<WMS.Data.Entities.YeastBrand, Business.Common.ICode>()
            .ConstructUsing(src => new Business.Common.Code())
            .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Brand));

         CreateMap<WMS.Data.Entities.YeastStyle, Business.Common.ICode>()
            .ConstructUsing(src => new Business.Common.Code())
            .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Style));

         // Convert Recipes Entity into Recipe DTO
         CreateMap<WMS.Data.Entities.Recipes, Business.Recipe.Dto.RecipeDto>()
             .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Ratings))
             .AfterMap((src, dest) => { dest.Variety = src.VarietyId.HasValue ? new Business.Common.Code { Id = src.VarietyId.Value } : null; })
             .AfterMap((src, dest) => { dest.Yeast = src.YeastId.HasValue ? new Business.Yeast.Dto.YeastDto { Id = src.YeastId.Value } : null; })
             .AfterMap((src, dest) => { dest.Target = src.TargetId.HasValue ? new Business.Journal.Dto.TargetDto { Id = src.TargetId.Value } : null; });


         // Convert Recipe DTO into Recipes Entity
         CreateMap<Business.Recipe.Dto.RecipeDto, WMS.Data.Entities.Recipes>().ConvertUsing(new RecipesConverter());

         CreateMap<WMS.Data.Entities.Yeasts, Business.Yeast.Dto.YeastDto>()
             .ForMember(dest => dest.Brand, opt => opt.Ignore())
             .ForMember(dest => dest.Style, opt => opt.Ignore())
             .AfterMap((src, dest) => { dest.Brand = src.Brand.HasValue ? new Business.Common.Code { Id = src.Brand.Value } : null; })
             .AfterMap((src, dest) => { dest.Style = src.Style.HasValue ? new Business.Common.Code { Id = src.Style.Value } : null; });
         CreateMap<Business.Yeast.Dto.YeastDto, WMS.Data.Entities.Yeasts>()
             .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Id))
             .ForMember(dest => dest.Style, opt => opt.MapFrom(src => src.Style.Id));

         CreateMap<WMS.Data.Entities.MaloCultures, Business.MaloCulture.Dto.MaloCultureDto>()
           .ForMember(dest => dest.Brand, opt => opt.Ignore())
           .ForMember(dest => dest.Style, opt => opt.Ignore())
           .AfterMap((src, dest) => { dest.Brand = src.Brand.HasValue ? new Business.Common.Code { Id = src.Brand.Value } : null; })
           .AfterMap((src, dest) => { dest.Style = src.Style.HasValue ? new Business.Common.Code { Id = src.Style.Value } : null; });
         CreateMap<Business.MaloCulture.Dto.MaloCultureDto, WMS.Data.Entities.MaloCultures>()
             .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Id))
             .ForMember(dest => dest.Style, opt => opt.MapFrom(src => src.Style.Id));
      }

      /// <summary>
      /// Convert Recipe DTO into Recipes Entity 
      /// </summary>
      sealed class RecipesConverter : ITypeConverter<Business.Recipe.Dto.RecipeDto, WMS.Data.Entities.Recipes>
      {
         WMS.Data.Entities.Recipes ITypeConverter<Business.Recipe.Dto.RecipeDto, WMS.Data.Entities.Recipes>
             .Convert(Business.Recipe.Dto.RecipeDto source, WMS.Data.Entities.Recipes destination, ResolutionContext context)
         {
            var entity = new WMS.Data.Entities.Recipes
            {
               Title = source.Title,
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
