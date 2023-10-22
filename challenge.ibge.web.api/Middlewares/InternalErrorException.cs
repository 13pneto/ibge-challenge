namespace challenge.ibge.web.api.Middlewares;

public class InternalErrorException
{
    public string Message { get; set; }
    public string Details { get; set; }

    public InternalErrorException(string message, string details)
    {
        Message = message;
        Details = details;
    }
}