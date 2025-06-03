using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "I am grpc server. Name I received from client was: " + request.GetValue()
        });
    }

    public override Task<HelloReply> SayNumber(NumberRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "I am grpc server. Number I received from client was: " + request.GetValue()
        });
    }
}
