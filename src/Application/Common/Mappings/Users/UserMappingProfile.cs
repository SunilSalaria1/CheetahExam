using CheetahExam.WebUI.Shared.Common.Models.Users;

namespace CheetahExam.Application.Common.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserDto, User>()
            .ForMember(
                destination => destination.UniqueId,
                source => source.Condition(source => !string.IsNullOrEmpty(source.UniqueId)))
            .ReverseMap();
    }
}
