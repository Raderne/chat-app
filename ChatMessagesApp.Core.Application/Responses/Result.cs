namespace ChatMessagesApp.Core.Application.Responses;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; set; }
    public T? Value { get; set; }

    protected Result(T value, bool isSuccess, string? error)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException("Cannot have an error when success is true.");
        if (!isSuccess && value != null)
            throw new InvalidOperationException("Cannot have a value when success is false.");

        IsSuccess = isSuccess;
        Error = error;
        Value = value;
    }

    public static Result<T> Success(T value) => new Result<T>(value, true, null);
    public static Result<T> Failure(string error) => new Result<T>(default!, false, error);
}
