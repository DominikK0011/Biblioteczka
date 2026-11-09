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
        public Task<Result<PersonDto>> AddPersonAsync(AddEditPersonRequest person);
        public Task<Result<PersonDto?>> GetPersonAsync(Expression<Func<Person,bool>> filterPredicate);
        public Task<Result<IEnumerable<PersonDto>>> GetPersonsAsync(Expression<Func<Person, bool>>? filterPredicate = null);
        public Task<Result<PersonDto>> EditPersonAsync(AddEditPersonRequest person);
    }
}
