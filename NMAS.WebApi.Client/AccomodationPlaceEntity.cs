using System.ComponentModel.DataAnnotations;

namespace NMAS.WebApi.Client
{
    public class AccommodationPlaceEntity
    {
        [Required]
        public int WorkerID { get; set; }
        [Required]
        public string PlaceName { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public int AccommodationCapacity { get; set; }
        [Required]
        public int UsedAccommodationCapacity { get; set; }
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        public string ContactPhone { get; set; }
    }
}
