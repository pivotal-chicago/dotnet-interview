using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FraudDomain.Dto;
using FraudDomain.Model;
using FraudDomain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FraudAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/visa-application/check")]
    public class VisaApplicationCheckController : Controller
    {
        private readonly IVisaApplicationCheckerService checker;

        public VisaApplicationCheckController(IVisaApplicationCheckerService checker)
        {
            this.checker = checker;
        }
        public MatchResult Post([FromBody]VisaApplication application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application), "missing application");
            return checker.validate(application);
        }
    }
}