using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FraudDomain.Request.Model;
using FraudDomain.Response.Model;
using FraudDomain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FraudAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/VisaApplication")]
    public class VisaApplicantFraudDetectionController : Controller
    {

        private readonly FraudulentAddressService service;

        public VisaApplicantFraudDetectionController(FraudulentAddressService service)
        {
            this.service = service;
        }

        [HttpPost("validateApplicantIsFraudClear")]
        public VisaApplicantRespsonse ValidatingIfApplicantIsNotFraud([FromBody]VisaApplicantRequest visaApplicantRequest)
        {
            return this.service.isApplicantFraud(visaApplicantRequest);
        }
    }
}