using FraudDomain.Model;
using FraudDomain.Service;
using Microsoft.AspNetCore.Mvc;

namespace FraudAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/VisaApplication")]
    public class VisaApplicationController : Controller
    {

        private readonly FraudulentAddressService service;

        public VisaApplicationController(FraudulentAddressService service)
        {
            this.service = service;
        }

        [HttpPost]
        public VisaFraudResponse Post([FromBody]VisaApplicationRequest request)
        {
            return service.IsFradulentVisaApplication(request);
        }

        [HttpGet]
        [Route("ping")]
        public string Ping()
        {
            return "Go";
        }
    }
}