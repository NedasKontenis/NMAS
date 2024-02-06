using System;
using System.ComponentModel.DataAnnotations;

namespace NMAS.WebApi.Client
{
    public class IllegalMigrantEntity
    {
        [Required]
        public int AccomodationPlaceID { get; set; }
        [Required]
        public string PersonalIdentityCode { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string OriginCountry { get; set; }
        [Required]
        public string Religion { get; set; }

    }
}
