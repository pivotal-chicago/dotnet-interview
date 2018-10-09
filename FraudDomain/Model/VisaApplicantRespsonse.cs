using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FraudDomain.Response.Model
{
    /**
     * {
    application-id: application id from the submitted visa application,
    fraud-status: "MATCHED",
    matching-field: "ADDRESS"
    case-id: case id from the matching Fraudulent Address record
}
     * */
    public class VisaApplicantRespsonse
    {
        [JsonProperty("application-id")]
        public string ApplicationId { get; set; }
        [JsonProperty("fraud-status")]
        public string FraudStatus { get; set; }
        [JsonProperty("matching-field")]
        public string MatchingField { get; set; }
        [JsonProperty("case-id")]
        public string CaseId {get;set;}

        public override bool Equals(object obj)
        {
            var respsonse = obj as VisaApplicantRespsonse;
            return respsonse != null &&
                   ApplicationId == respsonse.ApplicationId &&
                   FraudStatus == respsonse.FraudStatus &&
                   MatchingField == respsonse.MatchingField &&
                   CaseId == respsonse.CaseId;
        }

        public override int GetHashCode()
        {
            var hashCode = 1225346741;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ApplicationId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FraudStatus);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MatchingField);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CaseId);
            return hashCode;
        }

     }
}
