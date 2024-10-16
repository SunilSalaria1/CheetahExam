using CheetahExam.WebUI.Shared.Common.Models;
using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Common.Mappings.Exams;

public class ExamMappingProfile : Profile
{
    public ExamMappingProfile()
    {
        CreateMap<ExamDto, Exam>().ForMember(destination => destination.UniqueId, source => source.Condition(source => !string.IsNullOrEmpty(source.UniqueId)))
            .ReverseMap()
            .ForMember(destination => destination.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count));

        CreateMap<ExamResultOptionDto, ExamResultOption>().ForMember(destination => destination.UniqueId, source => source.Condition(source => !string.IsNullOrEmpty(source.UniqueId)))
            .ReverseMap();
    }
}
