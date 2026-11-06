using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request.PersonRequest;
using Biblioteczka.Model.DataModel;
using Biblioteczka.Shared;

namespace Biblioteczka.Services.Interfaces
{
    public interface IPersonService
    {
        public Task<Result<PersonDto>> AddPerson(AddEditPersonRequest person);
        public Task<Result<PersonDto>> GetPerson(Expression<Func<Person,bool>> filterPredicate);
        public Task<Result<IEnumerable<PersonDto>>> GetPersons(Expression<Func<Person, bool>>? filterPredicate = null);
        public Task<Result<PersonDto>> EditPerson(AddEditPersonRequest person);
    }
}
