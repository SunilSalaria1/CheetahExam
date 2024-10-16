using CheetahExam.WebUI.Shared.Common.Models.Companies;

namespace CheetahExam.Application.Common.Mappings.Companies;

public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<CompanyDto, Company>()
            .ForMember(
                destination => destination.UniqueId,
                source => source.Condition(source => !string.IsNullOrEmpty(source.UniqueId)))
            .ReverseMap();
    }
}
