using System;
using System.Collections.Generic;
using System.Text;

namespace FraudDomain.Model
{
    public class ResidentialAddress
    {
        
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }

        protected bool Equals(ResidentialAddress other)
        {
            return string.Equals(Street, other.Street) && string.Equals(StreetNumber, other.StreetNumber) && string.Equals(City, other.City) && string.Equals(State, other.State) && string.Equals(ZIP, other.ZIP);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ResidentialAddress) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Street != null ? Street.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StreetNumber != null ? StreetNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (State != null ? State.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ZIP != null ? ZIP.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
