using System.Collections.Generic;

namespace NMAS.WebApi.Contracts.Responses
{
    public class BadRequestResponse
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public BadRequestResponse(string message, List<string> errors = null)
        {
            Message = message;
            Errors = errors ?? new List<string>();
        }
    }
}
