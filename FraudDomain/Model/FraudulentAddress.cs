using System;

namespace FraudDomain.Model
{
    public class FraudulentAddress : Address
    {
        public int Id { get; set; }
        public string CaseId { get; set; }

        protected bool Equals(FraudulentAddress other)
        {
            return Id == other.Id && CaseId == other.CaseId && base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FraudulentAddress) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ base.GetHashCode();
                return hashCode;
            }
        }
    }
}