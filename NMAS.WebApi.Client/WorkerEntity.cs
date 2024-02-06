using System.ComponentModel.DataAnnotations;

namespace NMAS.WebApi.Client
{
    public class WorkerEntity
    {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string PersonalIdentityCode { get; set; }
            [Required]
            public string ContactEmail { get; set; }
            [Required]
            public string ContactPhone { get; set; }
    }
}
