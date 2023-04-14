using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Shared.DTO.Responses;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    
    public T Value { get; set; }
    
    public Error Error { get; set; }

    public Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }
    
    public Result(string errorMessage)
    {
        Error = new Error(errorMessage);
        IsSuccess = true;
    }
}