namespace NMAS.WebApi.Contracts.Responses
{
    public class UnauthorizedResponse
    {
        public string Message { get; set; }

        public UnauthorizedResponse(string message)
        {
            Message = message;
        }
    }
}