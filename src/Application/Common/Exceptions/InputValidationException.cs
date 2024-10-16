namespace CheetahExam.Application.Common.Exceptions;

public class InputValidationException : Exception
{
    public InputValidationException() : base("Input validation failed.") { }

    public InputValidationException(string message) : base(message) { }

    public InputValidationException(string message, Exception innerException) : base(message, innerException) { }
}
