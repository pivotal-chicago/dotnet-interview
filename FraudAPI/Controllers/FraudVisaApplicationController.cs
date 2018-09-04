using FraudDomain.Model;
using FraudDomain.Service;
using Microsoft.AspNetCore.Mvc;

namespace FraudAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/CheckFradulentVisaApplication")]
    public class FraudVisaApplicationController : Controller
    {
        private readonly FraudulentAddressService service;

        public FraudVisaApplicationController(FraudulentAddressService service)
        {
            this.service = service;
        }

        [HttpPost]
        public FraudulentApplicationResult Post([FromBody]VisaApplication value)
        {
            FraudulentApplicationResult result = new FraudulentApplicationResult
            {
                Id = value.Id,
                FraudStatus = "CLEAR"
            };
            FraudulentAddress fraudAddress = service.ValidateFradulentAddress(value.Address);
            if (fraudAddress != null)
            {
                result.FraudStatus = "MATCHED";
                result.MatchingField = "Address";
                result.CaseId = fraudAddress.CaseId;
            }
            return result;
        }
    }
}