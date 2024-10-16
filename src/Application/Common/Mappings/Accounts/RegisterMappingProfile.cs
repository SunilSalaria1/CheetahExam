using CheetahExam.WebUI.Shared.Common.Models.Accounts;

namespace CheetahExam.Application.Common.Mappings.Accounts;

public class RegisterMappingProfile : Profile
{
    public RegisterMappingProfile()
    {
        CreateMap<RegisterVm, User>()
            .ForMember(
                destination => destination.UniqueId,
                source => source.Condition(source => !string.IsNullOrEmpty(source.UniqueId)))
            .ReverseMap();
    }
}
