namespace Application.Common.Models.Responses;
public class Result<T>
{
    public bool IsSuccess { get; } = true;
    public T Data { get; }

    private Result(T data)
    {
        Data = data;
    }

    public static Result<T> Success(T data)
    {
        return new Result<T>(data);
    }
}
