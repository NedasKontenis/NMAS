using NMAS.WebApi.Contracts.Response;

namespace NMAS.WebApi.Contracts.WorkerEntity
{
    public class WorkerEntityCreated : BaseResponse
    {
        public int Id { get; set; }

        public WorkerEntityCreated(int id, string message = "Worker created successfully.")
            : base(true, message)
        {
            Id = id;
        }
    }
}
