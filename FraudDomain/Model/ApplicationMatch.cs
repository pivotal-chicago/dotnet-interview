using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FraudDomain.Model
{
    public class ApplicationMatch
    {
        public string ApplicationId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public eFraudStatus FraudStatus { get; set; }
        public string MatchingField { get; set; }   //this is probably a constant
        public string CaseId { get; set; }
    }

    public enum eFraudStatus
    {
        MATCHED = 0,
        CLEAR = 1
    }
}
