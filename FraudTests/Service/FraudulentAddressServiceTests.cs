using System.Configuration;
using System.Linq;
using FraudDomain.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FraudAPI.Database;

namespace FraudDomain.Service
{
    public class FraudulentAddressServiceTests
    {
        /*
         * Add [ClassInitialize()] for bulider
         */

        [Fact]
        public void ShouldSaveAnAddress()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);
                var address = new FraudulentAddress { City = "Bangor", State = "ME", ZIP = "09886" };
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
                var address = new FraudulentAddress { City = "Bangor", State = "ME", ZIP = "09886" };

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
                var address1 = new FraudulentAddress { City = "Bangor", State = "ME", ZIP = "09886" };
                db.Addresses.Add(address1);
                var address2 = new FraudulentAddress { City = "Bangor", State = "ME", ZIP = "09886" };
                db.Addresses.Add(address2);
                var address3 = new FraudulentAddress { City = "Bangor", State = "ME", ZIP = "09886" };
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
                var address = new FraudulentAddress { City = "Bangor", State = "ME", ZIP = "09886" };

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
                var originalAddress = new FraudulentAddress { City = "Bangor", State = "ME", ZIP = "09886" };

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

        [Fact]
        public void ShouldMatchAddress()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                AddressDataInitializer.Initialize(db);

                var service = new FraudulentAddressService(db);
                string streetnumber = "111",
                    street = "Main",
                    city = "Evanston",
                    state = "IL",
                    zip = "60201";

                var address = service.MatchAddress(streetnumber,street,city,state,zip);
                Assert.NotNull(address);

            }

        }

        [Fact]
        public void ShouldUserDbLookup()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=FraudData;Integrated Security=true;ConnectRetryCount=0;Trusted_Connection=True;MultipleActiveResultSets=true");
            using (var db = new FraudulentAddressContext(builder.Options))
            {
                AddressDataInitializer.Initialize(db);
                string StreetNumber = "111",
                    Street = "Main",
                    City = "Evanston",
                    State = "IL",
                    ZIP = "60201",
                    CaseId = "SomeCaseId1";

                var addresses = db.Addresses.AsNoTracking().Where(p => p.Street == Street
                                                                       && p.StreetNumber == StreetNumber
                                                                       && p.City == City
                                                                       && p.State == State
                                                                       && p.ZIP == ZIP).ToList();
                Assert.Single(addresses);
            }
        }

        /*
        [Fact]
        public void ShouldFindAnAddress()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);
                var originalAddress = new FraudulentAddress{City = "Bangor", State = "ME", ZIP="09886"};
                
                service.Save(originalAddress);

                var clone = new FraudulentAddress
                {
                    StreetNumber = originalAddress.StreetNumber,
                    Street = originalAddress.Street,
                    City = originalAddress.City,
                    State = originalAddress.State,
                    ZIP = originalAddress.ZIP
                };


                var updated = db.Addresses.Find(originalAddress.Id);
                Assert.Equal(updated, clone);
            }
        }
        */
    }
}
