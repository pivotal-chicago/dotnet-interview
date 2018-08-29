using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FraudDomain.Model;
using FraudDomain.Service;
using Microsoft.AspNetCore.Mvc;

namespace FraudAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/validate")]
    public class VisaApplicationController : Controller
    {
        private readonly IVisaValidator _applicationValidationService;

        public VisaApplicationController(IVisaValidator applicationValidationService)
        {
            _applicationValidationService = applicationValidationService;
        }

        [HttpPost]
        public string Validate([FromBody]VisaApplication request)
        {
            return _applicationValidationService.Validate(request);
        }
    }
}
