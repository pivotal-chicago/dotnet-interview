using System;
using System.Collections.Generic;
using System.Linq;
using FraudDomain.Model;
using Microsoft.EntityFrameworkCore;

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

        public FraudulentAddress MatchAddress(string Street,string City,string State,string ZIP)
        {
            return db.Addresses.AsNoTracking().FirstOrDefault(p => p.StreetNumber + " " + p.Street == Street
                                                                   && p.City == City
                                                                   && p.State == State
                                                                   && p.ZIP == ZIP);
        }

        public FraudulentAddress MatchAddress(string StreetNumber,string Street,string City,string State,string ZIP)
        {
            return db.Addresses.AsNoTracking().FirstOrDefault(p => p.Street == Street
                                                          && p.StreetNumber == StreetNumber
                                                          && p.City == City
                                                          && p.State == State
                                                          && p.ZIP == ZIP);
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
    }
}