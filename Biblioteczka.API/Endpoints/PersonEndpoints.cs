using Biblioteczka.Contracts.Request.PersonRequest;
using Biblioteczka.Services.Interfaces;

namespace Biblioteczka.API.Endpoints
{
    public static class PersonEndpoints
    {
        public static void MapPersonEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/people").WithTags("People");

            group.MapGet("/", async (IPersonService personService) =>
            {
                var result = await personService.GetPersonsAsync();

                return result.Match(
                    onSuccess: person => Results.Ok(person),
                    onFailure: err => Results.BadRequest( new { message = err.Message })
                    );
            });

            group.MapGet("/{id:int}", async (int id, IPersonService personService) =>
            {
                var result = await personService.GetPersonAsync(p => p.Id == id);

                return result.Match(
                    onSuccess: person => person is not null 
                        ? Results.Ok(person)
                        : Results.NotFound(new {message = "Person not found"}),
                    onFailure: err => Results.NotFound(err)
                    );
            });

            group.MapPost("/", async (AddEditPersonRequest addEditPersonRequest, IPersonService personService) => 
            {
                var result = await personService.AddPersonAsync(addEditPersonRequest);

                return result.Match(
                    onSuccess: person => Results.Created(),
                    onFailure: err => Results.BadRequest(new {message = err.Message})
                    );
            }).DisableAntiforgery().RequireAuthorization(policy => policy.RequireRole("Admin"));

            group.MapPut("/", async (AddEditPersonRequest addEditPersonRequest, IPersonService personService) =>
            {
                var result = await personService.EditPersonAsync(addEditPersonRequest);

                return result.Match(
                    onSuccess: person => Results.Ok(person),
                    onFailure: err => Results.BadRequest(new { message = err.Message})
                    );
            }).DisableAntiforgery().RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
