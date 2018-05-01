using System;
using FraudDomain.Service;
using Microsoft.AspNetCore.Mvc;

namespace FraudAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/VisaCheck")]
    public class VisaCheckController : Controller
    {
        private readonly IVisaApplicationChecker visaApplicationChecker;

        public VisaCheckController(IVisaApplicationChecker visaApplicationChecker)
        {
            this.visaApplicationChecker = visaApplicationChecker;
        }

        [HttpGet]
        public string GetSomeData()
        {
            return "Here's something for you.";
        }

        [HttpPost]
        public MatchResponse Post([FromBody]VisaApplication application)
        {
            return visaApplicationChecker.ValidateApplication(application);
        }
    }
}