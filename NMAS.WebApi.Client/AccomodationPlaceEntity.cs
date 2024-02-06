using System.ComponentModel.DataAnnotations;

namespace NMAS.WebApi.Client
{
    public class AccomodationPlaceEntity
    {
        [Required]
        public int WorkerID { get; set; }
        [Required]
        public string PlaceName { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public int AccomodationCapacity { get; set; }
        [Required]
        public int UsedAccomodationCapacity { get; set; }
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        public string ContactPhone { get; set; }
    }
}
