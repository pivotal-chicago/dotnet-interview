using System;
using System.Collections.Generic;
using System.Text;
using FraudDomain.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace FraudDomain.Service
{
    public class ApplicationValidationServiceTest
    {
        readonly DbContextOptionsBuilder<FraudulentAddressContext> _builder =
            new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

        [Fact]
        public void ShouldReturnValidWhenAddressIsNotInDatabase()
        {
            using (var db = new FraudulentAddressContext(_builder.Options))
            {
                var service = new ApplicationValidationService(db);

                var actualResponse = service.Validate(new VisaApplication
                {
                    Id = 123,
                    Address = new ApplicationAddress
                    {
                        City = "Other",
                        State = "OK",
                        Street = "Something",
                        Zipcode = "60606"
                    }
                });

                var expectedResponse = JsonConvert.SerializeObject(new ValidApplicationResponse
                {
                    ApplicationId = 123,
                    FraudStatus = FraudStatus.Clear
                });

                Assert.Equal(expectedResponse, actualResponse);
            }
        }

        [Fact]
        public void ShouldReturnInvalidWhenAddressMatchesExactlyInDatabase()
        {
            using (var db = new FraudulentAddressContext(_builder.Options))
            {
                db.Addresses.Add(new FraudulentAddress
                {
                    CaseId = "CaseId111",
                    City = "Evanston",
                    State = "IL",
                    Street = "Main",
                    StreetNumber = "111",
                    ZIP = "60201"
                });
                db.SaveChanges();

                var service = new ApplicationValidationService(db);

                var actualResponse = service.Validate(new VisaApplication
                {
                    Id = 125,
                    Address = new ApplicationAddress
                    {
                        City = "Evanston",
                        State = "IL",
                        Street = "111 Main",
                        Zipcode = "60201"
                    }
                });

                var expectedResponse = JsonConvert.SerializeObject(new InvalidApplicationResponse
                {
                    ApplicationId = 125,
                    FraudStatus = FraudStatus.Matched,
                    CaseId = "CaseId111",
                    MatchingField = "ADDRESS"
                });

                Assert.Equal(expectedResponse, actualResponse);
            }
        }

        [Fact]
        public void ShouldReturnInvalidWhenAddressMatchesWithDifferentCaseInDatabase()
        {
            using (var db = new FraudulentAddressContext(_builder.Options))
            {
                db.Addresses.Add(new FraudulentAddress
                {
                    CaseId = "CaseId111",
                    City = "Evanston",
                    State = "IL",
                    Street = "Main",
                    StreetNumber = "111",
                    ZIP = "60201"
                });
                db.SaveChanges();

                var service = new ApplicationValidationService(db);

                var actualResponse = service.Validate(new VisaApplication
                {
                    Id = 125,
                    Address = new ApplicationAddress
                    {
                        City = "EVANSTON",
                        State = "il",
                        Street = "111 MAIN",
                        Zipcode = "60201"
                    }
                });

                var expectedResponse = JsonConvert.SerializeObject(new InvalidApplicationResponse
                {
                    ApplicationId = 125,
                    FraudStatus = FraudStatus.Matched,
                    CaseId = "CaseId111",
                    MatchingField = "ADDRESS"
                });

                Assert.Equal(expectedResponse, actualResponse);
            }
        }
    }
}
