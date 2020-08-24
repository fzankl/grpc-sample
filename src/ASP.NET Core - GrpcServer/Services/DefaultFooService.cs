using System.Threading.Tasks;
using Grpc.Core;
using GrpcSamples;
using Microsoft.Extensions.Logging;

namespace GrpcServer
{
    public class DefaultFooService : FooService.FooServiceBase
    {
        private readonly ILogger<DefaultFooService> _logger;
        public DefaultFooService(ILogger<DefaultFooService> logger)
        {
            _logger = logger;
        }

        public override Task<FooResponse> GetFoo(FooRequest request, ServerCallContext context)
        {
            return Task.FromResult(new FooResponse
            {
                Message = $"Request message: {request.Message}"
            });
        }
    }
}
