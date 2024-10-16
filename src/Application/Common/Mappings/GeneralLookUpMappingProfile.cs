using CheetahExam.WebUI.Shared.Common.Models.GeneralLookUps;

namespace CheetahExam.Application.Common.Mappings;

public class GeneralLookUpMappingProfile : Profile
{
    public GeneralLookUpMappingProfile()
    {
        CreateMap<GeneralLookUpDto, GeneralLookUp>()
            .ForMember(
                destination => destination.UniqueId,
                source => source.Condition(source => !string.IsNullOrEmpty(source.UniqueId)))
            .ReverseMap();
    }
}
