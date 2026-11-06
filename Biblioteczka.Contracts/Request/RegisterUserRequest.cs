using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteczka.Contracts.Request
{
    public record RegisterUserRequest(string Email, string Username, string Password);
}
