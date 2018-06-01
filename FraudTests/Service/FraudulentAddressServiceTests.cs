using System.Linq;
using FraudDomain.DTOs;
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
                var address = CreateFraudulentAddress();
                service.Save(address);
                Assert.Equal(address, db.Addresses.Find(address.Id));
            }
        }

        private static FraudulentAddress CreateFraudulentAddress()
        {
            return new FraudulentAddress {City = "Bangor", State = "ME", ZIP = "09886", CaseId = "SomeCaseId"};
        }

        [Fact]
        public void ShouldReadAnAddress()
        {
            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);
                var address = CreateFraudulentAddress();

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
                var address1 = CreateFraudulentAddress();
                db.Addresses.Add(address1);

                var address2 = CreateFraudulentAddress();
                db.Addresses.Add(address2);

                var address3 = CreateFraudulentAddress();
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
                var address = CreateFraudulentAddress();

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
                var originalAddress = CreateFraudulentAddress();

                service.Save(originalAddress);
                originalAddress.StreetNumber = "1234W";
                originalAddress.Street = "Oakton St";

                service.Save(originalAddress);

                var updated = db.Addresses.Find(originalAddress.Id);
                Assert.Equal(updated, originalAddress);
            }
        }

        //should be able to get the address from the endpoint : post 
        //We need to verify that the input address is in the right format:
        //id
        //address object
        //we need to compare our input address to what we get from our fraudulent address service. Service available and the get all method
        //We need to use a search or find method on the address service for the exact match. 
        //if there is match , we return the match object
        //otherwise, we return the no-match object


        [Fact]
        public void OnApplicationSubmissionIfAddressAvailableShouldReturnMatchingResponse()
        {
            var visaApplication = new VisaApplicationDto
            {
                Street = "2222 Main St",
                City = "Chicago",
                State = "IL",
                ZIP = "60602",
                Id = "testId"
            };


            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);

                var inDatabase = new FraudulentAddress
                {
                    CaseId = "SomeOtherCaseId",
                    Street = "Main St",
                    StreetNumber = "2222",
                    City = "Chicago",
                    State = "IL",
                    ZIP = "60602"
                };
                service.Save(inDatabase);

                var result = service.Search(visaApplication);

                Assert.Equal(visaApplication.Id, result.ApplicationId);
                Assert.Equal(eFraudStatus.MATCHED, result.FraudStatus);
                Assert.Equal("ADDRESS", result.MatchingField);
                Assert.Equal(inDatabase.CaseId, result.CaseId);
            }
        }

        [Fact]
        public void OnApplicationSubmissionIfAddressAvailableShouldReturnNoMatchResponse()
        {
            var visaApplication = new VisaApplicationDto
            {
                Street = "2222 Main St",
                City = "Evanston",
                State = "IL",
                ZIP = "60602",
                Id = "testId"
            };


            visaApplication.Id = "testId";

            var builder = new DbContextOptionsBuilder<FraudulentAddressContext>().UseInMemoryDatabase("unitTestDb");

            using (var db = new FraudulentAddressContext(builder.Options))
            {
                var service = new FraudulentAddressService(db);

                var inDatabase = new FraudulentAddress
                {
                    Street = "Main",
                    StreetNumber = "2222",
                    City = "Chicago",
                    State = "IL",
                    ZIP = "60602",
                    CaseId = "SomeOtherCaseId"
                };

                service.Save(inDatabase);

                var result = service.Search(visaApplication);

                Assert.Equal(eFraudStatus.CLEAR, result.FraudStatus);
                // Assert.Equal(inDatabase.CaseId, result.CaseId);}
            }
        }
    }
}