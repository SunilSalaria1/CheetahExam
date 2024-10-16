using CheetahExam.WebUI.Shared.Common.Models.Exams;

namespace CheetahExam.Application.Exams.Commands.Create
{
    public record CreateExamCommand : IRequest<string>
    {
        public ExamDto? Exam { get; set; }
    }

    public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, string>
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        #endregion

        #region Ctor

        public CreateExamCommandHandler(
            IMapper mapper,
            IApplicationDbContext context
            )
        {
            _mapper = mapper;
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<string> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            var exam = _mapper.Map<Exam>(request.Exam);

            if (request.Exam?.ExamResultOptions?.Count > 0)
            {
                exam.ExamResultOptions = _mapper.Map<List<ExamResultOption>>(request.Exam.ExamResultOptions);
            }

            await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync(cancellationToken);

            return exam.UniqueId;
        }
        #endregion
    }
}
