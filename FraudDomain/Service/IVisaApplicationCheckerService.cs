using FraudDomain.Dto;
using FraudDomain.Model;

namespace FraudDomain.Service
{
    public interface IVisaApplicationCheckerService
    {
        MatchResult validate(VisaApplication application);
    }
}