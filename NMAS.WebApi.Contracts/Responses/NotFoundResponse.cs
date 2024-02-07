namespace NMAS.WebApi.Contracts.Responses
{
    public class NotFoundResponse
    {
        public string Message { get; set; }

        public NotFoundResponse(string message)
        {
            Message = message;
        }
    }
}
