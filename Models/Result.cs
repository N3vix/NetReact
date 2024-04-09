namespace Models;

public readonly struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;

    public bool IsSuccess => !IsError;
    public bool IsError { get; }
    public TValue Value => _value;
    public TError Error => _error;

    private Result(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;
    }

    private Result(TError error)
    {
        IsError = true;
        _value = default;
        _error = error;
    }

    public static Result<TValue, TError> Successful(TValue value)
        => new(value);

    public static Result<TValue, TError> Failed(TError error)
        => new(error);

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);
}
