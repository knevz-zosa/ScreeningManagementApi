namespace Screening.Common.Wrapper;
public class ResponseWrapper<T>
{
    public bool IsSuccessful { get; set; }
    public List<string> Messages { get; set; } = new List<string>();
    public T Data { get; set; }
    public ResponseWrapper<T> Success(T data, string message = null)
    {
        IsSuccessful = true;
        Messages = message == null ? new List<string>() : new List<string> { message };
        Data = data;
        return this;
    }

    public ResponseWrapper<T> Failed(string message)
    {
        IsSuccessful = false;
        Messages = new List<string> { message };
        return this;
    }
}
public class ErrorResponse
{
    public string Title { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}
