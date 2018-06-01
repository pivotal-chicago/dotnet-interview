using System;
using System.Collections.Generic;
using System.Text;

namespace FraudDomain.DTOs
{
    public class VisaApplicationDto
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string Id { get; set; }
    }
}
