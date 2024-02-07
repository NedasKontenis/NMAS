using System;

namespace NMAS.WebApi.Contracts.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message)
            : base(message)
        {
        }
    }
}
