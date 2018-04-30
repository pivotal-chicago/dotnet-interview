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

            address2.Street = "Maple Avenue";
            Assert.NotEqual(address, address2);
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

        [Fact]
        public void ShouldDeleteAddress()
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
                db.Addresses.Add(address);
                db.SaveChanges();
                Assert.Single(db.Addresses);
                db.Remove(address);
                db.SaveChanges();
                Assert.Empty(db.Addresses);
            }
        }
    }
}
