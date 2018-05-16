using System.Collections.Generic;
using System.Text;
using FraudDomain.Dto;
using FraudDomain.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FraudDomain.Service
{
    public class VisaApplicationCheckerServiceTests
    {
        //            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

        [Fact]
        public void ValidateMatchesAgainstAnAddressInDatabase()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var svc = new VisaApplicationCheckerService(db);
                var address = new FraudulentAddress { };
                db.Addresses.Add(address);
                db.SaveChanges();
                MatchResult matchResult = svc.validate(new VisaApplication {Id = "13", Address = address});
                Assert.Equal(Fields.Address, matchResult.MatchingField);
                Assert.Equal(FraudStatus.Matched, matchResult.FraudStatus);
                Assert.Equal("13", matchResult.ApplicationId);
                Assert.Equal(address.CaseId, matchResult.CaseId);
            }
        }
    }
}

