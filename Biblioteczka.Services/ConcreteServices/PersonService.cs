using System.Linq.Expressions;
using AutoMapper;
using Biblioteczka.API.Data;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request.PersonRequest;
using Biblioteczka.Model.DataModel;
using Biblioteczka.Services.Interfaces;
using Biblioteczka.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Biblioteczka.Services.ConcreteServices
{
    public class PersonService : BaseService, IPersonService
    {
        public PersonService(AppDbContext context, ILogger<PersonService> logger, IMapper mapper) 
            : base(context, logger, mapper)
        {
        }

        public async Task<Result<PersonDto>> AddPersonAsync(AddEditPersonRequest person)
        {
            var p = mapper.Map<Person>(person);

            await context.People.AddAsync(p);

            await context.SaveChangesAsync();

            var personDto = mapper.Map<PersonDto>(p);

            return Result<PersonDto>.Success(personDto);
        }

        public async Task<Result<PersonDto>> EditPersonAsync(AddEditPersonRequest personData)
        {
            if (!personData.Id.HasValue)
                return Result<PersonDto>.Failure(new Error.Validation("Id is null in personData to edit"));

            var existingPerson = await context.People.FirstOrDefaultAsync(person => person.Id == personData.Id.Value);

            if (existingPerson == null)
                return Result<PersonDto>.Failure(new Error.NotFound(nameof(Person), personData.Id.Value));

            mapper.Map(personData, existingPerson);

            await context.SaveChangesAsync();
            var personDto = mapper.Map<PersonDto>(existingPerson);

            return Result<PersonDto>.Success(personDto);
        }

        public async Task<Result<PersonDto?>> GetPersonAsync(Expression<Func<Person, bool>> filterPredicate)
        {
            if (filterPredicate == null)
                return Result<PersonDto?>.Failure(new Error.Validation("filter predicate is null"));

            var person = await context.People.FirstOrDefaultAsync(filterPredicate);

            if (person == null)
                return Result<PersonDto?>.Success(null);

            var personDto = mapper.Map<PersonDto>(person);

            return Result<PersonDto?>.Success(personDto);
        }

        public async Task<Result<IEnumerable<PersonDto>>> GetPersonsAsync(Expression<Func<Person, bool>>? filterPredicate = null)
        {
            var persons = context.People.AsQueryable();

            if (filterPredicate != null)
                persons = persons.Where(filterPredicate);

            var personsList = await persons.ToListAsync();
            var personsDto = mapper.Map<IEnumerable<PersonDto>>(personsList);

            return Result<IEnumerable<PersonDto>>.Success(personsDto);
        }
    }
}
