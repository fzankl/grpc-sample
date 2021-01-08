# gRPC Sample
**Example how to work with gRPC using .NET Core/ASP.NET Core and Python.**

-------------------------------------

This example shows how to work with gRPC using .NET and Python. gRPC Server is provided by an ASP.NET Core backend. Each client shows then how to connect and interact with this server.

Examples consider all available communication patters of gRPC:
* Unary RPCs
* Server streaming RPCs
* Client streaming RPCs
* Bidirectional streaming RPCs

Current state of implementation: 

- ASP.NET Core Server
  - [x] Unary RPC
  - [x] Server streaming RPC
  - [x] Client streaming RPC
  - [x] Bidirectional streaming 
- ASP.NET Core Client
  - [x] Unary RPC
  - [x] Server streaming RPC
  - [x] Client streaming RPC
  - [x] Bidirectional streaming 
- Python Client (in progress)
  - [ ] Unary RPC
  - [ ] Server streaming RPC
  - [ ] Client streaming RPC
  - [ ] Bidirectional streaming 
  
Further information: 
 * English: https://www.fzankl.de/en/blog/distributed-services-using-grpc-in-dotnet-and-python
 * German: https://www.fzankl.de/de/blog/verteilte-services-mit-grpc-in-dotnet-und-python

## How to run this sample

To run this sample you have to start the ASP.NET Core Server project first. Navigate to `src\ASP.NET Core - GrpcServer` inside the repository and run following command (Requires .NET Core Runtime installed):

```bash
dotnet run
```

After that you can start the client you prefer, e.g. the .NET Core gRPC client application as shown in following figure:

![Output from .NET Core client application](https://user-images.githubusercontent.com/44210522/93888984-f6b6aa80-fce8-11ea-83fd-7a48e95dd4ef.jpg)
