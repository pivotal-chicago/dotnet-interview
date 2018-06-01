using System;
using System.Collections.Generic;
using System.Linq;
using FraudDomain.DTOs;
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

        public ApplicationMatch Search(VisaApplicationDto  dto)
        {
            var visaApplication = VisaApplication.FromDto(dto);

            var inputAddress = visaApplication.Address;
            var matchingAddress = db.Addresses.FirstOrDefault(r => r.Street == inputAddress.Street && r.State == inputAddress.State && r.ZIP == inputAddress.ZIP && r.StreetNumber == inputAddress.StreetNumber && r.City == inputAddress.City);

            if (matchingAddress != null)
            {

                var response = new ApplicationMatch
                {
                    ApplicationId = visaApplication.Id,
                    FraudStatus = eFraudStatus.MATCHED,
                    MatchingField = "ADDRESS",
                    CaseId = matchingAddress.CaseId
                };

                return response;
            }

            var noMatch = new ApplicationMatch
            {
                ApplicationId = visaApplication.Id,
                FraudStatus = eFraudStatus.CLEAR
            };

            return noMatch;
        }
    }
}