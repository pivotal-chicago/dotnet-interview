using System.Linq;
using FraudDomain.Model;

namespace FraudAPI.Database
{
    public static class AddressDataInitializer
    {
        public static void Initialize(FraudulentAddressContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
//            if (context.Addresses.Any())
//            {
//                return; // DB has been seeded
//            }

            context.Add(new FraudulentAddress
            {
                StreetNumber = "111",
                Street = "Main",
                City = "Evanston",
                State = "IL",
                ZIP = "60201",
                CaseId = "SomeCaseId1"
            });
            context.Add(new FraudulentAddress
            {
                StreetNumber = "222",
                Street = "Main",
                City = "Evanston",
                State = "IL",
                ZIP = "60201",
                CaseId = "SomeCaseId2"
            });
            context.Add(new FraudulentAddress
            {
                StreetNumber = "333",
                Street = "Main",
                City = "Evanston",
                State = "IL",
                ZIP = "60201",
                CaseId = "SomeCaseId3"
            });
            context.SaveChanges();
        }
    }
}