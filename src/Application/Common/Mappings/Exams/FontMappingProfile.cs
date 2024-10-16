using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Common.Mappings.Exams;

public class FontMappingProfile : Profile
{
    public FontMappingProfile()
    {
        CreateMap<FontDto, Font>().ReverseMap();
    }
}
