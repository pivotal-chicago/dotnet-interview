using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FraudDomain.Model;

namespace FraudDomain.Service
{
    public class FraudulentAddressService
    {
        private readonly FraudulentAddressContext db;

        public FraudulentAddressService(FraudulentAddressContext db)
        {
            this.db = db;
        }

        public void Save(FraudulentAddress address)
        {
            try
            {
                db.Addresses.Add(address);
                db.SaveChanges();
            }
            catch (ArgumentException)
            {
                db.Addresses.Update(address);
                db.SaveChanges();
            }
        }

        public FraudulentAddress Find(int addressId)
        {
            return db.Addresses.Find(addressId);
        }

        public void Delete(int addressId)
        {
            var address = db.Addresses.Find(addressId);
            if (address != null)
            {
                db.Addresses.Remove(address);
                db.SaveChanges();
            }
        }

        public IEnumerable<FraudulentAddress> All()
        {
            return db.Addresses.ToList();
        }

        public FraudulentAddress ValidateFradulentAddress(FraudulentAddress address)
        {
            return db.Addresses.FirstOrDefault(fromDb => 
                fromDb.StreetNumber.Equals(address.StreetNumber) &&
                fromDb.Street.Equals(address.Street) &&
                fromDb.City.Equals(address.City) &&
                fromDb.State.Equals(address.State) &&
                fromDb.ZIP.Equals(address.ZIP));
        }
    }
}