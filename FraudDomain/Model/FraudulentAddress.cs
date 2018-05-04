using System;

namespace FraudDomain.Model
{
    public class FraudulentAddress
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string CaseId { get; set; }

        protected bool Equals(FraudulentAddress other)
        {
            return Id == other.Id && string.Equals(Street, other.Street) && string.Equals(StreetNumber, other.StreetNumber) && string.Equals(City, other.City) && string.Equals(State, other.State) && string.Equals(ZIP, other.ZIP);
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
                hashCode = (hashCode * 397) ^ (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StreetNumber != null ? StreetNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (State != null ? State.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ZIP != null ? ZIP.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}