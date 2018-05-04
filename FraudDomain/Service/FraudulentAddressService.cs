using System;
using System.Collections.Generic;
using System.Linq;
using FraudDomain.Model;

namespace FraudDomain.Service
{
    public class FraudulentAddressService
    {
        private readonly FraudulentAddressContext db;
        public const string MatchingFieldAddress = "Address";

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

        public VisaFraudResponse IsFradulentVisaApplication(VisaApplicationRequest request)
        {
            if (request.Address == null)
            {
                throw new ArgumentNullException("Address");
            }

            FraudulentAddress address = db.Addresses
                .FirstOrDefault(a => request.Address.Street.Equals(a.StreetNumber + " " + a.Street, StringComparison.CurrentCultureIgnoreCase)
                    && request.Address.City.Equals(a.City, StringComparison.CurrentCultureIgnoreCase)
                    && request.Address.ZIP.Equals(a.ZIP, StringComparison.CurrentCultureIgnoreCase)
                    && request.Address.State.Equals(a.State, StringComparison.CurrentCultureIgnoreCase));

            var response = new VisaFraudResponse
            {
                FraudStatus = FraudStatus.Clear,
                ApplicationId = request.Id
            };

            if (address == null)
            {
                return response;
            }

            response.FraudStatus = FraudStatus.Matched;
            response.CaseId = address.CaseId;
            response.MatchingField = MatchingFieldAddress;

            return response;
        }
    }
}