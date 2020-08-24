using Grpc.Net.Client;
using GrpcSamples;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    public class Program
    {
        public static async Task Main()
        {
            var options = new GrpcChannelOptions();
            var channel = GrpcChannel.ForAddress("https://localhost:5001", options);
            var client = new FooService.FooServiceClient(channel);

            // Synchronous unary RPC
            var fooSynchronousRequest = new FooRequest { Message = "Sample synchronous request" };
            var fooSynchronousResponse = client.GetFoo(fooSynchronousRequest);

            // Asynchronous unary RPC
            var fooAsynchronousRequest = new FooRequest { Message = "Sample asynchronous request" };
            var fooAsynchronousResponse = await client.GetFooAsync(fooAsynchronousRequest);

            Console.WriteLine(fooSynchronousResponse.Message);
            Console.WriteLine(fooAsynchronousResponse.Message);
            Console.ReadLine();
        }
    }
}
