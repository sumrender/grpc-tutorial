namespace GrpcGreeter
{
    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using FluentValidation;
    using Microsoft.Extensions.DependencyInjection;

    public class ValidationInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            if (request is IValidatable)
            {
                var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
                
                // Get the HttpContext from the ServerCallContext
                var httpContext = context.GetHttpContext();
                var scopedServiceProvider = httpContext.RequestServices;
                
                var validator = scopedServiceProvider.GetService(validatorType);

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

}