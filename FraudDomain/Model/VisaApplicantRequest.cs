using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FraudDomain.Request.Model
{
    /**
     * {
    id: application id from the submitted visa application,
    address: {
        street: "123 Main Street",
        city: "Chicago",
        state: IL,
        zip: "60001"
    }
}
     * */
    public class VisaApplicantRequest
    {
        [JsonProperty("id")]
        public string Id{ get;set;}
        public VisaApplicantRequestAddress  Address{get;set;}

        public override bool Equals(object obj)
        {
            var request = obj as VisaApplicantRequest;
            return request != null &&
                   Id == request.Id &&
                   EqualityComparer<VisaApplicantRequestAddress>.Default.Equals(Address, request.Address);
        }

        public override int GetHashCode()
        {
            var hashCode = -306707981;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<VisaApplicantRequestAddress>.Default.GetHashCode(Address);
            return hashCode;
        }
    }

    public class VisaApplicantRequestAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public USState State {get;set;}
        public string Zip { get; set; }

        public override bool Equals(object obj)
        {
            var address = obj as VisaApplicantRequestAddress;
            return address != null &&
                   string.Equals(Street,address.Street, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(City,address.City, StringComparison.OrdinalIgnoreCase) &&
                   State == address.State &&
                  string.Equals(Zip,address.Zip, StringComparison.OrdinalIgnoreCase) ;
        }

        public override int GetHashCode()
        {
            var hashCode = 1825761482;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Street);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(City);
            hashCode = hashCode * -1521134295 + State.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Zip);
            return hashCode;
        }
    }

    public enum USState
    {
        IL
    }
}
