using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteczka.Shared
{
    public abstract record Error(string Code, string Message)
    {
        public sealed record NotFound(string EntityName, int Id) : Error("NotFound", $"{EntityName} with ID {Id} was not found.");
        public sealed record Validation(string Message) : Error("ValidationError", Message);
        public sealed record Conflict(string Message) : Error("ConflictError", Message);
    }
}
