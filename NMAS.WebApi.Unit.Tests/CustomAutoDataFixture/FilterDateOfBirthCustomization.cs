using AutoFixture;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using System;

namespace NMAS.WebApi.Unit.Tests.CustomAutoDataFixture
{
    public class FilterDateOfBirthCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<FilterDateOfBirth>(composer => composer
                .With(x => x.From, DateTime.Now.AddYears(-10))
                .With(x => x.To, DateTime.Now));
        }
    }
}
