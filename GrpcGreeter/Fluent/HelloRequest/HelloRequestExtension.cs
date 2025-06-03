using System;

namespace GrpcGreeter
{
    public partial class HelloRequest: IValidatable
    {

        public string GetValue()
        {
            return $"Name: {Name}";
        }
    }
} 