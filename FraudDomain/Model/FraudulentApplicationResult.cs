using Newtonsoft.Json;

namespace FraudDomain.Model
{
    public class FraudulentApplicationResult
    {
        [JsonProperty("application-id")]
        public string Id { get; set; }
        [JsonProperty("fraud-status")]
        public string FraudStatus { get; set; }
        [JsonProperty("matching-field", NullValueHandling=NullValueHandling.Ignore)]
        public string MatchingField { get; set; }
        [JsonProperty("case-id", NullValueHandling=NullValueHandling.Ignore)]
        public string CaseId { get; set; }
    }
}