using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FraudDomain.Service
{
    public interface IVisaValidator
    {
        string Validate(JObject visaApplication);
    }
}
