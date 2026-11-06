using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using Biblioteczka.API.Data;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request.PersonRequest;
using Biblioteczka.Model.DataModel;
using Biblioteczka.Services.Interfaces;
using Biblioteczka.Shared;
using Microsoft.Extensions.Logging;

namespace Biblioteczka.Services.ConcreteServices
{
    public class PersonService : BaseService, IPersonService
    {
        public PersonService(AppDbContext context, ILogger logger, IMapper mapper) 
            : base(context, logger, mapper)
        {
        }

        public async Task<Result<PersonDto>> AddPerson(AddEditPersonRequest person)
        {
            var p = mapper.Map<Person>(person);

            context.People.Add(p);

            await context.SaveChangesAsync();

            var personDto = mapper.Map<PersonDto>(p);

            return Result<PersonDto>.Success(personDto);
        }

        public async Task<Result<PersonDto>> EditPerson(AddEditPersonRequest person)
        {
            var p = mapper.Map<Person>(person);

            if (person.Id.HasValue)
                context.People.Update(p);
            else
                return Result<PersonDto>.Failure(new Error.Validation("Entity don't have Id"));

            await context.SaveChangesAsync();

            var personDto = mapper.Map<PersonDto>(p);

            return Result<PersonDto>.Success(personDto);
        }

        public Task<Result<PersonDto>> GetPerson(Expression<Func<Person, bool>> filterPredicate)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<PersonDto>>> GetPersons(Expression<Func<Person, bool>>? filterPredicate = null)
        {
            throw new NotImplementedException();
        }
    }
}
