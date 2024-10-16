using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Common.Mappings.Exams;

public class QuestionMappingProfile : Profile
{
    public QuestionMappingProfile()
    {
        CreateMap<Question, QuestionDto>()
            .ReverseMap()
            .ForMember(destination => destination.UniqueId, source => source.Condition(source => !string.IsNullOrEmpty(source.UniqueId)));
    }
}
