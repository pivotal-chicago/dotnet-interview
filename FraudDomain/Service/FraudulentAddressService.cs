using System;
using System.Collections.Generic;
using System.Linq;
using FraudDomain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FraudDomain.Service
{
    public interface IVisaApplicationChecker
    {
        MatchResponse ValidateApplication(VisaApplication application);
    }

    public class VisaApplication
    {
        public string id;

        public Address address;
    }

    public class Address
    {
        public string streetNumber;
        
        public string streetAddress;

        public string city;

        public string state;

        public string zip;
    }

    public struct MatchResponse
    {
        [JsonProperty("application-id")]
        public string applicationId;

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("fraud-status")]
        public FraudStatus status;

        [JsonProperty("matching-field")]
        public string matchingField;

        [JsonProperty("case-id")]
        public string caseId;
    }

    public enum FraudStatus
    {
        Matched,
        NotMatched
    }

public interface IFraudulentAddressCrudOperations
    {
    }

    public class FraudulentAddressService : IFraudulentAddressCrudOperations
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
    }
}