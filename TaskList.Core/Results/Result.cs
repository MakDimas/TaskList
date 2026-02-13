namespace TaskList.Core.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }
    public ResultErrorType? ErrorType { get; }

    protected Result(bool isSuccess, string? error, ResultErrorType? errorType)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorType = errorType;
    }

    public static Result Success() => new(true, null, null);
    public static Result Failure(string error, ResultErrorType errorType) => new(false, error, errorType);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base(true, null, null)
    {
        Value = value;
    }

    private Result(string error, ResultErrorType errorType) : base(false, error, errorType)
    {
    }

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Failure(string error, ResultErrorType errorType) => new(error, errorType);
}