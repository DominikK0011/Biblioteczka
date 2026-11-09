using AutoMapper;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Contracts.Request.PersonRequest;
using Biblioteczka.Model.DataModel;

namespace Biblioteczka.Services.Configuration.AutoMapper
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<Movie, MovieDto>();

            CreateMap<AddUpdateMovieDto, Movie>();

            CreateMap<AddUpdateUserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<RegisterUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();

            CreateMap<Person, PersonDto>();
            CreateMap<AddEditPersonRequest, Person>();
        }
    }
}
