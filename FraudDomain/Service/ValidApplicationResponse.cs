using System;

namespace FraudDomain.Service
{
    public class ValidApplicationResponse
    {
        public int ApplicationId {get; set; }
        public String FraudStatus { get; set; }
    }
}