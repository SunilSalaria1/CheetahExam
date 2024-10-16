using CheetahExam.WebUI.Shared.Common.Models.Exams;
using static CheetahExam.WebUI.Shared.Constants.Constant;

namespace CheetahExam.Application.Exams.Commands.Update
{
    public record UpdateExamCommand : IRequest<string>
    {
        public ExamDto? Exam { get; set; }
    }

    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, string>
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        #endregion

        #region Ctor

        public UpdateExamCommandHandler(
            IMapper mapper,
            IApplicationDbContext context
            )
        {
            _mapper = mapper;
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<string> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            var existingExam = await _context.Exams
                .Include(exam => exam.Media)
                .Include(exam => exam.ExamResultOptions)
                .Include(exam => exam.Questions)
                .Where(exam => exam.UniqueId == request.Exam.UniqueId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingExam is null)
                return CommandsReturnStatus.NotFound;

            existingExam = _mapper.Map(request.Exam, existingExam);

            if (request.Exam?.ExamResultOptions?.Count > 0)
            {
                existingExam.ExamResultOptions = _mapper.Map<List<ExamResultOption>>(request.Exam.ExamResultOptions);
            }
            else {
                // Remove existing ExamResultOptions
                existingExam.ExamResultOptions.Clear();
            }

            _context.Exams.Update(existingExam);
            await _context.SaveChangesAsync(cancellationToken);

            return CommandsReturnStatus.Updated;
        }

        #endregion
    }
}
