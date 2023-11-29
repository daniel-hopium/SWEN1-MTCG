using System;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException() : base("User with the same username already exists.")
    {
    }

    public UserAlreadyExistsException(string message) : base(message)
    {
    }

    public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}