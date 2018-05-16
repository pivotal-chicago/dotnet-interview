using System;
using System.Collections.Generic;
using System.Text;

namespace FraudDomain.Model
{
    public class VisaApplication
    {
        public string Id { get; set; }
        public Address Address {
            get;
            set;
        }

        protected bool Equals(VisaApplication other)
        {
            return string.Equals(Id, other.Id) && Equals(Address, other.Address);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VisaApplication) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Id != null ? Id.GetHashCode() : 0) * 397) ^ (Address != null ? Address.GetHashCode() : 0);
            }
        }
    }
}
