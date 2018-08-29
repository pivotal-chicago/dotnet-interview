using System;
using System.Linq;
using FraudDomain.Model;

namespace FraudDomain.Service
{
    public class ApplicationValidationService: IVisaValidator
    {
        private readonly FraudulentAddressContext _db;

        public ApplicationValidationService(FraudulentAddressContext db)
        {
            _db = db;
        }

        public string Validate(VisaApplication request)
        {
            var matchingAddresses = _db.Addresses
                .Where(address =>
                    (address.StreetNumber + " " + address.Street).Equals(request.Address.Street,
                        StringComparison.InvariantCultureIgnoreCase) &&
                    address.City.Equals(request.Address.City, StringComparison.InvariantCultureIgnoreCase) &&
                    address.State.Equals(request.Address.State, StringComparison.InvariantCultureIgnoreCase) &&
                    address.ZIP.Equals(request.Address.Zipcode, StringComparison.InvariantCultureIgnoreCase)
                );

            if (matchingAddresses.Any())
            {
                //return JsonConvert.SerializeObject(new InvalidApplicationResponse
                //{
                //    ApplicationId = request.Id,
                //    FraudStatus = FraudStatus.Matched,
                //    CaseId = matchingAddresses.First().CaseId,
                //    MatchingField = "ADDRESS"
                //});
                return "";
            }

//            return JsonConvert.SerializeObject(new ValidApplicationResponse
//            {
//                ApplicationId = request.Id,
//                FraudStatus = FraudStatus.Clear
//            });
            return "";
        }
    }
}