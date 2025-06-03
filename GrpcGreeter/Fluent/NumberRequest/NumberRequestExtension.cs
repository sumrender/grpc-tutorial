using System;

namespace GrpcGreeter
{

    public partial class NumberRequest: IValidatable
    {

        public string GetValue()
        {
            return $"Number: {Num}";
        }
    }
} 