# 🚀 gRPC Payload Validation Interceptor (.NET Core)

This engineering document provides a step-by-step guide for implementing centralized payload validation using a **gRPC interceptor** in a .NET backend. It uses [FluentValidation](https://docs.fluentvalidation.net/) to cleanly validate requests before they reach your service logic.

The example is based on the official [ASP.NET Core gRPC Greeter tutorial](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-9.0&tabs=visual-studio).

---

## 📦 Prerequisites

- .NET 6.0 or later (recommended: .NET 8 / 9)
- A gRPC backend project (e.g., with GreeterService)
- NuGet packages:
  - `FluentValidation`
  - `FluentValidation.DependencyInjectionExtensions`

---

## 🛠️ Step-by-Step Setup

### 1. Install Required Packages

```bash
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions
```

---

### 2. Define a Marker Interface

Create a new file `IValidatable.cs`:

```csharp
public interface IValidatable { }
```

---

### 3. Implement Marker on gRPC Request

Your gRPC-generated request types are `partial` classes. Extend them to implement the marker interface:

```csharp
// HelloRequestExtensions.cs
public partial class HelloRequest : IValidatable { }
```

---

### 4. Add a Validator

Create a validator for the request using FluentValidation:

```csharp
using FluentValidation;

public class HelloRequestValidator : AbstractValidator<HelloRequest>
{
    public HelloRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}
```

---

### 5. Create the Validation Interceptor

```csharp
using Grpc.Core;
using Grpc.Core.Interceptors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public class ValidationInterceptor : Interceptor
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        if (request is IValidatable)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
            var validator = _serviceProvider.GetService(validatorType);

            if (validator is IValidator validatorInstance)
            {
                var validationContext = new ValidationContext<TRequest>(request);
                var result = await validatorInstance.ValidateAsync(validationContext);
                
                if (!result.IsValid)
                {
                    var errorMsg = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                    throw new RpcException(new Status(StatusCode.InvalidArgument, errorMsg));
                }
            }
        }

        return await continuation(request, context);
    }
}
```

---

### 6. Register Everything in `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add gRPC
builder.Services.AddGrpc();

// Register validation services
builder.Services.AddValidatorsFromAssemblyContaining<HelloRequestValidator>();
builder.Services.AddSingleton<ValidationInterceptor>();

var app = builder.Build();

// Register GreeterService with the interceptor
app.MapGrpcService<GreeterService>()
    .AddInterceptor<ValidationInterceptor>();

// Default endpoint
app.MapGet("/", () => "Use a gRPC client to communicate with the gRPC endpoints.");

app.Run();
```

---

### 7. Test the Validation

Call the `SayHello` method with an empty `Name`:

```json
{
  "name": ""
}
```

You should receive an error like:

```
StatusCode: InvalidArgument
Detail: Name is required.
```

---

## 📂 Project Structure

```
GrpcValidationDemo/
├── Protos/
│   └── greet.proto
├── Services/
│   └── GreeterService.cs
├── Validation/
│   ├── IValidatable.cs
│   ├── HelloRequestValidator.cs
│   └── ValidationInterceptor.cs
├── Program.cs
└── GrpcValidationDemo.csproj
```

---

## 🧪 Sample .proto File (greet.proto)

```proto
syntax = "proto3";

option csharp_namespace = "GrpcValidationDemo";

service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
}

message HelloRequest {
  string name = 1;
}

message HelloReply {
  string message = 1;
}
```

---

## ✅ Benefits of This Design

* ✅ Clean and readable service methods
* ✅ Centralized and reusable validation logic
* ✅ Interceptor pattern ensures consistency across services
* ✅ Seamless integration with FluentValidation

---

## 🔗 References

* [Microsoft gRPC Docs](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-9.0)
* [FluentValidation Docs](https://docs.fluentvalidation.net/)
* [gRPC Interceptors Guide](https://learn.microsoft.com/en-us/aspnet/core/grpc/interceptors)

---

## 👏 Done!

You've now implemented centralized request validation in your gRPC server using FluentValidation and interceptors. 🎉

Let your service logic focus on business rules—not request format checking.

```
