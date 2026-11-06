using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Model.DataModel;

namespace Biblioteczka.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
