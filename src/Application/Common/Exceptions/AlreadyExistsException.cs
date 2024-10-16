namespace CheetahExam.Application.Common.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException() : base() { }

    public AlreadyExistsException(string value) : base($"{value} already exists.") { }

    public AlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

    public AlreadyExistsException(string name, object key) : base($"Entity {name}, ({key}) already exists.") { }
}
