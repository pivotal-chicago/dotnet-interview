using System.Linq;
using FraudDomain.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FraudDomain.Service
{
    public class FraudulentAddressServiceTests
    {
        [Fact]
        public void ShouldSaveAnAddress()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);
                var address = new FraudulentAddress{City = "Bangor", State = "ME", ZIP="09886"};
                service.Save(address);
                Assert.Equal(address, db.Addresses.Find(address.Id));
            }
        }

        [Fact]
        public void ShouldReadAnAddress()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);
                var address = new FraudulentAddress{City = "Bangor", State = "ME", ZIP="09886"};

                db.Addresses.Add(address);
                db.SaveChanges();
                var fromDb = service.Find(address.Id);
                Assert.Equal(address, fromDb);
            }
        }

        [Fact]
        public void ShouldReadAllAddresses()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);
                var address1 = new FraudulentAddress{City = "Bangor", State = "ME", ZIP="09886"};
                db.Addresses.Add(address1);
                var address2 = new FraudulentAddress{City = "Bangor", State = "ME", ZIP="09886"};
                db.Addresses.Add(address2);
                var address3 = new FraudulentAddress{City = "Bangor", State = "ME", ZIP="09886"};
                db.Addresses.Add(address3);
                db.SaveChanges();

                var fromDb = service.All().ToList();
                Assert.Contains(address1, fromDb);
                Assert.Contains(address2, fromDb);
                Assert.Contains(address3, fromDb);
            }
        }

        [Fact]
        public void ShouldDeleteAnAddress()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);
                var address = new FraudulentAddress{City = "Bangor", State = "ME", ZIP="09886"};

                db.Addresses.Add(address);
                db.SaveChanges();
                var fromDb = service.Find(address.Id);
                Assert.NotNull(fromDb);
                service.Delete(fromDb.Id);
                var deleted = db.Addresses.Find(fromDb.Id);
                Assert.Null(deleted);
            }
        }

        [Fact]
        public void ShouldHandleDeleteRequestOfNonexistentAddress()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);
                service.Delete(999);

            }
        }

        [Fact]
        public void ShouldUpdateAnAddress()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);
                var originalAddress = new FraudulentAddress{City = "Bangor", State = "ME", ZIP="09886"};
                
                service.Save(originalAddress);
                originalAddress.StreetNumber = "1234W";
                originalAddress.Street = "Oakton St";

                // Because the test database is in-memory and reads just return the same object
                var clone = new FraudulentAddress
                {
                    Id = originalAddress.Id,
                    StreetNumber = originalAddress.StreetNumber,
                    Street = originalAddress.Street,
                    City = originalAddress.City,
                    State = originalAddress.State,
                    ZIP = originalAddress.ZIP
                };

                service.Save(originalAddress);

                var updated = db.Addresses.Find(originalAddress.Id);
                Assert.Equal(updated, clone);
            }

        }
    }
}
