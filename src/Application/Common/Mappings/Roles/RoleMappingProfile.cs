using CheetahExam.WebUI.Shared.Common.Models.GeneralLookUps;
using CheetahExam.WebUI.Shared.Common.Models.Users;

namespace CheetahExam.Application.Common.Mappings;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<RoleDto, Role>()
            .ForMember(
                destination => destination.UniqueId,
                source => source.Condition(source => !string.IsNullOrEmpty(source.UniqueId)))
            .ReverseMap();
    }
}
