using AutoMapper;
using WMS.Domain;

namespace WMS.Ui.Mvc6
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Admin.RoleViewModel, Models.ApplicationRole>().ReverseMap();
            CreateMap<Models.Admin.UserViewModel, Models.ApplicationUser>().ReverseMap();                                       

        }
            
    }
}
