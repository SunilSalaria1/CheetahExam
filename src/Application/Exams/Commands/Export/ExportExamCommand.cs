using CheetahExam.Application.Common.Services.File;

namespace CheetahExam.Application.Exams.Commands.Export;

public class ExportExamCommand : IRequest<MemoryStream>
{
    public required string ExamId { get; set; }
}

public class ExportExamCommandHandler : IRequestHandler<ExportExamCommand, MemoryStream>
{
    #region Fields

    private readonly IFileService _fileService;

    #endregion

    #region Ctor

    public ExportExamCommandHandler(IFileService fileService)
    {
        _fileService = fileService;
    }
    #endregion

    #region Methods

    public async Task<MemoryStream> Handle(ExportExamCommand request, CancellationToken cancellationToken)
    {
        return await _fileService.ExportExam(examId: request.ExamId);
    }

    #endregion
}
