namespace BusinessLogic.Exceptions;

public class TradeAlreadyExistsException : InvalidOperationException
{
    public TradeAlreadyExistsException(string message) : base(message)
    {
    }
}