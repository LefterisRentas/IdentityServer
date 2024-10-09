namespace Identity.Server.Extended.Models;

/// <summary>
/// Operation result class for returning results of operations.
/// </summary>
/// <typeparam name="T"></typeparam>
public class OperationResult
{
    /// <summary>
    /// The result of the operation.
    /// </summary>
    public object? Result { get; }

    /// <summary>
    /// If the operation was successful.
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// The validation errors.
    /// </summary>
    public List<string> ValidationErrors { get; } = new();

    /// <summary>
    /// Create a new operation result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="isSuccess"></param>
    protected OperationResult(object? result, bool isSuccess)
    {
        Result = result;
        IsSuccess = isSuccess;
    }

    /// <summary>
    /// Create a new operation result.
    /// </summary>
    /// <param name="validationErrors"></param>
    protected OperationResult(List<string>? validationErrors = null)
    {
        ValidationErrors = validationErrors ?? new List<string>();
        IsSuccess = false;
    }

    /// <summary>
    /// Create a new success result.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static OperationResult<T?> Success<T>(T? result)
    {
        return new OperationResult<T?>(result, true);
    }
    
    /// <summary>
    /// Create a new success result.
    /// </summary>
    /// <returns></returns>
    public static OperationResult Success()
    {
        return new OperationResult(default, true);
    }

    /// <summary>
    /// Create a new failure result.
    /// </summary>
    /// <param name="validationErrors"></param>
    /// <returns></returns>
    public static OperationResult<T> Failure<T>(List<string> validationErrors)
    {
        return new OperationResult<T>(validationErrors);
    }
    
    /// <summary>
    /// Create a new failure result.
    /// </summary>
    /// <param name="validationErrors"></param>
    /// <returns></returns>
    public static OperationResult<T> Failure<T>(params string[] validationErrors)
    {
        return new OperationResult<T>(new List<string>(validationErrors));
    }
    
    /// <summary>
    /// Create a new failure result.
    /// </summary>
    /// <param name="validationErrors"></param>
    /// <returns></returns>
    public static OperationResult Failure(List<string> validationErrors)
    {
        return new OperationResult(validationErrors);
    }
    
    /// <summary>
    /// Create a new failure result.
    /// </summary>
    /// <param name="validationErrors"></param>
    /// <returns></returns>
    public static OperationResult Failure(params string[] validationErrors)
    {
        return new OperationResult(new List<string>(validationErrors));
    }

    /// <summary>
    /// If the result has validation errors.
    /// </summary>
    /// <returns></returns>
    public bool HasErrors()
    {
        return ValidationErrors.Count > 0;
    }
}

/// <summary>
/// <see cref="OperationResult"/> with a result of type <see cref="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public class OperationResult<T> : OperationResult
{
    /// <summary>
    /// The result of the operation.
    /// </summary>
    public new T? Result => (T?)base.Result;

    /// <summary>
    /// Create a new operation result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="isSuccess"></param>
    public OperationResult(T result, bool isSuccess) : base(result, isSuccess)
    {
    }

    /// <summary>
    /// Create a new operation result.
    /// </summary>
    /// <param name="validationErrors"></param>
    public OperationResult(List<string> validationErrors) : base(validationErrors)
    {
    }
}
