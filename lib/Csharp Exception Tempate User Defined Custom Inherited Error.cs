using System;

public class CustomAppException: Exception
{
    public CustomAppException()
    {
    }

    public CustomAppException(string message)
        : base(message)
    {
    }

    public CustomAppException(string message, Exception inner)
        : base(message, inner)
    {
    }
}