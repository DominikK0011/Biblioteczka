using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Contracts.DTO;

namespace Biblioteczka.Contracts.Response
{
    public record LoginResponse(string Token, UserDto User);
}
