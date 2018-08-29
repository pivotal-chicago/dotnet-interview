using FraudDomain.Model;

namespace FraudDomain.Service
{
    public interface IVisaValidator
    {
        string Validate(VisaApplication request);
    }
}