using System;

namespace FraudDomain.Model
{
    public class FraudulentAddress : ResidentialAddress
    {
        public int Id { get; set; }
       // public ResidentialAddress Address { get; set; }
        public string CaseId { get; set; }

        protected bool Equals(FraudulentAddress other)
        {
                return base.Equals(other) &&  Id == other.Id && string.Equals(CaseId, other.CaseId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FraudulentAddress) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
             //   hashCode = (hashCode * 397) ^ (Address != null ? Address.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CaseId != null ? CaseId.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}