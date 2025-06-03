using GrpcGreeter.Services;
using FluentValidation;
using GrpcGreeter;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc(options =>
        {
            options.Interceptors.Add<ValidationInterceptor>();
        });

        // Register validation services
        builder.Services.AddValidatorsFromAssemblyContaining<HelloRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<NumberRequestValidator>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<GreeterService>();

        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}