using System;
using System.Collections.Generic;
using System.Text;

namespace FraudDomain.Model
{
    public class VisaApplication
    {
        public int Id { get; set; }
        public ApplicationAddress Address { get; set; }
    }

    public class ApplicationAddress
    {
        public String Street { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Zipcode { get; set; }
    }
}
