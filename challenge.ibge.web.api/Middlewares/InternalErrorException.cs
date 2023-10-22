namespace challenge.ibge.web.api.Middlewares;

/// <summary>
/// Exception used to return the message and details of the exception
/// </summary>
public class InternalErrorException
{
    public string Message { get; private set; }
    public string Details { get; private set; }

    public InternalErrorException(string message, string details)
    {
        Message = message;
        Details = details;
    }
}