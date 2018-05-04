using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FraudDomain.Model;

namespace FraudAPI.Database
{
    public static class AddreessDataInitializer
    {
        public static void Initialize(FraudulentAddressContext context)
        {
            context.Database.EnsureCreated();

            // Look for any data.
            if (context.Addresses.Any())
            {
                return; // DB has been seeded
            }

            context.Add(new FraudulentAddress
            {
                StreetNumber = "111",
                Street = "Main",
                City = "Evanston",
                State = "IL",
                ZIP = "60201"
            });
            context.Add(new FraudulentAddress
            {
                StreetNumber = "222",
                Street = "Main",
                City = "Evanston",
                State = "IL",
                ZIP = "60201"
            });
            context.Add(new FraudulentAddress
            {
                StreetNumber = "333",
                Street = "Main",
                City = "Evanston",
                State = "IL",
                ZIP = "60201"
            });
            context.SaveChanges();
        }
    }
}