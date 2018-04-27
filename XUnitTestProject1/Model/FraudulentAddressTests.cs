using System.Linq;
using FraudDomain.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FraudDomain
{
    public class FraudulentAddressTests
    {
        [Fact]
        public void ShouldBeEqualWhenItMakesSense()
        {
            var address = new FraudulentAddress
            {
                Street = "Main Street",
                StreetNumber = "123",
                City = "Chicago",
                State = "IL",
                ZIP = "60001"
            };

            var address2 = new FraudulentAddress
            {
                Street = "Main Street",
                StreetNumber = "123",
                City = "Chicago",
                State = "IL",
                ZIP = "60001"
            };

            Assert.Equal(address, address2);
        }

        [Fact]
        public void ShouldCreateANewFraudulentAddress()
        {
            var address = new FraudulentAddress
            {
                Street = "Main Street",
                StreetNumber = "123",
                City = "Chicago",
                State = "IL",
                ZIP = "60001"
            };

            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var allAddresses = db.Addresses;
                Assert.Empty(allAddresses);
                
                allAddresses.Add(address);
                db.SaveChanges();

                allAddresses = db.Addresses;
                Assert.Single(allAddresses);
                Assert.All(allAddresses, addressFromDb => Assert.Equal(address, addressFromDb));
            }
        }
    }
}
