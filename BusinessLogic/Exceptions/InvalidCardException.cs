namespace BusinessLogic.Exceptions;

public class InvalidCardException : InvalidOperationException
{
    public InvalidCardException(string message) : base(message)
    {
    }
    
}