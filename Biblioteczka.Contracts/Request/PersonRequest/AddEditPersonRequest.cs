using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Model.DataModel;

namespace Biblioteczka.Contracts.Request.PersonRequest
{
    public record AddEditPersonRequest(int? Id,string FirstName, string LastName, UserRole Role, DateOnly BirthDay, string PhotoUrl);
}
