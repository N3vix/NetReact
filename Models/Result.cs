namespace Models;

public class Result<T>
{
    public bool Success { get; private set; }
    public string[] Errors { get; private set; }
    public T Value { get; private set; }

    public static Result<T> Failed(params string[] errors)
        => new() { Success = false, Errors = errors };

    public static Result<T> Successful(T value)
        => new() { Success = true, Value = value };

    public static implicit operator Result<T>(T value) => Result<T>.Successful(value);
}
