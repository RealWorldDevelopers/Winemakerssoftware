using AutoMapper;
using System;
using System.Linq;

namespace WMS.Ui
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Admin.RoleViewModel, Models.ApplicationRole>().ReverseMap();
            CreateMap<Models.Admin.UserViewModel, Models.ApplicationUser>().ReverseMap();


            CreateMap<Models.Admin.CategoryViewModel, Business.Shared.ICode>();
            CreateMap<Models.Admin.VarietyViewModel, Business.Shared.ICode>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Parent.Id));
            CreateMap<Models.Admin.YeastBrandViewModel, Business.Shared.ICode>()
                .AfterMap((src, dest) =>
                {
                    dest = src != null ? new Business.Shared.Code
                    {
                        Id = src.Id,
                        Literal = src.Literal,
                        Description = src.Description,
                        Enabled = src.Enabled,
                        ParentId = null
                    } : null;
                });
            CreateMap<Models.Admin.YeastStyleViewModel, Business.Shared.ICode>()
                .AfterMap((src, dest) =>
                {
                    dest = src != null ? new Business.Shared.Code
                    {
                        Id = src.Id,
                        Literal = src.Literal,
                        Description = src.Description,
                        Enabled = src.Enabled,
                        ParentId = null
                    } : null;
                });

            CreateMap<WMS.Data.Entities.Images, Business.Image.Dto.Image>().ReverseMap();

            CreateMap<WMS.Data.Entities.PicturesXref, Business.Recipe.Dto.ImageFile>()
                .ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>src.ImageId));
            CreateMap<Business.Recipe.Dto.ImageFile, WMS.Data.Entities.PicturesXref>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.Id));

            CreateMap<WMS.Data.Entities.Ratings, Business.Recipe.Dto.Rating>().ReverseMap();
            CreateMap<Business.Yeast.Dto.YeastPair, WMS.Data.Entities.YeastPair>().ReverseMap();

            CreateMap<WMS.Data.Entities.Varieties, Business.Shared.ICode>()
                .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Variety))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.CategoryId)).ReverseMap();

            //CreateMap< Business.Shared.Dto.ICode, WMS.Data.Entities.Varieties>()
            //   .ForMember(dest => dest.Variety, opt => opt.MapFrom(src => src.Literal))
            //   .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.ParentId));

            CreateMap<WMS.Data.Entities.Categories, Business.Shared.ICode>()
                .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Category)).ReverseMap();

            //CreateMap< Business.Shared.Dto.ICode, WMS.Data.Entities.Categories>()
            // .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Literal));          

            CreateMap<WMS.Data.Entities.YeastBrand, Business.Shared.ICode>()
                .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Brand));

            CreateMap<WMS.Data.Entities.YeastStyle, Business.Shared.ICode>()
                .ForMember(dest => dest.Literal, opt => opt.MapFrom(src => src.Style));

            CreateMap<WMS.Data.Entities.Recipes, Business.Recipe.Dto.Recipe>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Ratings))
                .ForMember(dest => dest.ImageFiles, opt => opt.MapFrom(src => src.PicturesXref.ToList()))
                .AfterMap((src, dest) => { dest.Variety = src.VarietyId.HasValue ? new Business.Shared.Code { Id = src.VarietyId.Value } : null; });

            CreateMap<Business.Recipe.Dto.Recipe, WMS.Data.Entities.Recipes>().ConvertUsing(new RecipesConverter());

            CreateMap<WMS.Data.Entities.Yeasts, Business.Yeast.Dto.Yeast>()
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.Style, opt => opt.Ignore())
                .AfterMap((src, dest) => { dest.Brand = src.Brand.HasValue ? new Business.Shared.Code { Id = src.Brand.Value } : null; })
                .AfterMap((src, dest) => { dest.Style = src.Style.HasValue ? new Business.Shared.Code { Id = src.Style.Value } : null; });
            CreateMap<Business.Yeast.Dto.Yeast, WMS.Data.Entities.Yeasts>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Id))
                .ForMember(dest => dest.Style, opt => opt.MapFrom(src => src.Style.Id));
        }

        /// <summary>
        /// Convert Recipes Entity into Recipe DTO
        /// </summary>
        public class RecipesConverter : ITypeConverter<Business.Recipe.Dto.Recipe, WMS.Data.Entities.Recipes>
        {
            WMS.Data.Entities.Recipes ITypeConverter<Business.Recipe.Dto.Recipe, WMS.Data.Entities.Recipes>
                .Convert(Business.Recipe.Dto.Recipe source, WMS.Data.Entities.Recipes destination, ResolutionContext context)
            {
                var entity = new WMS.Data.Entities.Recipes
                {
                    Title = source.Title,
                    VarietyId = source.Variety.Id,
                    Description = source.Description,
                    Ingredients = source.Ingredients,
                    Instructions = source.Instructions,
                    SubmittedBy = source.SubmittedBy,
                    AddDate = DateTime.Now,
                    Hits = source.Hits,
                    Enabled = source.Enabled,
                    NeedsApproved = source.NeedsApproved
                };

                return entity;
            }
        }

    }
}
