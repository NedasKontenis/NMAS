using System.Collections.Generic;

namespace NMAS.WebApi.Contracts.Response
{
    public class BadRequestResponse : BaseResponse
    {
        public List<string> Errors { get; set; }

        public BadRequestResponse(string message, List<string> errors = null)
            : base(false, message)
        {
            Errors = errors ?? new List<string>();
        }
    }
}
