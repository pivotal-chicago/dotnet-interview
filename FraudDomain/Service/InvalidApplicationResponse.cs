using System;

namespace FraudDomain.Service
{
    public class InvalidApplicationResponse
    {
        public int ApplicationId {get; set; }
        public String FraudStatus { get; set; }
        public String MatchingField { get; set; }
        public String CaseId { get; set; }
    }
}