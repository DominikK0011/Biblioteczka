using Microsoft.Extensions.Logging;
using Biblioteczka.API.Data;
using AutoMapper;

namespace Biblioteczka.Services.ConcreteServices
{
    public class BaseService
    {
        protected readonly AppDbContext context;
        protected readonly ILogger logger;
        protected readonly IMapper mapper;

        public BaseService(AppDbContext context, ILogger logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }
    }
}
