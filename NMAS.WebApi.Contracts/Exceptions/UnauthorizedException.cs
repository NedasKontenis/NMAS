using System;

namespace NMAS.WebApi.Contracts.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}