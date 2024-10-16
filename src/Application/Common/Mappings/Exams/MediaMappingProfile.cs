using CheetahExam.WebUI.Shared.Common.Models;

namespace CheetahExam.Application.Common.Mappings.Exams
{
    public class MediaMappingProfile : Profile
    {
        public MediaMappingProfile()
        {
            CreateMap<Media, MediaDto>()
                .ReverseMap()
                .ForMember(destination => destination.UniqueId, source => source.Condition(source => !string.IsNullOrEmpty(source.UniqueId)));
        }
    }
}
