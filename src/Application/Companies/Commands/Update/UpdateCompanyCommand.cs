using CheetahExam.Application.Common.Exceptions;
using CheetahExam.WebUI.Shared.Common;
using CheetahExam.WebUI.Shared.Common.Models.Companies;

namespace CheetahExam.Application.Companies.Commands.Update
{
    public record UpdateCompanyCommand : IRequest<Result>
    {
        public required CompanyDto Company { get; init; }
    }

    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Result>
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        #endregion

        #region Ctor

        public UpdateCompanyCommandHandler(
            IMapper mapper,
            IApplicationDbContext context
            )
        {
            _mapper = mapper;
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<Result> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            List<string> errors = new();

            try
            {
                if (string.IsNullOrWhiteSpace(request.Company.UniqueId)) throw new BadRequestException("Unique Id is not available");

                var company = await _context.Companies
                                                .Include(company => company.Media)
                                                .Where(company => company.UniqueId == request.Company.UniqueId)
                                                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Company", request.Company.UniqueId);

                company = _mapper.Map(request.Company, company);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (BadRequestException br) { errors.Add(br.Message); }
            catch (NotFoundException nf) { errors.Add(nf.Message); }
            catch (Exception ex) { errors.Add(ex.Message); }


            return errors.Any() ? Result.Failure(errors) : Result.Success();
        }

        #endregion
    }
}
