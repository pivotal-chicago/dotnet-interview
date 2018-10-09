using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FraudDomain.Model;
using FraudDomain.Request.Model;
using FraudDomain.Response.Model;

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

        public const string FRAUD_NOT_MATCHED ="CLEAR";
        public const string FRAUD_MATCHED="MATCHED";
        public const string FRAUD_FIELD = "ADDRESS";

        public VisaApplicantRespsonse isApplicantFraud(VisaApplicantRequest visaApplicantRequest)
        {
           VisaApplicantRespsonse visaApplicantResponse = new VisaApplicantRespsonse{ApplicationId=visaApplicantRequest.Id ,FraudStatus = FRAUD_NOT_MATCHED };
           foreach (FraudulentAddress retrievedFradulentAddress in this.All())
            {
                if(isFradulentAddessSameAsApplicantAddress(visaApplicantRequest.Address,retrievedFradulentAddress))
                {
                 visaApplicantResponse.FraudStatus = FRAUD_MATCHED;
                 visaApplicantResponse.MatchingField = FRAUD_FIELD;
                 visaApplicantResponse.CaseId = retrievedFradulentAddress.CaseId;
                 return visaApplicantResponse;
                }
            }
           return visaApplicantResponse;
        }

         private VisaApplicantRequestAddress constructVisaApplicantRequestAddressFromFraudulentAddress(FraudulentAddress fraudulentAddress)
        {
           return new VisaApplicantRequestAddress
            {
                City = fraudulentAddress.City,
                State = (USState)Enum.Parse(typeof(USState), fraudulentAddress.State),
                Street = fraudulentAddress.StreetNumber+" "+fraudulentAddress.Street,
                Zip = fraudulentAddress.ZIP
            };
        }

        private Boolean isFradulentAddessSameAsApplicantAddress(VisaApplicantRequestAddress applicantAddress,FraudulentAddress retrievedFradulentAddress)
        {
            var constructedVisaApplAddrFromFradulentAddress = this.constructVisaApplicantRequestAddressFromFraudulentAddress(retrievedFradulentAddress);
            return applicantAddress.Equals(constructedVisaApplAddrFromFradulentAddress);
        }

       

    }
}