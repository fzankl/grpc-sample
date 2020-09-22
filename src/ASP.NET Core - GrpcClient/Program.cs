using Grpc.Core;
using Grpc.Net.Client;
using GrpcSamples;
using System;
using System.Collections.Generic;
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

            ConsoleKey choice;

            while((choice = ShowApplicationInformation()) != ConsoleKey.Escape)
            {
                if (choice == ConsoleKey.D1)
                {
                    await RunUnaryRpc(client).ConfigureAwait(false);
                }
                else if (choice == ConsoleKey.D2)
                {
                    await RunServerStreamingRpc(client).ConfigureAwait(false);
                }
                else if (choice == ConsoleKey.D3)
                {
                    await RunClientStreamingRpc(client).ConfigureAwait(false);
                }
                else if (choice == ConsoleKey.D4)
                {
                    await RunBidirectionalRpc(client).ConfigureAwait(false);
                }
                else
                {
                    Console.WriteLine("Invalid option selected.\n");
                }
            }
        }

        private static async Task RunUnaryRpc(FooService.FooServiceClient client)
        {
            WriteLine("Starting unary RPC example...\n", ConsoleColor.Blue);

            // Synchronous unary RPC
            var fooSynchronousRequest = new FooRequest { Message = "Sample synchronous request" };
            var fooSynchronousResponse = client.GetFoo(fooSynchronousRequest);

            // Asynchronous unary RPC
            var fooAsynchronousRequest = new FooRequest { Message = "Sample asynchronous request" };
            var fooAsynchronousResponse = await client.GetFooAsync(fooAsynchronousRequest);

            Console.WriteLine($"\t{fooSynchronousResponse.Message}");
            Console.WriteLine($"\t{fooAsynchronousResponse.Message}\n");

            WriteLine("Finished unary RPC example...", ConsoleColor.Blue);
            WriteSeparator();
        }

        private static async Task RunServerStreamingRpc(FooService.FooServiceClient client)
        {
            WriteLine("Starting server streaming RPC example...\n", ConsoleColor.Blue);

            Console.WriteLine("You will be requested for an amount of messages that should be streamed to client.");
            Console.WriteLine("gRPC Server will then stream this amount of messages before it will finish the request.");

            WriteRequestInput("Amount of messages:");
            var result = Convert.ToInt32(Console.ReadLine());

            var fooRequest = new FooServerStreamingRequest { Message = "Sample request", MessageCount = result };
            var serverStreamingCall = client.GetFoos(fooRequest);

            Console.WriteLine($"\n\tgRPC Server responses:");

            await foreach (var response in serverStreamingCall.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"\t> {response.Message}");
            }

            WriteLine("\nFinished server streaming RPC example...", ConsoleColor.Blue);
            WriteSeparator();
        }

        private static async Task RunClientStreamingRpc(FooService.FooServiceClient client)
        {
            WriteLine("Starting client streaming RPC example...\n", ConsoleColor.Blue);

            Console.WriteLine("You will be requested for messages that should be streamed to server.");
            Console.WriteLine("Each message will be streamed to server until request client requests finishes.");
            Console.WriteLine("To finish the request just leave the message empty.");

            var clientStreamingCall = client.SendFoos(new CallOptions());

            while (true)
            {
                WriteRequestInput("Enter a message that should be streamed to gRPC server:");
                var result = Console.ReadLine();

                if (string.IsNullOrEmpty(result))
                {
                    await clientStreamingCall.RequestStream.CompleteAsync().ConfigureAwait(false);
                    break;
                }

                var fooRequest = new FooRequest { Message = result };
                await clientStreamingCall.RequestStream.WriteAsync(fooRequest).ConfigureAwait(false);
            }

            var response = await clientStreamingCall.ResponseAsync.ConfigureAwait(false);
            Console.WriteLine($"\n\tgRPC Server response:\n\t> {response.Message}\n");

            WriteLine("Finished client streaming RPC example...", ConsoleColor.Blue);
            WriteSeparator();
        }

        private static async Task RunBidirectionalRpc(FooService.FooServiceClient client)
        {
            WriteLine("Starting bidirectional streaming RPC example......\n", ConsoleColor.Blue);

            Console.WriteLine("You will be requested for messages that should be streamed to server.");
            Console.WriteLine("Each message will be streamed to server until request client requests finishes.");
            Console.WriteLine("To finish the request just leave the message empty.");

            var receivedMessages = new List<string>();
            var bidirectionalCall = client.SendAndGetFoos();

            var readTask = Task.Run(async () =>
            {
                await foreach (var response in bidirectionalCall.ResponseStream.ReadAllAsync())
                {
                    receivedMessages.Add(response.Message);
                }
            });

            while (true)
            {
                WriteRequestInput("Enter a message that should be streamed to gRPC server:");
                var result = Console.ReadLine();

                if (string.IsNullOrEmpty(result))
                {
                    break;
                }

                await bidirectionalCall.RequestStream.WriteAsync(new FooRequest { Message = result });
            }

            await bidirectionalCall.RequestStream.CompleteAsync();

            Console.WriteLine($"\n\tgRPC Server responses:");

            foreach (var receivedMessage in receivedMessages)
            {
                Console.WriteLine($"\t> {receivedMessage}");
            }

            WriteLine("\nFinished bidirectional streaming RPC example...", ConsoleColor.Blue);
            WriteSeparator();
        }

        private static ConsoleKey ShowApplicationInformation()
        {
            Console.WriteLine("gRPC");
            Console.WriteLine("Example: How to use gRPC in various programming languages");
            Console.WriteLine("\n-------------------------------------------------------\n");

            Console.WriteLine("Please select gRPC communication mode:\n");
            Console.WriteLine(" > 1 = Unary RPC");
            Console.WriteLine(" > 2 = Server streaming RPC");
            Console.WriteLine(" > 3 = Client streaming RPC");
            Console.WriteLine(" > 4 = Bidirectional streaming RPC");
            Console.WriteLine();
            Console.WriteLine(" > Esc = Exit the application");

            WriteSeparator();
            Console.Write("Mode: ");
            var choice = Console.ReadKey();
            Console.WriteLine();
            WriteSeparator();

            return choice.Key;
        }

        private static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void WriteRequestInput(string text)
        {
            Console.WriteLine($"\n\t{text}");
            Console.Write("\t> ");
        }

        private static void WriteSeparator()
        {
            Console.WriteLine("\n-------------------------------------------------------\n");
        }
    }
}
