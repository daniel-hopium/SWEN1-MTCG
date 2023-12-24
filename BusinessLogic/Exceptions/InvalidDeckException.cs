namespace BusinessLogic.Exceptions;

public class InvalidDeckException : InvalidOperationException
{
    public InvalidDeckException(string message) : base(message)
    {
    }
}