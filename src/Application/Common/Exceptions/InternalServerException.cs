namespace CheetahExam.Application.Common.Exceptions;

public class InternalServerException : Exception
{
    public InternalServerException() : base("An internal server error has occurred.") { }

    public InternalServerException(string message) : base(message) { }

    public InternalServerException(string message, Exception innerException) : base(message, innerException) { }
}
