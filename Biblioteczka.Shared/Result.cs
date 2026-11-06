using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Biblioteczka.Shared
{
    public readonly record struct Result<T>(T? Value, Error? Error)
    {
        public bool IsSuccess => Error is null;
        public static Result<T> Success(T value) => new Result<T>(value, null);
        public static Result<T> Failure(Error e) => new Result<T>(default, e);
        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
        {
            return IsSuccess
                ? onSuccess(Value!)
                : onFailure(Error!);
        }
    }
}