using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Identity.Server.Extended.Models;

/// <summary>
/// Represents the result of an operation, including success state and validation errors.
/// </summary>
public class OperationResult
{
    /// <summary>The result of the operation.</summary>
    public object? Result { get; }

    /// <summary>If the operation was successful.</summary>
    public bool IsSuccess { get; private set; }

    /// <summary>The validation errors associated with the operation.</summary>
    public Dictionary<string, List<string>> ValidationErrors { get; } = new();

    /// <summary>Creates a new operation result.</summary>
    /// <param name="result">The result object of the operation.</param>
    /// <param name="isSuccess">Indicates if the operation was successful.</param>
    protected OperationResult(object? result, bool isSuccess)
    {
        Result = result;
        IsSuccess = isSuccess;
    }

    /// <summary>Creates a new operation result with validation errors.</summary>
    /// <param name="validationErrors">A dictionary of validation errors.</param>
    protected OperationResult(Dictionary<string, List<string>>? validationErrors = null)
    {
        if (validationErrors != null)
        {
            ValidationErrors = validationErrors;
        }
        IsSuccess = false;
    }

    /// <summary>Creates a success result with a specified result.</summary>
    public static OperationResult<T?> Success<T>(T? result)
    {
        return new OperationResult<T?>(result, true);
    }

    /// <summary>Creates a generic success result.</summary>
    public static OperationResult Success()
    {
        return new OperationResult(null, true);
    }

    /// <summary>Creates a failure result with validation errors.</summary>
    public static OperationResult Failure(Dictionary<string, List<string>> validationErrors)
    {
        return new OperationResult(validationErrors);
    }
    
    /// <summary>Creates a failure result with validation errors.</summary>
    public static OperationResult<T> Failure<T>(Dictionary<string, List<string>> validationErrors)
    {
        return new OperationResult<T>(validationErrors);
    }

    /// <summary>Checks if the result has any validation errors.</summary>
    public bool HasErrors()
    {
        return ValidationErrors.Count > 0;
    }

    /// <summary>
    /// Returns the validation errors as a <see cref="ValidationProblem"/>. 
    /// If no errors or the operation was successful, returns null.
    /// </summary>
    public ValidationProblem? ToValidationProblem()
    {
        if (!HasErrors() || IsSuccess)
        {
            return null;
        }

        // Directly use ValidationErrors for field-specific errors
        return TypedResults.ValidationProblem(
            ValidationErrors.ToDictionary(k => k.Key, v => v.Value.ToArray())
        );
    }
}

/// <summary>
/// Represents the result of an operation with a specified result type.
/// </summary>
/// <typeparam name="T">The type of the operation result.</typeparam>
public class OperationResult<T> : OperationResult
{
    /// <summary>The result of the operation.</summary>
    public new T? Result => (T?)base.Result;

    /// <summary>Creates a new operation result.</summary>
    public OperationResult(T result, bool isSuccess) : base(result, isSuccess)
    {
    }

    /// <summary>Creates a new operation result with validation errors.</summary>
    public OperationResult(Dictionary<string, List<string>> validationErrors) : base(validationErrors)
    {
    }
}
