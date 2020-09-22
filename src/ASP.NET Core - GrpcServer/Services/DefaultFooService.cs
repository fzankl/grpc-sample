using System;
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

        /// <summary>
        /// Unary RPC
        /// </summary>
        public override Task<FooResponse> GetFoo(FooRequest request, ServerCallContext context)
        {
            var response = new FooResponse
            {
                Message = $"Request message: {request.Message}"
            };

            return Task.FromResult(response);
        }

        /// <summary>
        /// Server streaming RPC
        /// </summary>
        public override async Task GetFoos(FooServerStreamingRequest request, IServerStreamWriter<FooResponse> responseStream, ServerCallContext context)
        {
            var count = 0;

            while(count < request.MessageCount && !context.CancellationToken.IsCancellationRequested)
            {
                count++;

                var response = new FooResponse
                {
                    Message = $"Request message: {request.Message} ({count})"
                };

                await responseStream.WriteAsync(response).ConfigureAwait(false);
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Client streaming RPC
        /// </summary>
        public override async Task<FooResponse> SendFoos(IAsyncStreamReader<FooRequest> requestStream, ServerCallContext context)
        {
            var messageCount = 0;

            while (await requestStream.MoveNext().ConfigureAwait(false))
            {
                messageCount++;

                var request = requestStream.Current;
                Console.WriteLine(request.Message);
            }

            return new FooResponse
            {
                Message = $"Received {messageCount} messages."
            };
        }

        /// <summary>
        /// Bidirectional streaming RPC
        /// </summary>
        public override async Task SendAndGetFoos(IAsyncStreamReader<FooRequest> requestStream, IServerStreamWriter<FooResponse> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext().ConfigureAwait(false))
            {
                var response = new FooResponse
                {
                    Message = $"Request message: {requestStream.Current.Message}"
                };

                await responseStream.WriteAsync(response).ConfigureAwait(false);
            }
        }
    }
}
