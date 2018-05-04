using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FraudDomain.Model
{
    public class VisaFraudResponse : IEquatable<VisaFraudResponse>
    {
        [JsonProperty(PropertyName = "fraud-status")]
//        [JsonConverter(EnumConverter)]
        public FraudStatus FraudStatus { get; set; }

        [JsonProperty(PropertyName = "application-id")]
        public string ApplicationId { get; set; }

        [JsonProperty(PropertyName = "matching-field")]
        public string MatchingField { get; set; }

        [JsonProperty(PropertyName = "case-id")]
        public string CaseId { get; set; }

        public bool Equals(VisaFraudResponse response)
        {

            return response.FraudStatus == this.FraudStatus
                   && string.Equals(response.ApplicationId, this.ApplicationId)
                   && string.Equals(response.MatchingField, this.MatchingField)
                   && string.Equals(response.CaseId, this.CaseId);
        }
    }
}
