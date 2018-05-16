using System;
using System.Linq;
using FraudDomain.Dto;
using FraudDomain.Model;

namespace FraudDomain.Service
{
    public class VisaApplicationCheckerService : IVisaApplicationCheckerService
    {
        private readonly FraudulentAddressContext db;

        public VisaApplicationCheckerService(FraudulentAddressContext db)
        {
            this.db = db;
        }

        public MatchResult validate(VisaApplication application)
        {
            var matchingAddress = db.Addresses.FirstOrDefault(address => address.Equals(application.Address));
            if (matchingAddress != null)
            {
                // it's a fraud!
                return new MatchResult
                {
                    ApplicationId = application.Id,
                    CaseId = matchingAddress.CaseId,
                    FraudStatus = FraudStatus.Matched,
                    MatchingField = Fields.Address
                };
            }
            else
            {
                // it's legit!
                throw new NotImplementedException();
            }
        }
    }
}