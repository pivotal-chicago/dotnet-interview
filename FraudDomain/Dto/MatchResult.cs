using System;
using System.Collections.Generic;
using System.Text;

namespace FraudDomain.Dto
{
    public class MatchResult
    {
        public string ApplicationId { get; set; }
        public FraudStatus FraudStatus { get; set; }
        public Fields MatchingField { get; set; }
        public string CaseId { get; set; }

        protected bool Equals(MatchResult other)
        {
            return string.Equals(ApplicationId, other.ApplicationId) && FraudStatus == other.FraudStatus && MatchingField == other.MatchingField && string.Equals(CaseId, other.CaseId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MatchResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ApplicationId != null ? ApplicationId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) FraudStatus;
                hashCode = (hashCode * 397) ^ (int) MatchingField;
                hashCode = (hashCode * 397) ^ (CaseId != null ? CaseId.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    public enum Fields
    {
        Address
    }


    public enum FraudStatus
    {
        Matched
    }
}
