using FraudDomain.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FraudAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/VisaApplication")]
    public class VisaApplicationController : Controller
    {
        private readonly IVisaValidator _validator;

        private JObject response = new JObject();

        public VisaApplicationController(IVisaValidator validator)
        {
            _validator = validator;
        }

        public JObject Verify(JObject visaApplication)
        {
            var caseId = _validator.Validate(visaApplication);
            response["application-id"] = visaApplication["id"];
            if (string.IsNullOrEmpty(caseId))
            {
                response["fraud-status"] = "CLEAR";
                return response;
            }
            else
            {
                response["fraud-status"] = "MATCHED";
                response["matching-field"] = "ADDRESS";
                response["case-id"] = caseId;
            }

            return response;
        }
    }
}