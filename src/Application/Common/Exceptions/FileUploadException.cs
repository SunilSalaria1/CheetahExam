namespace CheetahExam.Application.Common.Exceptions;

public class FileUploadException : Exception
{
    public FileUploadException() : base("File upload failed.") { }

    public FileUploadException(string message) : base(message) { }

    public FileUploadException(string message, Exception innerException) : base(message, innerException) { }
}
