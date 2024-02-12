using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;
using System;

namespace NMAS.WebApi.Unit.Tests.CustomAutoDataFixture
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base(() =>
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Customize(new FilterDateOfBirthCustomization());

            // ensuring DateOfBirth falls within a specific range
            fixture.Customize<IllegalMigrantEntityDocument>(c => c
                .With(x => x.DateOfBirth, fixture.Create<DateTime>().Date));

            return fixture;
        })
        {
        }
    }
}
