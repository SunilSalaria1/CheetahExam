using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Common.Mappings.Exams;

public class OptionMappingProfile : Profile
{
    public OptionMappingProfile()
    {
        CreateMap<Option, OptionDto>()
            .ReverseMap()
            .ForMember(destination => destination.UniqueId, source => source.Condition(source => !string.IsNullOrEmpty(source.UniqueId)));
    }
}
