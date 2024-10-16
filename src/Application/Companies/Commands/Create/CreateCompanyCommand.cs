using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models.Companies;

namespace CheetahExam.Application.Companies.Commands.Create;

public record CreateCompanyCommand : IRequest<Result>
{
    public required CompanyDto Company { get; init; }
}

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Result>
{
    #region Fields

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public CreateCompanyCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    public async Task<Result> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = new();

        try
        {
            Company company = _mapper.Map<Company>(request.Company);

            if (string.IsNullOrWhiteSpace(company.Name)) throw new BadRequestException("Name is not available");

            Company? existingCompany = _context.Companies
                                            .AsEnumerable()
                                            .FirstOrDefault(comp =>
                                                comp.Name != null &&
                                                comp.Name.Equals(company.Name, StringComparison.OrdinalIgnoreCase));

            if (existingCompany != null) throw new AlreadyExistsException("Company", request.Company.Name);

            _context.Companies.Add(company);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (NotFoundException nf) { errors.Add(nf.Message); }
        catch (AlreadyExistsException ae) { errors.Add(ae.Message); }
        catch (Exception ex) { errors.Add(ex.Message); }

        return errors.Any() ? Result.Failure(errors) : Result.Success();
    }

    #endregion
}
