﻿using System;

namespace NMAS.WebApi.Repositories.Models.IllegalMigrantEntity
{
    public class IllegalMigrantEntityDocument
    {
        public int Id { get; set; }
        public int? AccommodationPlaceId { get; set; }
        public string PersonalIdentityCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string OriginCountry { get; set; }
        public string Religion { get; set; }
    }
}
