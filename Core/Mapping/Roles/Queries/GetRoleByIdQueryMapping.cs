using Core.Features.Authorization.Queries.Responses;

namespace Core.Mapping.Roles
{
    public partial class RoleProfile
    {
        public void GetRoleByIdQueryMapping()
        {
            CreateMap<Role, GetSingleRoleResponse>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
