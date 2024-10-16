namespace CheetahExam.Application.Common.Exceptions;

public class FunctionalException : Exception
{
    public FunctionalException() : base("A functional error has occurred.") { }

    public FunctionalException(string message) : base(message) { }

    public FunctionalException(string message, Exception innerException) : base(message, innerException) { }
}
