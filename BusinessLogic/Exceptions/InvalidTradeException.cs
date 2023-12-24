namespace BusinessLogic.Exceptions;

public class InvalidTradeException : InvalidOperationException
{
    public InvalidTradeException(string message) : base(message)
    {
    }
}