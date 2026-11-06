using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteczka.Contracts.Request
{
    public record LoginUserRequest(string Login, string Password);
}
